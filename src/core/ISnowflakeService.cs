/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */


using System;

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
        int GetDataCenterIdBySnowflakId(long snowflakeId);
    }
}
