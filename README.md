### SnowflakeId
This is the implementation of twitter's snowflakeId algorithm in C# programming language

---

### Get Started
SnowflakeId is a library that can help you to generate a unique Id, specifically for those who are working in a Distributed Systems.
The currently version is version 3.0.1, and there are break changes in this version when you upgrade from an older versions so be careful when you upgrade to version 3.0.1

For any .NET application target .NET version higher than .Net 6 install the library by using NuGet package command
```
dotnet add package Hussien.SnowflakeId --version 3.0.1
```
---

### With Asp.Net Core Application

First you have to imports these namespaces
```
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;
```

Second register the Hussine.SnowflakeId library service by adding next two lines of code:
```C#
builder.Services.AddSnowflakeUniqueId(options =>
{
    options.DataCenterId = 1; // Its best if you read the value from the appsettings.json file
});
```
---

### The Complete Asp.NET Core Example:
```C#
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
    DateTime generatedAt = snowflakeService.GetGeneratedDateTimeBySnowflakeId(generatingId);
    int dataCenterId = snowflakeService.GetDataCenterIdBySnowflakId(generatingId);

    return $"The genrated Id is: { generatingId } - and is genrated at: { generatedAt } - at Data Center Id: {dataCenterId}";
});

app.Run();
```

And here is the result if you run the app:
![image](https://github.com/Shoogn/SnowflakeId/assets/18530495/6d05dcd7-4a87-4fb9-86a7-bfd79aca7c80)

---
### With Console Application Or any other .NET application:
```C#
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnowflakeId.Core;
using SnowflakeId.DependencyInjection;
using System;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSnowflakeUniqueId(options =>
        {
            options.DataCenterId = 7;
        });
    }).Build();

var idServive = host.Services.GetRequiredService<ISnowflakeService>();

var uniqueId = idServive.GenerateSnowflakeId();
Console.WriteLine("The unique Id is: {0}", uniqueId);
Console.WriteLine("*******************************");

var generatedAt = idServive.GetGeneratedDateTimeBySnowflakeId(uniqueId);
Console.WriteLine("The Id is: {0} and is generated At: {1}", uniqueId, generatedAt);

var dataCenterId = idServive.GetDataCenterIdBySnowflakId(uniqueId);
Console.WriteLine("The id is generated at data center has id: {0}", dataCenterId);

Console.ReadLine();
```
As you can see from the previous code, you can generate a new id, then you can query at which time the id is generated, and lastly at which data center id is saved.
