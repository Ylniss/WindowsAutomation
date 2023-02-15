using CliWrap;
using WindowsAutomation.Shared.Shell;
using WindowsAutomation.Shared.Web;

namespace WindowsAutomation.InitAll.Application.Installers.Choco;

public class ChocoAppsInstaller : IPackageInstaller
{
    private const string ChocoInstallUri = """https://chocolatey.org/install.ps1""";
    private readonly string[] _packages;

    private readonly IShellRunner _shellRunner;
    private readonly IWebDownloader _webDownloader;

    public ChocoAppsInstaller(IPackageProvider packageProvider, IShellRunner shellRunner, IWebDownloader webDownloader)
    {
        _shellRunner = shellRunner;
        _webDownloader = webDownloader;
        _packages = packageProvider.LoadPackages().ToArray();
    }

    public async Task InstallChoco(Action<string>? onInstallChocoScriptOutput = null)
    {
        var chocoInstallationScript = await _webDownloader.DownloadContent(ChocoInstallUri);

        await foreach (var output in _shellRunner.RunScript(chocoInstallationScript))
            onInstallChocoScriptOutput?.Invoke(output);
    }

    public async Task<bool> CheckPackages(Action<string, bool>? afterCheck = null)
    {
        var anyNotFound = true;

        var findPackageTasks = _packages.Select(ChocoFindPackage).ToList();

        while (findPackageTasks.Any())
        {
            var finishedTask = await Task.WhenAny(findPackageTasks);
            findPackageTasks.Remove(finishedTask);
            var (package, result) = await finishedTask;

            var found = result.ExitCode == 0;
            if (!found) anyNotFound = found;
            afterCheck?.Invoke(package, found);
        }

        return anyNotFound;
    }

    public Task InstallPackages(Action<string>? onInstall = null)
    {
        throw new NotImplementedException();
    }

    private static async Task<(string package, CommandResult result)> ChocoFindPackage(string package)
    {
        var chocoCmd = Cli.Wrap("choco");
        return (package, await chocoCmd.WithArguments(new[] { "find", $"{package}" })
            .ExecuteAsync().Task);
    }
}