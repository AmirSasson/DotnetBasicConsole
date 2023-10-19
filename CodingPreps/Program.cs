// See https://aka.ms/new-console-template for more information
using CodingPreps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IPersonsService, PersonsService>();
builder.Services.AddSingleton<IPersonsRepository, InMemPersonsRepository>();
builder.Logging.AddConsole();


using IHost host = builder.Build();
CancellationTokenSource tokenSrc = new CancellationTokenSource();



var loggerFac = host.Services.GetService<ILoggerFactory>();
var logger = loggerFac.CreateLogger<PersonsService>();

Console.CancelKeyPress += (sender, eArgs) => {
    tokenSrc.Cancel();
    logger.LogCritical("Shutting Down requested..");
    eArgs.Cancel = true;
};
// kick off asynchronous stuff 


logger.LogCritical("Staring.");


Task.Run(async () =>
{
    while (!tokenSrc.IsCancellationRequested)
    {
        var person = new Person(DateTime.UtcNow.Ticks, $"amir-{DateTime.UtcNow.Ticks}", "sasson");

        var service = host.Services.GetService<IPersonsService>();

        await service!.Upsert(person);

        await Task.Delay(200);
    }
});


Task.Run(async () =>
{
    while (!tokenSrc.IsCancellationRequested)
    {
        var person = new Person(DateTime.UtcNow.Ticks, $"amir-{DateTime.UtcNow.Ticks}", "sasson");

        var service = host.Services.GetService<IPersonsService>();

        var persons = await service!.GetAll();
        logger.LogInformation($"has {persons.Count()}");

        await Task.Delay(1000);
    }
});

tokenSrc.Token.WaitHandle.WaitOne();