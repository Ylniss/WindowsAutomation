using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Compression;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public class NandeckAppInstaller : AppInstaller
{
    private readonly IZipper _zipper;
    public override string AppName => "Nandeck";
    public override string SetupPath => $"""{Constants.Paths.ProgramFilesX86}\Nandeck\Nandeck.zip""";

    public NandeckAppInstaller(IWebDownloader webDownloader, IZipper zipper) : base(webDownloader)
    {
        _zipper = zipper;
    }

    public override async Task InstallApp()
    {
        _whenInstallStarted.OnNext(new PackageInstallationStep(AppName, InstallationStep.Download));
        await DownloadApp();

        var destination = Directory.GetParent(SetupPath)!.FullName;
        _whenInstallStarted.OnNext(new PackageInstallationStep(AppName, InstallationStep.Extract, destination));
        _zipper.Extract(SetupPath, Directory.GetParent(SetupPath)!.FullName);
        _whenInstallStarted.OnCompleted();
    }

    private async Task DownloadApp()
    {
        await _webDownloader.ExtractLinkAndDownloadFile(
            new WebFileDownload("""https://www.nandeck.com/""",
                SetupPath),
            new RegexGroupNameMatch(
                """Version\s\d+\.\d+\.\d+"\shref="(?<downloadurl>.*)"\srel="nofollow""",
                "downloadurl"));
    }
}