// Read more about the licenses under the root of the project in the LICENSE.txt file.

using System;
using System.Threading.Tasks;
using System.Threading;

namespace SnowflakeId.Core
{
    /// <summary>
    /// An abstraction define methods to generate a new unique ids.
    /// </summary>
    public interface ISnowflakeService
    {
        /// <summary>
        /// Generate new unique number, it's length is 19 digits.
        /// </summary>
        /// <returns>A new unique number that has a long type.</returns>
        long GenerateSnowflakeId();


        /// <summary>
        /// Generate new unique number, it's length is 19 digits asynchrony.
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>A new unique number that has a long type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<long> GenerateSnowflakeIdAsync(CancellationToken cancellationToken =default);

        /// <summary>
        /// A method caculated the generate date time for a given generated snowflake id.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId.</param>
        /// <returns>A SnowflakeId object that hold the id and the generated date time.</returns>
        SnowflakeId GetSnowflakeById(long snowflakeId);

        /// <summary>
        /// Methos to return at which time the id is generated.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId.</param>
        /// <returns>The generated date time as a <see cref="DateTime"/> struct.</returns>
        DateTime GetGeneratedDateTimeBySnowflakeId(long snowflakeId);

        /// <summary>
        /// Methos to return at which time the id is generated py passing the seconds Since UnixEpoch.
        /// </summary>
        /// <param name="secondsSinceUnixEpoch">seconds Since UnixEpoch.</param>
        /// <returns>A <see cref="DateTime"/> struct.</returns>
        DateTime GetDateTimeBySecondsSinceUnixEpoch(long secondsSinceUnixEpoch);

        /// <summary>
        /// Method return seconds since the UnixEpoch from a given generated snowflake id.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId.</param>
        /// <returns>The seconds as a long data type.</returns>
        long GetSecondsSinceUnixEpochFromId(long snowflakeId);

        /// <summary>
        /// A method return at which data cennter id the snowflake id is generated.
        /// </summary>
        /// <param name="snowflakeId">snowflakeId.</param>
        /// <returns>Data center id which has int data type.</returns>
        int GetDataCenterIdBySnowflakeId(long snowflakeId);
    }
}
