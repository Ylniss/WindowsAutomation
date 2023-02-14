using CliWrap;

namespace WindowsAutomation.InitAll.Application.Installers.Choco;

public class ChocoAppsInstaller : IInstaller
{
    private readonly string[] _packages;

    public ChocoAppsInstaller(IPackageProvider packageProvider)
    {
        _packages = packageProvider.LoadPackages().ToArray();
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