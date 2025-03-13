// Read more about the licenses under the root of the project in the LICENSE.txt file.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnowflakeId.Core
{
    public class SnowflakeIdService : ISnowflakeService, IDisposable
    {
        // Lock Token
        private readonly object threadLock = new object();

        private long _lastTimestamp = -1L;
        private long _sequence = 0L;

        // result is 22
        const int _timeStampShift = SnowflakeIdConfig.TotalBits - SnowflakeIdConfig.EpochBits;

        // result is 12
        const int _machaineIdShift = SnowflakeIdConfig.TotalBits - SnowflakeIdConfig.EpochBits - SnowflakeIdConfig.MachineIdBits;

        private readonly SnowflakOptions _snowflakOptions;
        private readonly ILogger<SnowflakeIdService> _logger;

        /// <summary>
        /// When generating the Id <see cref="SnowflakeIdService"/> I use the  Epoch that start at 1970 Jan 1s ( Unix Time )
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private bool _disposed;
        public SnowflakeIdService(IOptions<SnowflakOptions> options, ILogger<SnowflakeIdService> logger)
        {
            _snowflakOptions = options.Value;
            _logger = logger ?? new NullLogger<SnowflakeIdService>();
        }

        /// <summary>
        /// Generate new unique number, it's length is 19 digits.
        /// </summary>
        /// <returns>A new unique number that has a long type.</returns>
        public virtual long GenerateSnowflakeId()
        {
            lock (threadLock)
            {
                long currentTimestamp = getTimestamp();

                if (currentTimestamp < _lastTimestamp)
                {
                    if (_snowflakOptions.UseConsoleLog)
                        _logger.LogError("error in the server clock, the current timestamp should be bigger than generated one, current timestamp is: {0}, and the last generated timestamp is: {1}", currentTimestamp, _lastTimestamp);
                    throw new InvalidOperationException("Error_In_The_Server_Clock");
                }

                if (currentTimestamp == _lastTimestamp)
                {
                    // generate a new timestamp when the _sequence is reached the ( 4096 - 1 )
                    _sequence = (_sequence + 1) & SnowflakeIdConfig.MaxSequenceId;

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

                long result = (currentTimestamp << _timeStampShift) | ((long)_snowflakOptions.DataCenterId << _machaineIdShift) | (_sequence);
                if (_snowflakOptions.UseConsoleLog)
                    _logger.LogInformation("the gnerated unique id is {0}", result);
                return result;
            }
        }

        /// <summary>
        /// Generate new unique number, it's length is 19 digits asynchrony.
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>A new unique number that has a long type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual Task<long> GenerateSnowflakeIdAsync(CancellationToken cancellationToken = default)
        {
            SemaphoreSlim sem = new SemaphoreSlim(1, 1);
            try
            {
                sem.Wait(cancellationToken);
                long currentTimestamp = getTimestamp();

                if (currentTimestamp < _lastTimestamp)
                {
                    if (_snowflakOptions.UseConsoleLog)
                        _logger.LogError("error in the server clock, the current timestamp should be bigger than generated one, current timestamp is: {0}, and the last generated timestamp is: {1}", currentTimestamp, _lastTimestamp);
                    throw new InvalidOperationException("Error_In_The_Server_Clock");
                }

                if (currentTimestamp == _lastTimestamp)
                {
                    // generate a new timestamp when the _sequence is reached the ( 4096 - 1 )
                    _sequence = (_sequence + 1) & SnowflakeIdConfig.MaxSequenceId;

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

                long result = (currentTimestamp << _timeStampShift) | ((long)_snowflakOptions.DataCenterId << _machaineIdShift) | (_sequence);
                if (_snowflakOptions.UseConsoleLog)
                    _logger.LogInformation("the gnerated unique id is {0}", result);
                return Task.FromResult(result);
            }
            finally
            {
                sem.Release();
            }


        }

        /// <summary>
        /// A method caculated the generate date time for a given generated snowflake id.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId</param>
        /// <returns>A SnowflakeId object that hold the id and the generated date time.</returns>
        public virtual SnowflakeId GetSnowflakeById(long snowflakeId)
        {
            SnowflakeId result = new SnowflakeId() { Id = 0, GeneratedDateTime = UnixEpoch };

            if (snowflakeId <= 0)
            {
                return result;
            }

            long secondsSinceUnixEpoch = GetSecondsSinceUnixEpochFromId(snowflakeId);
            DateTime snowflakeIdGeneratedTime = GetDateTimeBySecondsSinceUnixEpoch(secondsSinceUnixEpoch);

            result.GeneratedDateTime = snowflakeIdGeneratedTime;
            result.Id = snowflakeId;
            result.DataCenterId = GetDataCenterIdBySnowflakeId(snowflakeId);
            return result;

        }

        /// <summary>
        /// Methos to return at which time the id is generated.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId</param>
        /// <returns>The generated date time as a <see cref="DateTime"/> struct.</returns>
        public virtual DateTime GetGeneratedDateTimeBySnowflakeId(long snowflakeId)
        {
            return GetSnowflakeById(snowflakeId).GeneratedDateTime;
        }

        /// <summary>
        /// Methos to return at which time the id is generated py passing the seconds Since UnixEpoch.
        /// </summary>
        /// <param name="secondsSinceUnixEpoch">seconds Since UnixEpoch</param>
        /// <returns>A <see cref="DateTime"/> struct.</returns>
        public virtual DateTime GetDateTimeBySecondsSinceUnixEpoch(long secondsSinceUnixEpoch)
        {
            if (secondsSinceUnixEpoch <= 0)
            {
                return UnixEpoch;
            }

            DateTime dateTime = UnixEpoch.AddMilliseconds(secondsSinceUnixEpoch);
            DateTime time = dateTime.ToLocalTime();
            return time;
        }

        /// <summary>
        /// Method return seconds since the UnixEpoch from a given generated snowflake id.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId</param>
        /// <returns>The seconds as a long data type.</returns>
        public virtual long GetSecondsSinceUnixEpochFromId(long snowflakeId)
        {
            if (snowflakeId <= 0)
            {
                return 0L;
            }

            // 41 bits of 1s, will shifted left by 22 bits.
            long timestampMask = 0x1FFFFFFFFFF; 
            long timeStamp = (snowflakeId >> _timeStampShift) & timestampMask;
            return timeStamp;
        }

        /// <summary>
        /// A method return at which data cennter id the snowflake id is generated.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId.</param>
        /// <returns>Data center id which has int data type.</returns>
        public virtual int GetDataCenterIdBySnowflakeId(long snowflakeId)
        {
            if (snowflakeId <= 0)
            {
                return 0;
            }

            // 10 bits mask (0b1111111111) will shifted left by 12 bits.
            long dataCenterIdMask = 0x3FF; 

            long dataCenterId = (snowflakeId >> _machaineIdShift) & dataCenterIdMask;
            return (int)dataCenterId;
        }


        private long getTimestamp()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
        }

        private long waitToGetNextMillis(long currentTimestamp)
        {
            while (currentTimestamp == _lastTimestamp)
            {
                currentTimestamp = getTimestamp();
            }
            return currentTimestamp;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // no-op.
                }
                _disposed = true;
            }
        }
    }
}
