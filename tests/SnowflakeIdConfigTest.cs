using SnowflakeId.Core;

namespace SnowflakeId.Tests
{
    public class SnowflakeIdConfigTest
    {
        [Fact]
        public void SnowflakeIdConfig_TotalBits_Lenght_Is_64()
        {
            Assert.Equal(64, SnowflakeIdConfig.TotalBits);
        }

        [Fact]
        public void SnowflakeIdConfig_EpochBits_Length_Is_42()
        {
            Assert.Equal(42, SnowflakeIdConfig.EpochBits);
        }

        [Fact]
        public void SnowflakeIdConfig_MachineIdBits_Length_Is_10()
        {
            Assert.Equal(10, SnowflakeIdConfig.MachineIdBits);
        }

        [Fact]
        public void SnowflakeIdConfig_SequenceBits_Length_Is_12()
        {
            Assert.Equal(12, SnowflakeIdConfig.SequenceBits);
        }

        [Fact]
        public void SnowflakeIdConfig_MaxMachineId_Is_Equal_1023()
        {
            Assert.Equal(1023, SnowflakeIdConfig.MaxMachineId);
        }

        [Fact]
        public void SnowflakeIdConfig_MaxSequenceId_Is_Equal_4095()
        {
            Assert.Equal(4095, SnowflakeIdConfig.MaxSequenceId);
        }
    }
}
