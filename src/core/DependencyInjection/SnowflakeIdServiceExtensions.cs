// Read more about the licenses under the root of the project in the LICENSE.txt file./

using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SnowflakeId.Core;

namespace SnowflakeId.DependencyInjection
{
    public static class SnowflakeIdServiceExtensions
    {
        public static IServiceCollection AddSnowflakeUniqueId(this IServiceCollection services, 
            Action<SnowflakOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.TryAddScoped<ISnowflakeService, SnowflakeIdService>();
            services.Configure(setupAction);
            return services;
        }
    }
}
