using Microsoft.AspNetCore.Builder;
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSnowflakeUniqueId(builder.Configuration);

var app = builder.Build();

app.MapGet("/", (ISnowflakeService snowflakeService) =>
{
    long generatingId = snowflakeService.GenerateSnowflakeId();
    DateTime generatedAt = snowflakeService.GetGeneratedDateTimeBySnowflakeId(generatingId);
    int dataCenterId = snowflakeService.GetDataCenterIdBySnowflakeId(generatingId);


    return $"The genrated Id is: { generatingId } - and is genrated at: { generatedAt } - at Data Center Id: {dataCenterId}";
});

app.Run();
