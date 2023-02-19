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

    if (initAllRunner is null) throw new ApplicationException("IInitAllRunner not injected properly.");

    SetupInitAllConsoleActions(initAllRunner);
    await initAllRunner.RunCoreLogic();
}
catch (Exception e)
{
    Console.WriteLine($"Error occured during initialization:\n{e.Message}");
    throw;
}

static void SetupInitAllConsoleActions(IInitAllRunner initAllRunner)
{
    if (initAllRunner is WindowsInitAllRunner windowsInitAllRunner)
    {
        windowsInitAllRunner.BeforeInstallChoco += (_, _) => Console.WriteLine("\nInstalling Chocolatey...");
        windowsInitAllRunner.OnInstallChocoOutput += (_, output) => Console.WriteLine(output);
    }

    initAllRunner.BeforeInstallPackages += (_, _) => Console.WriteLine("\nInstalling choco packages...");
    initAllRunner.OnPackageInstall += (_, output) => ConsoleChocoInstallOutput(output);

    initAllRunner.OnDownloadProgress += (_, progress) => Console.Write($"\rDownload progress: {progress * 100: 0.00}&");

    initAllRunner.BeforeExitInitRunner += (_, _) => Console.WriteLine("\nInitialization finished");
}


static void ConsoleChocoInstallOutput(string output)
{
    if (output.Contains("Progress: "))
        Console.Write($"\r{output}");
    else
        Console.WriteLine(output);
}