
/*
 
 Build and implemented with love by Mohammed Ahmed Hussien Babiker
 
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnowflakeId.Core;
using SnowflakeId.Core.Events;
using SnowflakeId.DependencyInjection;
using System;
using System.Threading.Tasks;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSnowflakeUniqueId( options =>
        {
            options.DataCenterId = 7;
            options.Events = new SnowflakeIdEvents
            {
                OnCreatedSnowflakeId = context =>
                {
                    Console.WriteLine("OnCreatedSnowflakeId --> The Id is: {0} and is generated At: {1}", context.Id, context.GeneratedDateTime);
                    return Task.CompletedTask;
                },
                OnCreatingSnowflakeId = context =>
                {
                    Console.WriteLine("OnCreatingSnowflakeId --> Generating Id at data center has id: {0}", context.DataCenterId);
                    return Task.CompletedTask;
                }
            };
        }
        );
    }).Build();

var idServive = host.Services.GetRequiredService<ISnowflakeService>();

var uniqueId = idServive.GenerateSnowflakeId();
Console.WriteLine("The unique Id is: {0}", uniqueId);
Console.WriteLine("*******************************");

var generatedAt = idServive.GetGeneratedDateTimeBySnowflakeId(uniqueId);

Console.WriteLine("The Id is: {0} and is generated At: {1}", uniqueId, generatedAt);

var dataCenterId = idServive.GetDataCenterIdBySnowflakeId(uniqueId);
Console.WriteLine("The id is generated at data center has id: {0}", dataCenterId);

Console.ReadLine();
