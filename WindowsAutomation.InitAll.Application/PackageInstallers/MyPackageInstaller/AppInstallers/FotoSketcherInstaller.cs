using CliWrap;
using CliWrap.Buffered;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public class FotoSketcherInstaller : AppInstaller
{
    public override string AppName => "FotoSketcher";
    public override string SetupPath => $"""{Constants.CommonPaths.Software}\FotoSketcher.exe""";

    public FotoSketcherInstaller(IWebDownloader webDownloader) : base(webDownloader)
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
            new WebFileDownload("""https://fotosketcher.com/download-fotosketcher/""",
                SetupPath),
            new RegexGroupNameMatch(
                """<a href="(?<downloadurl>.*)"><img loading="lazy" src="https://fotosketcher.com""",
                "downloadurl"), "https://");
    }
}