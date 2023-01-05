/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

namespace SnowflakeId.Core.Options
{
    public class SnowflakOptions
    {
        /// <summary>
        /// This could be data center id or server id, 
        /// or any other unique things in your webfarm
        /// Be sure to set this property with number greater than zero
        /// </summary>
        public int DataCenterId { get; set; }
    }
}
