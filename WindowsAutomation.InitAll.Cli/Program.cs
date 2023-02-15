using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Cli;

Console.WriteLine(" ---------- Running Windows initialization script ---------- ");

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddApplicationServices().AddInstallerServices().AddSharedServices(); })
    .Build();

var serviceProvider = host.Services;

using var serviceScope = serviceProvider.CreateScope();
var provider = serviceScope.ServiceProvider;

try
{
    var initAllRunner = provider.GetService<IInitAllRunner>();

    if (initAllRunner is WindowsInitAllRunner windowsInitAllRunner)
    {
        windowsInitAllRunner.BeforeInstallChoco = ConsoleBeforeChocoInstall;
        windowsInitAllRunner.OnInstallChocoOutput = ConsoleChocoInstallOutput;
    }

    initAllRunner.BeforePackageStatusSet = ConsoleCheckingPackages;
    initAllRunner.OnPackageStatusSet = ConsoleWritePackageStatus;
    initAllRunner.OnPackageNotFound = ConsolePackagesNotFound;
    initAllRunner.AskQuestionYesNoToContinueOnNotFoundPackages = ConsoleAskQuestionYesNo;

    await initAllRunner.RunCoreLogic();
}
catch (Exception e)
{
    Console.WriteLine($"Error occured during initialization:/n{e.Message}");
    throw;
}

static void ConsoleBeforeChocoInstall() =>
    Console.WriteLine("Installing Chocolatey...");

static void ConsoleChocoInstallOutput(string scriptOutput) =>
    Console.WriteLine(scriptOutput);

static void ConsoleCheckingPackages() =>
    Console.WriteLine("Checking choco_packages.json...");

static void ConsolePackagesNotFound() =>
    Console.WriteLine("Failed to find some packages.");

static bool ConsoleAskQuestionYesNo(string question)
{
    Console.WriteLine($"{question} [y/n]:");

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

static void ConsoleWritePackageStatus(string package, bool found) =>
    Console.WriteLine($"{package}: " + (found ? "OK" : "NOT FOUND"));