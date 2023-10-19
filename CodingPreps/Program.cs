// See https://aka.ms/new-console-template for more information
using CodingPreps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IPersonsService, PersonsService>();
builder.Services.AddScoped<IPersonsRepository, InMemPersonsRepository>();

using IHost host = builder.Build();


var person = new Person(1, "amir", "sasson");


var service = host.Services.GetService<IPersonsService>();

await service!.Upsert(person);


var fetchedPerson =  await service.GetById(1);

Console.WriteLine(fetchedPerson.FirstName);

