// Read more about the licenses under the root of the project in the LICENSE.txt file.

using System;
using System.Threading.Tasks;
namespace SnowflakeId.Core.Events;

public class SnowflakeIdEvents
{
    /// <summary>
    /// Invoke before creating a snowflakeId.
    /// </summary>
    public Func<SnowflakeIdCreatingContext, Task> OnCreatingSnowflakeId { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoke after a snowflakeId has been created.
    /// </summary>
    public Func<SnowflakeIdCreatedContext, Task> OnCreatedSnowflakeId { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked before creating a snowflakeId.
    /// </summary>
    public virtual Task CreatingSnowflakeId(SnowflakeIdCreatingContext context)
    {
        if (OnCreatingSnowflakeId != null)
        {
            return OnCreatingSnowflakeId.Invoke(context);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Invoked after a snowflakeId has been created.
    /// </summary>
    public virtual Task CreatedSnowflakeId(SnowflakeIdCreatedContext context)
    {
        if (OnCreatedSnowflakeId != null)
        {
            return OnCreatedSnowflakeId.Invoke(context);
        }
        return Task.CompletedTask;
    }
}
