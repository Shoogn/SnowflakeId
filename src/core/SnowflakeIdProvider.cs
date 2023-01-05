/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System;

namespace SnowflakeId.Provider
{
    public class SnowflakeIdProvider : ISnowflakeIdProvider
    {
        /// <summary>
        /// When generating the Id <see cref="SnowflakeIdService"/> I use the  Epoch that start at 1970 Jan 1s ( Unix Time )
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public SnowflakeId GetDateTimeBySnowflakeId(long snowflakeId)
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
            return result;

        }

        public DateTime GetDateTimeBySecondsSinceUnixEpoch(long secondsSinceUnixEpoch)
        {
            if (secondsSinceUnixEpoch <= 0)
            {
                return UnixEpoch;
            }

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(secondsSinceUnixEpoch);
            DateTime time = dateTime.ToLocalTime();
            return time;
        }


        public long GetSecondsSinceUnixEpochFromId(long snowflakeId)
        {
            if (snowflakeId <= 0)
            {
                return 0L;
            }

            string result = Convert.ToString(snowflakeId, 2).PadLeft(64, '0');
            string getTimestampInbit = result.Substring(1, 41);
            long timestamp = Convert.ToInt64(getTimestampInbit, 2);
            return timestamp;
        }


    }
}
