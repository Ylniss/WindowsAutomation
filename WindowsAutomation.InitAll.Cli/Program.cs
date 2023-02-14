using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Cli;

Console.WriteLine(" ---------- Running Windows initialization script ---------- ");

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddApplicationServices();
        services.AddInstallerServices();
        services.AddSharedServices();
    })
    .Build();

var serviceProvider = host.Services;

using var serviceScope = serviceProvider.CreateScope();
var provider = serviceScope.ServiceProvider;

try
{
    var initAllRunner = provider.GetService<IInitAllRunner>();
    initAllRunner.BeforePackageStatusSet = ConsoleCheckingPackages;
    initAllRunner.OnPackageStatusSet = ConsoleWritePackageStatus;
    initAllRunner.OnPackageNotFound = ConsolePackagesNotFound;
    initAllRunner.AskQuestionYesNoToContinueOnNotFoundPackages = ConsoleAskQuestionYesNo;

    await initAllRunner.RunCoreLogic(provider);
}
catch (Exception e)
{
    Console.WriteLine($"Error occured during initialization:/n{e.Message}");
    throw;
}

static void ConsoleCheckingPackages()
{
    Console.WriteLine("Checking choco_packages.json...");
}

static void ConsolePackagesNotFound()
{
    Console.WriteLine("Failed to find some packages.");
}

static bool ConsoleAskQuestionYesNo(string question)
{
    Console.WriteLine(question);

    while (true)
    {
        var key = Console.ReadKey();

        return key.Key switch
        {
            ConsoleKey.Y => true,
            _ => false
        };
    }
}

static void ConsoleWritePackageStatus(string package, bool found)
{
    Console.WriteLine($"{package}: " + (found ? "OK" : "NOT FOUND"));
}