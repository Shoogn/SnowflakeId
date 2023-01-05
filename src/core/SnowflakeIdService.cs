/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */


using Microsoft.Extensions.Options;
using SnowflakeId.Core.Options;
using System;

namespace SnowflakeId.Core
{
    public class SnowflakeIdService : ISnowflakeService
    {
        // Lock Token
        private readonly object threadLock = new object();

        private long _lastTimestamp = -1L;
        private long _sequence = 0L;

        // result is 22
        const int _timeStampShift = SnowflakeIdOptionBuilder.TotalBits - SnowflakeIdOptionBuilder.EpochBits;

        // result is 12
        const int _machaineIdShift = SnowflakeIdOptionBuilder.TotalBits - SnowflakeIdOptionBuilder.EpochBits - SnowflakeIdOptionBuilder.MachineIdBits;

        private readonly SnowflakOptions _snowflakOptions;

        public SnowflakeIdService(IOptions<SnowflakOptions> options)
        {
            _snowflakOptions = options.Value;
        }

        public virtual long GenerateSnowflakeId()
        {
            lock (threadLock)
            {
                long currentTimestamp = getTimestamp();

                if (currentTimestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException("Error_In_The_Server_Clock");
                }

                if (currentTimestamp == _lastTimestamp)
                {
                    // generate a new timestamp when the _sequence is reached the ( 4096 - 1 )

                    _sequence = (_sequence + 1) & SnowflakeIdOptionBuilder.MaxSequenceId;

                    if (_sequence == 0)
                    {
                        currentTimestamp = waitToGetNextMillis(currentTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = currentTimestamp;

                var result = (currentTimestamp << _timeStampShift) | ((long)_snowflakOptions.DataCenterId << _machaineIdShift) | (_sequence);

                return result;
            }
        }

   
        private long getTimestamp()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private long waitToGetNextMillis(long currentTimestamp)
        {
            while (currentTimestamp == _lastTimestamp)
            {
                currentTimestamp = getTimestamp();
            }
            return currentTimestamp;
        }

        // Your Epoch Start at 1970 Jan 1s ( Unix Time )
        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
  
    }
}
