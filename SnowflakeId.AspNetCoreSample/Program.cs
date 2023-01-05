using Microsoft.AspNetCore.Builder;
using SnowflakeId.Core;
using SnowflakeId.Core.DependencyInjection;
using SnowflakeId.Provider;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSnowflakeUniqueId(options =>
{
    options.DataCenterId = 1;
});

var app = builder.Build();

app.MapGet("/", (ISnowflakeService snowflakeService, ISnowflakeIdProvider snowflakeIdProvider) =>
{
    long generatingId = snowflakeService.GenerateSnowflakeId();
    SnowflakeId.Provider.SnowflakeId sn = snowflakeIdProvider.GetDateTimeBySnowflakeId(generatingId);


    return $"The genrated Id is: { generatingId } - and is genrated at { sn.GeneratedDateTime }";
});

app.Run();
