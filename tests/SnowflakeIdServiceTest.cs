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
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 1);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            var id = snowflakeService.GenerateSnowflakeId();
            var idLength = id.ToString().Length == 19;

            Assert.IsType<long>(id);
            Assert.True(idLength);
        }

        [Fact]
        public void Can_Find_Data_CenterId_From_Generated_UniqueId()
        {
            var services = new ServiceCollection();
            services.AddSnowflakeUniqueId(s => s.DataCenterId = 17);

            var serviceProvider = services.BuildServiceProvider();
            var snowflakeService = serviceProvider.GetRequiredService<ISnowflakeService>();

            var id = snowflakeService.GenerateSnowflakeId();
            var dataCenterId = snowflakeService.GetDataCenterIdBySnowflakId(id);
            Assert.IsType<int>(dataCenterId);
            Assert.Equal(17, dataCenterId);

        }

    }
}
