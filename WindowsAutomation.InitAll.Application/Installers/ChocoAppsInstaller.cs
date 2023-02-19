﻿using CliWrap;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Shell;

namespace WindowsAutomation.InitAll.Application.Installers;

public class ChocoAppsInstaller : IPackageInstaller
{
    private const string ChocoInstallUri = """https://chocolatey.org/install.ps1""";

    private readonly IShellRunner _shellRunner;

    public ChocoAppsInstaller(IShellRunner shellRunner)
    {
        _shellRunner = shellRunner;
    }

    public async Task InstallChoco(EventHandler<string>? onInstallChocoScriptOutput = null)
    {
        await _shellRunner.RunScriptFromWeb(ChocoInstallUri, onInstallChocoScriptOutput);
    }

    public async Task InstallPackages(EventHandler<string>? beforeDownload = null)
    {
        await Task.Run(() =>
            _shellRunner.RunScript($"choco install {Constants.ChocoPackagesConfig} -y --acceptlicense --force",
                beforeDownload));
    }

    private static async Task<(string package, CommandResult result)> ChocoFindPackage(string package)
    {
        var chocoCmd = Cli.Wrap("choco");
        return (package, await chocoCmd.WithArguments(new[] { "find", $"{package}" })
            .ExecuteAsync().Task);
    }
}