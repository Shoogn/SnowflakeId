// Read more about the licenses under the root of the project in the LICENSE.txt file.

namespace SnowflakeId.Core
{
    public class SnowflakOptions
    {
        /// <summary>
        /// This could be data center id or server id, 
        /// or any other unique things in your webfarm
        /// Be sure to set this property with number greater than zero.
        /// </summary>
        public int DataCenterId { get; set; }

        /// <summary>
        /// Get or set the value that determined whether using console log or not, the default value is <c>false</c>.
        /// </summary>
        public bool UseConsoleLog { get; set; }
    }
}
