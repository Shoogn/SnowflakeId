using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnowflakeId.Core;

namespace SnowflakeId.Tests
{
    public class SnowflakeIdOptionBuilderTest
    {
        [Fact]
        public void SnowflakeIdOptionBuilder_TotalBits_Lenght_Is_64()
        {
            Assert.Equal(64, SnowflakeIdOptionBuilder.TotalBits);
        }

        [Fact]
        public void SnowflakeIdOptionBuilder_EpochBits_Length_Is_42()
        {
            Assert.Equal(42, SnowflakeIdOptionBuilder.EpochBits);
        }

        [Fact]
        public void SnowflakeIdOptionBuilder_MachineIdBits_Length_Is_10()
        {
            Assert.Equal(10, SnowflakeIdOptionBuilder.MachineIdBits);
        }

        [Fact]
        public void SnowflakeIdOptionBuilder_SequenceBits_Length_Is_12()
        {
            Assert.Equal(12, SnowflakeIdOptionBuilder.SequenceBits);
        }

        [Fact]
        public void SnowflakeIdOptionBuilder_MaxMachineId_Is_Equal_1023()
        {
            Assert.Equal(1023, SnowflakeIdOptionBuilder.MaxMachineId);
        }

        [Fact]
        public void SnowflakeIdOptionBuilder_MaxSequenceId_Is_Equal_4095()
        {
            Assert.Equal(4095, SnowflakeIdOptionBuilder.MaxSequenceId);
        }
    }
}
