// Read more about the licenses under the root of the project in the LICENSE.txt file.

using System;

namespace SnowflakeId.Core
{
    public class SnowflakeId
    {
        /// <summary>
        /// Get or set snowflakeId.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Get or set the generation datetime of the snowflakeId.
        /// </summary>
        public DateTime GeneratedDateTime { get; set; }

        /// <summary>
        /// Get or set the data centerid.
        /// </summary>
        public int DataCenterId { get; set; }
    }
}
