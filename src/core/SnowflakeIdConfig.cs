// Read more about the licenses under the root of the project in the LICENSE.txt file.

using System;

namespace SnowflakeId.Core
{
    internal class SnowflakeIdConfig
    {
        /// <summary>
        /// Total size of the Id in bits, 64
        /// </summary>
        public const int TotalBits = 64;

        /// <summary>
        /// timestamp in bits - 1 bit, 42
        /// </summary>
        public const int EpochBits = 42;

        /// <summary>
        /// machain id in bits, 10
        /// </summary>
        public const int MachineIdBits = 10;

        /// <summary>
        /// sequence in bits, 12
        /// </summary>
        public const int SequenceBits = 12;


        /// <summary>
        /// 1023
        /// </summary>
        public static readonly int MaxMachineId = (int)(Math.Pow(2, MachineIdBits) - 1);

        /// <summary>
        /// 4095
        /// </summary>
        public static readonly int MaxSequenceId = (int)(Math.Pow(2, SequenceBits) - 1);


        /// <summary>
        public static readonly DateTime DefaultEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly int MaxDataCenterId = ((1 << MachineIdBits) - 1);
    }
}
