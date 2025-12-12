### SnowflakeId
This is an implementation of snowflakeId algorithm in C# programming language, this algorithm is developed by X formally (Twitter)

---

### Get Started
Hussien.SnowflakeId is a library that can help you to generate a unique Id, specifically for those who are working in a Distributed Systems.
The current version of this library is 3.3.0, and it supports net6.0, net7.0, net8.0, net9.0 and net10.0.

To strat using Hussien.SnowflakeId library you can install it by using Nugget Mackage Manger or by installing it from the command line  via `dotnet cli` by running the following command:
```
dotnet add package Hussien.SnowflakeId --version 3.3.0
```
|   Package      |    Varsion   |    Downloads   |
| ------- | ----- | ----- |
| `Hussien.SnowflakeId` | [![NuGet](https://img.shields.io/nuget/v/Hussien.SnowflakeId.svg)](https://nuget.org/packages/Hussien.SnowflakeId) | [![Nuget](https://img.shields.io/nuget/dt/Hussien.SnowflakeId.svg)](https://nuget.org/packages/Hussien.SnowflakeId) |

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
    options.DataCenterId = 1;      // in production get this value from the appsettings.json file.
    options.UseConsoleLog = true;  // this is available only on version 3.1.0 and above.
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
    options.DataCenterId = 1;             // in production get this value from the appsettings.json file.
    options.UseConsoleLog = false;        // this is available only on version 3.1.0 and above.
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
            options.UseConsoleLog = false; // this is available only on version 3.1.0 and above.

            // These are two new events are added to version 3.2.0,
            // one is firing before generating Id and the other one after generating id.
            options.Events = new SnowflakeIdEvents
            {
                OnCreatedSnowflakeId = context =>
                {
                    Console.WriteLine("OnCreatedSnowflakeId --> The Id is: {0} and is generated At: {1}", context.Id,                                 context.GeneratedDateTime);
                    return Task.CompletedTask;
                },
                OnCreatingSnowflakeId = context =>
                {
                    Console.WriteLine("OnCreatingSnowflakeId --> Generating Id at data center has id: {0}",                                             context.DataCenterId);
                    return Task.CompletedTask;
                }
            };
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
As you can see from the previous code, you can generate a new id, then you can query at which time the id is generated, and lastly at which data center id is saved .
See the result of the above console application in the below pic:

![image](https://github.com/user-attachments/assets/232917a8-f2ca-41dd-8c11-780e53ea65a9)
