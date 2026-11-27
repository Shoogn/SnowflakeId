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
        /// timestamp in bits, 41. The remaining (most significant) bit of the 64-bit id is
        /// always left unset so generated ids never become negative.
        /// </summary>
        public const int EpochBits = 41;

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
        /// ((1 << MachineIdBits) - 1);
        /// </summary>
        public static readonly int MaxMachineId = (int)(Math.Pow(2, MachineIdBits) - 1);

        /// <summary>
        /// 4095
        /// </summary>
        public static readonly int MaxSequenceId = (int)(Math.Pow(2, SequenceBits) - 1);

        /// <summary>
        /// The largest timestamp (relative to the configured epoch, in milliseconds) that fits
        /// in <see cref="EpochBits"/> bits without overflowing into the id's sign bit.
        /// </summary>
        public static readonly long MaxTimestamp = (1L << EpochBits) - 1;
    }
}
