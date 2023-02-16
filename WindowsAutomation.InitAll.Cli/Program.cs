using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.PowerShell.Commands;
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

    if (initAllRunner is null) throw new ApplicationException("IInitAllRunner not injected properly.");

    if (initAllRunner is WindowsInitAllRunner windowsInitAllRunner)
    {
        windowsInitAllRunner.BeforeInstallChoco = () => Console.WriteLine("Installing Chocolatey...");
        windowsInitAllRunner.OnInstallChocoOutput = scriptOutput => Console.WriteLine(scriptOutput);
    }

    initAllRunner.BeforePackageStatusSet = () => Console.WriteLine("Checking choco_packages.json...");
    initAllRunner.OnPackageStatusSet =
        (package, found) => Console.WriteLine($"{package}: " + (found ? "OK" : "NOT FOUND"));

    initAllRunner.OnPackageNotFound = () => Console.WriteLine("Failed to find some packages.");
    initAllRunner.AskQuestionYesNoToContinueOnNotFoundPackages = ConsoleAskQuestionYesNo;

    initAllRunner.BeforeInstallPackages = () => Console.WriteLine("Installing choco packages...");
    initAllRunner.OnPackageInstallProgress = ConsoleProgressBar;
    initAllRunner.OnPackageInstall = ConsoleChocoInstallOutput;

    initAllRunner.BeforeExitInitRunner = () => Console.WriteLine("\nInitialization finished");

    await initAllRunner.RunCoreLogic();
}
catch (Exception e)
{
    Console.WriteLine($"Error occured during initialization:\n{e.Message}");
    throw;
}

static bool ConsoleAskQuestionYesNo(string question)
{
    Console.Write($"{question} [y/n]: ");

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

void ConsoleProgressBar(double progress)
{
    var myProgress = new WriteProgressCommand
    {
        Activity = "Running . . .",
        Status = $"Running {progress}"
    };

    myProgress.InvokeCommand();
}

static void ConsoleChocoInstallOutput(string output)
{
    if (output.Contains("Progress: "))
        Console.Write($"\r{output}\t");
    else
        Console.WriteLine(output);
}