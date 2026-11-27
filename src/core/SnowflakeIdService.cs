// Read more about the licenses under the root of the project in the LICENSE.txt file.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SnowflakeId.Core.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnowflakeId.Core
{
    public class SnowflakeIdService : ISnowflakeService, IDisposable
    {
        // Single lock shared by the sync and async generation paths so they can never
        // interleave access to _lastTimestamp / _sequence.
        private readonly SemaphoreSlim sem = new SemaphoreSlim(1, 1);

        private long _lastTimestamp = -1L;
        private long _sequence = 0L;

        // result is 22. Derived from MachineIdBits + SequenceBits so the
        // most significant bit of the id is always left as an unset sign bit.
        const int _timeStampShift = SnowflakeIdConfig.MachineIdBits + SnowflakeIdConfig.SequenceBits;

        // result is 12
        const int _machaineIdShift = SnowflakeIdConfig.SequenceBits;

        private readonly SnowflakOptions _snowflakOptions;
        private readonly ILogger<SnowflakeIdService> _logger;
        protected readonly int DataCenterId = 1;
        private static readonly DateTime DefaultEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        /// <summary>
        /// When generating the Id <see cref="SnowflakeIdService"/> I use the  Epoch that start at 1970 Jan 1s ( Unix Time )
        /// </summary>
        public readonly DateTime UnixEpoch;

        private bool _disposed;
        public SnowflakeIdService(IOptions<SnowflakOptions> options, ILogger<SnowflakeIdService> logger)
        {
            _snowflakOptions = options.Value;
            _logger = logger ?? new NullLogger<SnowflakeIdService>();

            UnixEpoch = options.Value.CustomEpoch ?? DefaultEpoch;
            if (UnixEpoch > DateTime.UtcNow)
            {
                throw new ArgumentException("CustomEpoch must not be in the future.");
            }
            _snowflakOptions.DataCenterId ??= DataCenterId;
            if (_snowflakOptions.DataCenterId < 0 || _snowflakOptions.DataCenterId > SnowflakeIdConfig.MaxMachineId)
            {
                throw new ArgumentException(string.Format("DataCenterId must be between 0 and {0}", SnowflakeIdConfig.MaxMachineId));
            }
        }

        /// <summary>
        /// Generate new unique number, it's length is 19 digits.
        /// </summary>
        /// <returns>A new unique number that has a long type.</returns>
        public virtual long GenerateSnowflakeId()
        {
            var idCreatingContext = new SnowflakeIdCreatingContext
            {
                DataCenterId = _snowflakOptions.DataCenterId.Value,
                DefaultEpoch = UnixEpoch
            };
            _snowflakOptions.Events.CreatingSnowflakeId(idCreatingContext).GetAwaiter().GetResult();

            long result;
            sem.Wait();
            try
            {
                result = generateIdCore();
            }
            finally
            {
                sem.Release();
            }

            var idCreatedContext = new SnowflakeIdCreatedContext
            {
                Id = result,
                GeneratedDateTime = GetDateTimeByMillisecondsSinceUnixEpoch(GetMillisecondsSinceUnixEpochFromId(result)),
                DataCenterId = _snowflakOptions.DataCenterId.Value,
                DefaultEpoch = UnixEpoch
            };
            _snowflakOptions.Events.CreatedSnowflakeId(idCreatedContext).GetAwaiter().GetResult();

            return result;
        }

        /// <summary>
        /// Generate new unique number, it's length is 19 digits asynchrony.
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>A new unique number that has a long type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual async Task<long> GenerateSnowflakeIdAsync(CancellationToken cancellationToken = default)
        {
            var idCreatingContext = new SnowflakeIdCreatingContext
            {
                DataCenterId = _snowflakOptions.DataCenterId.Value,
                DefaultEpoch = UnixEpoch
            };
            await _snowflakOptions.Events.CreatingSnowflakeId(idCreatingContext).ConfigureAwait(false);

            long result;
            await sem.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                result = generateIdCore();
            }
            finally
            {
                sem.Release();
            }

            var idCreatedContext = new SnowflakeIdCreatedContext
            {
                Id = result,
                GeneratedDateTime = GetDateTimeByMillisecondsSinceUnixEpoch(GetMillisecondsSinceUnixEpochFromId(result)),
                DataCenterId = _snowflakOptions.DataCenterId.Value,
                DefaultEpoch = UnixEpoch
            };
            await _snowflakOptions.Events.CreatedSnowflakeId(idCreatedContext).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Advances <see cref="_lastTimestamp"/> / <see cref="_sequence"/> and computes the next id.
        /// Must only be called while holding <see cref="sem"/>.
        /// </summary>
        private long generateIdCore()
        {
            long currentTimestamp = getTimestamp();

            if (currentTimestamp < _lastTimestamp)
            {
                long driftMillis = _lastTimestamp - currentTimestamp;
                if (driftMillis > _snowflakOptions.ClockBackwardnessToleranceMillis)
                {
                    if (_snowflakOptions.UseConsoleLog)
                        _logger.LogError("error in the server clock, the current timestamp should be bigger than generated one, " +
                            "current timestamp is: {CurrentTimestamp}, and the last generated timestamp is: {LastTimestamp}", currentTimestamp, _lastTimestamp);
                    throw new InvalidOperationException("Error_In_The_Server_Clock");
                }

                if (_snowflakOptions.UseConsoleLog)
                    _logger.LogWarning("the server clock moved backwards by {DriftMillis}ms, within the configured tolerance; waiting for it to catch up", driftMillis);
                currentTimestamp = waitUntilTimestampAtLeast(_lastTimestamp);
            }

            if (currentTimestamp > SnowflakeIdConfig.MaxTimestamp)
            {
                throw new InvalidOperationException("Error_Timestamp_Overflow");
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
                _logger.LogInformation("the generated unique id is {UniqueId}", result);

            return result;
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

            long millisecondsSinceUnixEpoch = GetMillisecondsSinceUnixEpochFromId(snowflakeId);
            DateTime snowflakeIdGeneratedTime = GetDateTimeByMillisecondsSinceUnixEpoch(millisecondsSinceUnixEpoch);

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
        /// Method to return at which time the id is generated, by passing the milliseconds since the UnixEpoch.
        /// </summary>
        /// <param name="millisecondsSinceUnixEpoch">milliseconds since the UnixEpoch</param>
        /// <returns>A <see cref="DateTime"/> struct.</returns>
        public virtual DateTime GetDateTimeByMillisecondsSinceUnixEpoch(long millisecondsSinceUnixEpoch)
        {
            if (millisecondsSinceUnixEpoch <= 0)
            {
                return UnixEpoch;
            }

            DateTime dateTime = UnixEpoch.AddMilliseconds(millisecondsSinceUnixEpoch);
            DateTime time = dateTime.ToLocalTime();
            return time;
        }

        /// <summary>
        /// Method return milliseconds since the UnixEpoch from a given generated snowflake id.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId</param>
        /// <returns>The milliseconds as a long data type.</returns>
        public virtual long GetMillisecondsSinceUnixEpochFromId(long snowflakeId)
        {
            if (snowflakeId <= 0)
            {
                return 0L;
            }

            long timeStamp = (snowflakeId >> _timeStampShift) & SnowflakeIdConfig.MaxTimestamp;
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
            SpinWait spinWait = default;
            while (currentTimestamp == _lastTimestamp)
            {
                spinWait.SpinOnce();
                currentTimestamp = getTimestamp();
            }
            return currentTimestamp;
        }

        private long waitUntilTimestampAtLeast(long target)
        {
            SpinWait spinWait = default;
            long currentTimestamp = getTimestamp();
            while (currentTimestamp < target)
            {
                spinWait.SpinOnce();
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
                    sem.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
