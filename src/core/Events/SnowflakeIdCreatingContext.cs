// Read more about the licenses under the root of the project in the LICENSE.txt file.

using System;
namespace SnowflakeId.Core.Events;

public class SnowflakeIdCreatingContext
{
    /// <summary>
    /// Gets or sets the default epoch (1970 Jan 1st - Unix Time).
    /// Default value is 1970 Jan 1st if the <see cref="SnowflakOptions.CustomEpoch"/> is null."/>
    /// </summary>
    public DateTime DefaultEpoch { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the data center.
    /// </summary>
    public int DataCenterId { get; set; }
}
