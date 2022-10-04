/*
 
 Build and implemented with love by Mohammed Ahmed Hussien Babiker
 
 */


using System;

namespace SnowflakeId.Core
{
    public class SnowflakeIdService
    {
        // Lock Token
        private readonly object threadLock = new object();

        private long _lastTimestamp = -1L;
        private long _sequence = 0L;
        private readonly int _machaineId;


        // result is 22
        const int _timeStampShift = SnowflakeIdOptionBuilder.TotalBits - SnowflakeIdOptionBuilder.EpochBits;

        // result is 12
        const int _machaineIdShift = SnowflakeIdOptionBuilder.TotalBits - SnowflakeIdOptionBuilder.EpochBits - SnowflakeIdOptionBuilder.MachineIdBits;

        public SnowflakeIdService(int machaineId)
        {
            _machaineId = machaineId;
        }
       // public SnowflakeIdOptionBuilder Options { get; }

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

                var result = (currentTimestamp << _timeStampShift) | ((long)_machaineId << _machaineIdShift) | (_sequence);

                return result;
            }
        }

        #region Helper Functions
        private long getTimestamp()
        {
            return (long)(DateTime.UtcNow - Jan1st2020).TotalMilliseconds;
        }

        private long waitToGetNextMillis(long currentTimestamp)
        {
            while (currentTimestamp == _lastTimestamp)
            {
                currentTimestamp = getTimestamp();
            }
            return currentTimestamp;
        }

        // Your Epoch Start at 2020 Jan 1s 
        private static DateTime Jan1st2020 = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        #endregion
    }
}
