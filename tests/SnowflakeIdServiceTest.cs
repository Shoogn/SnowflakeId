using Microsoft.Extensions.DependencyInjection;
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;
using System.Collections.Generic;

namespace SnowflakeId.Tests
{
    public class SnowflakeIdServiceTest
    {
        [Fact]
        public void Can_Genertate_UniqueId()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);
            
            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            var id = snowflakeService.GenerateSnowflakeId();
            var idLength = id.ToString().Length == 19;

            Assert.IsType<long>(id);
            Assert.True(idLength);
        }

        [Fact]
        public async void Can_Genertate_UniqueId_Asynchrony()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            List<long> uniqueIds = new List<long>();
            var cts = new CancellationTokenSource();
            for (int i = 0; i < 100; i++)
            {
                var id = await snowflakeService.GenerateSnowflakeIdAsync(cts.Token);
                var idLength = id.ToString().Length == 19;
                uniqueIds.Add(id);
                Assert.IsType<long>(id);
                Assert.True(idLength);
            }
            var listUniqeness = uniqueIds.Count != uniqueIds.Distinct().Count();
            Assert.False(listUniqeness);
        }

        [Fact]
        public void Can_Find_Data_CenterId_From_Generated_UniqueId()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 17);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            var id = snowflakeService.GenerateSnowflakeId();
            var dataCenterId = snowflakeService.GetDataCenterIdBySnowflakeId(id);
            Assert.IsType<int>(dataCenterId);
            Assert.Equal(17, dataCenterId);

        }

    }
}
