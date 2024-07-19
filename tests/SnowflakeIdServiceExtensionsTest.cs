/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;

namespace SnowflakeId.Tests
{
    public class SnowflakeIdServiceExtensionsTest
    {
        [Fact]

        public void AddSnowflakeUniqueId_ThrowArgumentNullException_With_Null_SnowflakOptions()
        {
            var services = new ServiceCollection();
            services.AddLogging();

            Assert.Throws<ArgumentNullException>(() => services.AddSnowflakeUniqueId(null));
        }

        [Fact]
        public void Can_Add_SnowflakeId_To_Service_Collections_With_SnowflakOptions()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();
            var snowflakeIdOptionGetter = serviceProvider.GetRequiredService<IOptions<SnowflakOptions>>();
            var snowflakeIdOption = snowflakeIdOptionGetter.Value;

            Assert.NotNull(snowflakeService);
            Assert.Equal(1, snowflakeIdOption.DataCenterId);
        }

        [Fact]
        public void Can_Replace_SnowflakeId_Default_Registration_By_Creating_Object_That_Implement_ISnowflakeService()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddScoped(typeof(ISnowflakeService), sp => Mock.Of<ISnowflakeService>());

            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();

            var snowflakeId = services.FirstOrDefault(desc => desc.ServiceType ==  typeof(ISnowflakeService));

            Assert.NotNull(snowflakeId);
            Assert.Equal(ServiceLifetime.Scoped, snowflakeId.Lifetime);
           // Assert.IsType<SnowflakeIdService>(serviceProvider.GetRequiredService<ISnowflakeService>());
        }

        [Fact]
        public void AddSnowflakeUniqueId_Allow_Chaining()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            Assert.Same(services, services.AddSnowflakeUniqueId(_ => { }));
        }
    }
}
