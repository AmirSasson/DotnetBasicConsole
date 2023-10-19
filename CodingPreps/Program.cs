// See https://aka.ms/new-console-template for more information
using CodingPreps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;

var delayArgument = new Argument<int>
    (name: "delay",
    description: "delay between upserts",
    getDefaultValue: () => 500);

var rootCommand = new RootCommand
{
    delayArgument
};

rootCommand.SetHandler((delayArgumentValue) =>
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddTransient<IPersonsService, PersonsService>();
    builder.Services.AddSingleton<IPersonsRepository, InMemPersonsRepository>();
    builder.Logging.AddConsole();


    using IHost host = builder.Build();
    CancellationTokenSource tokenSrc = new CancellationTokenSource();

    var loggerFactory = host.Services.GetService<ILoggerFactory>();
    var logger = loggerFactory!.CreateLogger<PersonsService>();

    Console.CancelKeyPress += (sender, eArgs) =>
    {
        tokenSrc.Cancel();
        logger.LogCritical("Shutting Down requested..");
        eArgs.Cancel = true;
    };
    
    logger.LogCritical("Staring engine.");

    Task.Run(async () =>
    {
        while (!tokenSrc.IsCancellationRequested)
        {
            var person = new Person(DateTime.UtcNow.Ticks, $"amir-{DateTime.UtcNow.Ticks}", "sasson");

            var service = host.Services.GetService<IPersonsService>();

            await service!.Upsert(person);

            await Task.Delay(delayArgumentValue);
        }
    });


    Task.Run(async () =>
    {
        while (!tokenSrc.IsCancellationRequested)
        {
            var service = host.Services.GetService<IPersonsService>();

            var persons = await service!.GetAll();
            logger.LogInformation($"has {persons.Count()}");

            await Task.Delay(1000);
        }
    });

    tokenSrc.Token.WaitHandle.WaitOne();

},delayArgument);

await rootCommand.InvokeAsync(args);
