using Microsoft.AspNetCore.Builder;
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSnowflakeUniqueId(options =>
{
    options.DataCenterId = 1;
});

var app = builder.Build();

app.MapGet("/", (ISnowflakeService snowflakeService) =>
{
    long generatingId = snowflakeService.GenerateSnowflakeId();
    var generatedAt = snowflakeService.GetGeneratedDateTimeBySnowflakeId(generatingId);


    return $"The genrated Id is: { generatingId } - and is genrated at { generatedAt }";
});

app.Run();
