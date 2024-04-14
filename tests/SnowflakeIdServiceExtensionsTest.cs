/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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


            Assert.Throws<ArgumentNullException>(() => services.AddSnowflakeUniqueId(null));
        }

        [Fact]
        public void Can_Add_SnowflakeId_To_Service_Collections_With_SnowflakOptions()
        {
            var services = new ServiceCollection();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();
            var snowflakeIdOptionGetter = serviceProvider.GetRequiredService<IOptions<SnowflakOptions>>();
            var snowflakeIdOption = snowflakeIdOptionGetter.Value;

            Assert.NotNull(snowflakeService);
            Assert.Equal(1, snowflakeIdOption.DataCenterId);
        }

        [Fact]
        public void AddSnowflakeUniqueId_Allow_Chaining()
        {
            var services = new ServiceCollection();

            Assert.Same(services, services.AddSnowflakeUniqueId(_ => { }));
        }
    }
}
