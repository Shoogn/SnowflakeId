/*
GNU GENERAL PUBLIC LICENSE
Version 3, 29 June 2007
Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.
*/

using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SnowflakeId.Core;
using Microsoft.Extensions.Logging.Abstractions;


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
