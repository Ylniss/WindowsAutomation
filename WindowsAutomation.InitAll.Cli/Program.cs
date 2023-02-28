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

    SetupConsoleEvents(initAllRunner);
    await RunCoreLogic(initAllRunner);

    Console.WriteLine("\n ---------- Windows initialization script finished ---------- ");
    Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine($"\nError occured during initialization:\n{e.Message}\n{e.InnerException?.Message}");
#if (DEBUG)
    Console.WriteLine($"\n{e.StackTrace}");
#endif

    Console.ReadKey();

    throw;
}

static async Task RunCoreLogic(IInitAllRunner initAllRunner)
{
    var config = initAllRunner.GetConfigFromJson();
    //
    // await initAllRunner.InstallPackages();
    //
    // initAllRunner.CloneReposFromGitHub(config.GithubCredentials, config.ReposToClone, config.Paths.Repo);
    // initAllRunner.SwapPowerShellProfileWithSymbolicLink($"""{config.Paths.Repo}\.dotfiles\{Constants.ProfileName}""");
    //
    // initAllRunner.CreateInitialFolderStructure(config.FolderStructure);
    // initAllRunner.CopyDirectories(config.CopyDirectories);
    //
    // initAllRunner.CursorChanger.SetCursorTheme(config.CursorTheme);

    initAllRunner.Pinner.PinToQuickAccess(config.CopyDirectories[0].To);

    initAllRunner.CleanDesktopAndRecycleBin();
}

static void SetupConsoleEvents(IInitAllRunner initAllRunner)
{
    var chocoAppsInstaller =
        initAllRunner.PackageInstallers.SingleOrDefault(installer =>
            installer is ChocoPackageInstaller) as ChocoPackageInstaller;

    ConsoleEvents.SetupChocoAppsInstaller(chocoAppsInstaller);

    var myAppsInstaller =
        initAllRunner.PackageInstallers.SingleOrDefault(installer => installer is MyPackageInstaller) as
            MyPackageInstaller;

    ConsoleEvents.SetupMyAppsInstaller(myAppsInstaller);

    ConsoleEvents.SetupGit(initAllRunner);
    ConsoleEvents.SetupFilesystem(initAllRunner);
    ConsoleEvents.SetupOs(initAllRunner);
}