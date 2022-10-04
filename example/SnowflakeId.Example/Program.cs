
/*
 
 Build and implemented with love by Mohammed Ahmed Hussien Babiker
 
 */

using System;
using SnowflakeId.Core;


SnowflakeIdService IdService = new SnowflakeIdService(1);

long result = IdService.GenerateSnowflakeId();
Console.WriteLine("The Id is: {0}", result);
Console.ReadLine();
