// Read more about the licenses under the root of the project in the LICENSE.txt file.

using System;

namespace SnowflakeId.Core
{
    public class SnowflakOptions
    {
        /// <summary>
        /// This could be data center id or server id, 
        /// or any other unique things in your webfarm
        /// Be sure to set this property with number greater than zero.
        /// </summary>
        public int? DataCenterId { get; set; }

        /// <summary>
        /// Get or set the value that determined whether using console log or not, the default value is <c>false</c>.
        /// </summary>
        public bool UseConsoleLog { get; set; }

        /// <summary>
        /// Get or set the custom epoch, the default value of this property with set to epoch is 1970 Jan 1s ( Unix Time ) if value is null.
        /// </summary>
        public DateTime? CustomEpoch { get; set; }
    }
}
