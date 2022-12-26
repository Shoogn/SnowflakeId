/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System;
namespace SnowflakeId.Core
{
    public class SnowflakeIdOptionBuilder
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
        /// 99
        /// </summary>
        public static readonly int MaxMachineId = (int)(Math.Pow(2, MachineIdBits) - 1);

        /// <summary>
        /// 143
        /// </summary>
        public static readonly int MaxSequenceId = (int)(Math.Pow(2, SequenceBits) - 1);
    }
}
