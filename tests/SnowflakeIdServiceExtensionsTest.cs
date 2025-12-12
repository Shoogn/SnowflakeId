/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.Extensions.Configuration;
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
        public void AddSnowflakId_Without_SnowflakeIdOption_And_RegistersService()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId();

            var serviceProvider = services.BuildServiceProvider();

            var snowflakeId = services.FirstOrDefault(desc => desc.ServiceType == typeof(ISnowflakeService));
            var snowflakeIdOptionGetter = serviceProvider.GetRequiredService<IOptions<SnowflakOptions>>();
            var snowflakeIdOption = snowflakeIdOptionGetter.Value;

            Assert.NotNull(snowflakeId);
            Assert.Equal(ServiceLifetime.Scoped, snowflakeId.Lifetime);
            Assert.Null(snowflakeIdOption.DataCenterId);
        }

        [Fact]
        public void AddSnowflakId_With_SnowflakeIdOption_And_RegistersService()
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
        public void AddSnowflakId_And_Binding_Configuration_And_RegistersService()
        {
            var inMemorySettings = new Dictionary<string, string> 
            {
                {"SnowflakeId:DataCenterId", "3"},
                {"SnowflakeId:UseConsoleLog", "true"},
                {"SnowflakeId:CustomEpoch", "2025-01-01T00:00:00Z"},
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();
            var snowflakeIdOptionGetter = serviceProvider.GetRequiredService<IOptions<SnowflakOptions>>();
            var snowflakeIdOption = snowflakeIdOptionGetter.Value;
            Assert.NotNull(snowflakeService);
            Assert.Equal(3, snowflakeIdOption.DataCenterId);
            Assert.True(snowflakeIdOption.UseConsoleLog);
            Assert.Equal(DateTime.Parse("2025-01-01T00:00:00Z").ToUniversalTime(), snowflakeIdOption.CustomEpoch?.ToUniversalTime());

        }

        [Fact]
        public void AddSnowflakId_Throe_When_Missing_Binding_Configuration()
        {
            // empty config intentionally
            IConfiguration config = new ConfigurationBuilder().Build();
            var services = new ServiceCollection();

            var ex = Assert.Throws<ArgumentException>(() => services.AddSnowflakeUniqueId(config));
            Assert.Contains("SnowflakeId", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Replace_SnowflakeId_Default_Registration_By_Creating_Object_That_Implement_ISnowflakeService()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddScoped(typeof(ISnowflakeService), sp => Mock.Of<ISnowflakeService>());

            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();

            var snowflakeId = services.FirstOrDefault(desc => desc.ServiceType ==  typeof(ISnowflakeService));

            Assert.NotNull(snowflakeId);
            Assert.Equal(ServiceLifetime.Scoped, snowflakeId.Lifetime);
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
