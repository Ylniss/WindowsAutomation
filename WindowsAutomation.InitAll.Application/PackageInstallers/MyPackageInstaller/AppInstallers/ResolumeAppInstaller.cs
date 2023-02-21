using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public class ResolumeAppInstaller : AppInstaller
{
    public override string AppName => "Resolume";
    public override string SetupPath => $"""{Constants.Paths.Software}\resolume-arena-installer.exe""";


    public ResolumeAppInstaller(IWebDownloader webDownloader) : base(webDownloader)
    {
    }

    public override async Task InstallApp()
    {
        _whenInstallStarted.OnNext(new PackageInstallationStep(AppName, InstallationStep.Download));
        await DownloadApp();

        _whenInstallStarted.OnNext(new PackageInstallationStep(AppName, InstallationStep.RunSetup));

        throw new NotImplementedException();
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