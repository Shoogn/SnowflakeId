/*
 
 Build and implemented with love by Mohammed Ahmed Hussien Babiker
 
 */

using System;
namespace SnowflakeId.Core
{
    public class SnowflakeIdOptionBuilder
    {
        public const int TotalBits = 64;
        public const int EpochBits = 42;
        public const int MachineIdBits = 10;
        public const int SequenceBits = 12;

        public static readonly int MaxMachineId = (int)(Math.Pow(2, MachineIdBits) - 1);

        public static readonly int MaxSequenceId = (int)(Math.Pow(2, SequenceBits) - 1);

        private Func<DateTime> GetUtcNow { get; set; } = () => DateTime.UtcNow;
    }
}
