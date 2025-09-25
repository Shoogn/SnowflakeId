using Microsoft.Extensions.DependencyInjection;
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;

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
        public async Task Can_Genertate_UniqueId_Asynchrony()
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

        [Fact]
        public async Task Can_Genertate_UniqueId_Asynchrony_In_The_Same_Millisecond()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            var cts = new CancellationTokenSource();
            var tasks = new List<Task<long>>(500_000);
            for (int i = 0; i < 50_000; i++)
                tasks.Add(Task.Run(() => snowflakeService.GenerateSnowflakeIdAsync(cts.Token)));

            var uniqueIds = await Task.WhenAll(tasks);
            var listUniqeness = uniqueIds.Length != uniqueIds.Distinct().Count();
            Assert.False(listUniqeness);
        }


        [Fact]
        public void DataCenterId_Is_Equal_To_One_When_Registering_SnowflakeId_WithoutOptions()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSnowflakeUniqueId();

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            var id = snowflakeService.GenerateSnowflakeId();
            var dataCenterId = snowflakeService.GetDataCenterIdBySnowflakeId(id);
            Assert.IsType<int>(dataCenterId);
            Assert.Equal(1, dataCenterId);
        }

  
    }
}
