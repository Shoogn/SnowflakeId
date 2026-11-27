// Read more about the licenses under the root of the project in the LICENSE.txt file.

using SnowflakeId.Core.Events;
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

        /// <summary>
        /// Get or set how many milliseconds the system clock is allowed to move backwards before
        /// id generation throws. When a backward move is within this tolerance, generation waits
        /// for the clock to catch up instead of failing. The default value is <c>0</c>, meaning any
        /// backward clock movement throws immediately.
        /// </summary>
        public long ClockBackwardnessToleranceMillis { get; set; } = 0;

        /// <summary>
        /// events
        public SnowflakeIdEvents Events { get; set; } = new SnowflakeIdEvents();

    }
}
