// Read more about the licenses under the root of the project in the LICENSE.txt file./

using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SnowflakeId.Core;
using Microsoft.Extensions.Configuration;

namespace SnowflakeId.DependencyInjection
{
    public static class SnowflakeIdServiceExtensions
    {
        public static IServiceCollection AddSnowflakeUniqueId(this IServiceCollection services, 
            Action<SnowflakOptions> setupAction = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            var o = new SnowflakOptions();
            services.AddSingleton(o);
            setupAction?.Invoke(o);

            services.TryAddScoped<ISnowflakeService, SnowflakeIdService>();
            if (setupAction != null)
                services.Configure(setupAction);
            return services;
        }


        public static IServiceCollection AddSnowflakeUniqueId(this IServiceCollection services, 
            IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddOptions();
            var snowflakeIdSection = configuration.GetSection("SnowflakeId");
            if (!snowflakeIdSection.Exists())
            {
                throw new ArgumentException("The SnowflakeId section is not found in the configuration.");
            }
            services.Configure<SnowflakOptions>(snowflakeIdSection);
            services.TryAddScoped<ISnowflakeService, SnowflakeIdService>();
            return services;
        }

    }
}
