### SnowflakeId
This is a implementation for twitter's snowflakeId algorithm in C# language

### Get Started
SnowflakeId is a library that can help you to generate a unique Id, specifically for those who are working in a Distributed Systems.
The currently version is version 2.0.0, and there are break changes in this version with version 1.0.0 so be careful when you upgrade from version 1.0.0 to version 2.0.0

For any .NET application higher than .Net 6 install the library by using NuGet package command
```
dotnet add package Hussien.SnowflakeId --version 2.0.0
```
---

### With Asp.Net Core Application

First you have to imports these namespaces
```
using SnowflakeId.Core;
using SnowflakeId.Core.DependencyInjection;
using SnowflakeId.Provider;
```

Second register the Snowflake service by adding these lines:
```C#
builder.Services.AddSnowflakeUniqueId(options =>
{
    options.DataCenterId = 1;
});
```

### The Complete Example:
```C#
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
```
And here is the result if you run the app:
![aspsnow](https://user-images.githubusercontent.com/18530495/210797780-f69c14c8-7158-4daa-bba0-36b313852026.JPG)


---
### With Console Application
```C#
using SnowflakeId.Core;
using SnowflakeId.Core.DependencyInjection;
using SnowflakeId.Provider;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSnowflakeUniqueId(options =>
        {
            options.DataCenterId = 1;
        });
    }).Build();

var idServive = host.Services.GetRequiredService<ISnowflakeService>();
var idProvider = host.Services.GetRequiredService<ISnowflakeIdProvider>();

var uniqueId = idServive.GenerateSnowflakeId();

var generatedAt = idProvider.GetDateTimeBySnowflakeId(uniqueId);

Console.WriteLine("The Id is: {0} and is generated At: {1}", generatedAt.Id, generatedAt.GeneratedDateTime);
Console.ReadLine();
```

