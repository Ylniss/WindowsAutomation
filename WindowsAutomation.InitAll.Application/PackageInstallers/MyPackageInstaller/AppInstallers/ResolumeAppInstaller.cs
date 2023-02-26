using CliWrap;
using CliWrap.Buffered;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public class ResolumeAppInstaller : AppInstaller
{
    public override string AppName => "Resolume";
    public override string SetupPath => $"""{Constants.CommonPaths.Software}\resolume-arena-installer.exe""";


    public ResolumeAppInstaller(IWebDownloader webDownloader) : base(webDownloader)
    {
    }

    public override async Task InstallApp()
    {
        _whenInstallStarted.OnNext(new PackageInstallationStep(AppName, InstallationStep.Download));
        await DownloadApp();

        _whenInstallStarted.OnNext(new PackageInstallationStep(AppName, InstallationStep.RunSetup));

        var cmd = Cli.Wrap("cmd")
            .WithArguments(a => a.Add("/c").Add(SetupPath).Add("/SP-").Add("/VERYSILENT"));

        var result = await cmd.ExecuteBufferedAsync();
        _whenSetupOutputReceived.OnNext(result.StandardOutput);
        _whenInstallStarted.OnCompleted();
    }

    private async Task DownloadApp()
    {
        await _webDownloader.ExtractLinkAndDownloadFile(
            new WebFileDownload("""https://resolume.com/download/?file=latest_arena""",
                SetupPath),
            new RegexGroupNameMatch(
                """<iframe\ssrc="//(?<downloadurl>.*)"\sstyle""",
                "downloadurl"), "https://");
    }
}