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
        await WhenInstall.ActAsync(new PackageInstallationStep(AppName, InstallationStep.Download), async step =>
        {
            await DownloadApp();

            WhenInstall.StartedSubject.OnNext(new PackageInstallationStep(AppName, InstallationStep.RunSetup));

            var cmd = Cli.Wrap("cmd")
                .WithArguments(a => a.Add("/c").Add(SetupPath).Add("/SP-").Add("/VERYSILENT"));

            var result = await cmd.ExecuteBufferedAsync();
            WhenSetupOutputReceive.StartedSubject.OnNext(result.StandardOutput);
        });
    }

    private async Task DownloadApp()
    {
        await WebDownloader.ExtractLinkAndDownloadFile(
            new WebFileDownload("""https://resolume.com/download/?file=latest_arena""",
                SetupPath),
            new RegexGroupNameMatch(
                """<iframe\ssrc="//(?<downloadurl>.*)"\sstyle""",
                "downloadurl"), "https://");
    }
}