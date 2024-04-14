/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.Extensions.Options;
using SnowflakeId.Core;

namespace SnowflakeId.Tests;

internal class TestOptions : IOptions<SnowflakOptions>
{
    private readonly SnowflakOptions _snowflakOptions;
    public TestOptions(SnowflakOptions snowflakOptions)
    {
        _snowflakOptions = snowflakOptions;
    }

    public SnowflakOptions Value
    {
        get
        {
            return _snowflakOptions;
        }
    }
}
