
/*
 
 Build and implemented with love by Mohammed Ahmed Hussien Babiker
 
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnowflakeId.Core;
using SnowflakeId.Core.DependencyInjection;
using SnowflakeId.Provider;
using System;


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
Console.WriteLine("The unique Id is: {0}", uniqueId);
Console.WriteLine("*******************************");

var generatedAt = idProvider.GetDateTimeBySnowflakeId(uniqueId);

Console.WriteLine("The Id is: {0} and is generated At: {1}", generatedAt.Id, generatedAt.GeneratedDateTime);


Console.ReadLine();
