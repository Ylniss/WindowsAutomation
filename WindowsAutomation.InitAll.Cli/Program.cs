using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Application.Installers;
using WindowsAutomation.InitAll.Cli;
using WindowsAutomation.Shared;

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

    SetupConsoleEvents(initAllRunner);
    await initAllRunner.RunCoreLogic();
}
catch (Exception e)
{
    Console.WriteLine($"Error occured during initialization:\n{e.Message}");
    throw;
}

static void SetupConsoleEvents(IInitAllRunner initAllRunner)
{
    var chocoAppsInstaller =
        initAllRunner.PackageInstallers.Single(installer => installer is ChocoAppsInstaller) as ChocoAppsInstaller;

    chocoAppsInstaller?.WhenInstallStarted.Where(config => config.Contains(Constants.ChocoPackagesConfig))
        .Subscribe(config =>
            Console.WriteLine($"Installation from {config} started..."));

    chocoAppsInstaller?.WhenInstallStarted.Where(uri => !uri.Contains(Constants.ChocoPackagesConfig))
        .Subscribe(config =>
            Console.WriteLine("Downloading chocolatey"));

    chocoAppsInstaller?.WhenChocoScriptOutputReceived?.Subscribe(ConsolePackageInstallOutput);

    chocoAppsInstaller?.WhenDownloadStarted?.Subscribe(uri =>
        Console.WriteLine($"Download from {uri} started..."));

    chocoAppsInstaller?.WhenDownloadProgressReceived?.Subscribe(progress =>
    {
        if (progress is not null)
            Console.Write($"\rDownload progress: {progress * 100: 0.00}%");
    });


    var myAppsInstaller =
        initAllRunner.PackageInstallers.Single(installer => installer is MyAppsInstaller) as MyAppsInstaller;

    myAppsInstaller?.WhenInstallStarted.Subscribe(package =>
        Console.WriteLine($"\n{package} installation started..."));

    //initAllRunner.BeforeExitInitRunner += (_, _) => Console.WriteLine("\nInitialization finished");
}


static void ConsolePackageInstallOutput(string output)
{
    if (output.Contains("Progress: "))
        Console.Write($"\r{output}");
    else
        Console.WriteLine(output);
}