using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;
using WindowsAutomation.InitAll.Cli;
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

    var consoleEvents = SetupConsoleEvents(initAllRunner);
    await RunCoreLogic(initAllRunner);

    consoleEvents.WriteErrors();
    Console.WriteLine("\n ---------- Windows initialization script finished ---------- ");
    Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine($"\nFatal error occured during initialization:\n{e.Message}\n{e.InnerException?.Message}");
#if (DEBUG)
    Console.WriteLine($"\n{e.StackTrace}");
#endif

    Console.ReadKey();

    throw;
}

static async Task RunCoreLogic(IInitAllRunner initAllRunner)
{
    var config = initAllRunner.GetConfigFromJson();

    await initAllRunner.InstallPackages();

    initAllRunner.SetupStartupApplications(config.StartupApps);

    initAllRunner.CloneReposFromGitHub(config.GithubCredentials, config.ReposToClone, config.Paths.Repo);
    initAllRunner.SwapPowerShellProfileWithSymbolicLink($"""{config.Paths.Repo}\.dotfiles\{Constants.ProfileName}""");

    initAllRunner.CreateInitialFolderStructure(config.FolderStructure);
    initAllRunner.CopyDirectories(config.CopyDirectories);
    initAllRunner.CreateShortcuts(config.ShortcutDirectories);
    initAllRunner.PinDirectoriesToQuickAccess(config.PinToQuickAccess);

    initAllRunner.CursorChanger.SetCursorTheme(config.CursorTheme);
    initAllRunner.CleanDesktopAndRecycleBin();
}

static ConsoleEvents SetupConsoleEvents(IInitAllRunner initAllRunner)
{
    ConsoleEvents consoleEvents = new();

    consoleEvents.SetupGeneralInstaller(initAllRunner.PackageInstallers.FirstOrDefault());

    var chocoAppsInstaller =
        initAllRunner.PackageInstallers.SingleOrDefault(installer =>
            installer is ChocoPackageInstaller) as ChocoPackageInstaller;

    consoleEvents.SetupChocoAppsInstaller(chocoAppsInstaller);

    var myAppsInstaller =
        initAllRunner.PackageInstallers.SingleOrDefault(installer => installer is MyPackageInstaller) as
            MyPackageInstaller;

    consoleEvents.SetupMyAppsInstaller(myAppsInstaller);

    consoleEvents.SetupGit(initAllRunner);
    consoleEvents.SetupFilesystem(initAllRunner);
    consoleEvents.SetupOs(initAllRunner);

    return consoleEvents;
}