using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;
using WindowsAutomation.Shared;

Console.WriteLine(" ---------- Running Windows initialization script ---------- ");

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddInitAllServices().AddSharedServices(); })
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
    Console.WriteLine($"\nError occured during initialization:\n{e.Message}");
    throw;
}

static void SetupConsoleEvents(IInitAllRunner initAllRunner)
{
    var chocoAppsInstaller =
        initAllRunner.PackageInstallers.Single(installer =>
            installer is ChocoPackageInstaller) as ChocoPackageInstaller;

    chocoAppsInstaller?.WhenInstallStarted
        .Where(installationStep => installationStep.Package.Contains(Constants.ChocoPackagesConfig))
        .Subscribe(installationStep =>
            Console.WriteLine($"Installation from {installationStep.Package} started..."));

    chocoAppsInstaller?.WhenInstallStarted
        .Where(installationStep => !installationStep.Package.Contains(Constants.ChocoPackagesConfig))
        .Subscribe(config =>
            Console.WriteLine("Downloading chocolatey"));

    chocoAppsInstaller?.WhenChocoScriptOutputReceived
        .Where(output => output.Contains("Progress: "))
        .Subscribe(output => Console.Write($"\r{output}"));

    chocoAppsInstaller?.WhenChocoScriptOutputReceived
        .Where(output => !output.Contains("Progress: "))
        .Subscribe(Console.WriteLine);

    chocoAppsInstaller?.WhenDownloadStarted?.Subscribe(uri =>
        Console.WriteLine($"Download from {uri} started..."));

    chocoAppsInstaller?.WhenDownloadProgressReceived?
        .Where(progress => progress is not null)
        .Subscribe(progress =>
            Console.Write($"\rDownload progress: {progress * 100: 0.00}%"));

    var myAppsInstaller =
        initAllRunner.PackageInstallers.Single(installer => installer is MyPackageInstaller) as MyPackageInstaller;

    myAppsInstaller?.WhenInstallStarted
        .Where(installationStep => installationStep.Step == InstallationStep.Download)
        .Subscribe(step =>
            Console.WriteLine($"\n{step.Package} download started..."));

    myAppsInstaller?.WhenInstallStarted
        .Where(installationStep => installationStep.Step == InstallationStep.Extract)
        .Where(installationStep => installationStep.Destination is not null)
        .Subscribe(step =>
            Console.WriteLine($"\nExtracting {step.Package} to {step.Destination}"));

    myAppsInstaller?.WhenInstallStarted
        .Where(installationStep => installationStep.Step == InstallationStep.RunSetup)
        .Subscribe(package =>
            Console.WriteLine($"\n{package} installation started..."));

    myAppsInstaller?.WhenInstallStarted
        .Subscribe(step => { }, () => Console.WriteLine("Done."));

//initAllRunner.BeforeExitInitRunner += (_, _) => Console.WriteLine("\nInitialization finished");
}