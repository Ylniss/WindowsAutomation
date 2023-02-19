using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.InitAll.Application.Installers;

public class MyAppsInstaller : IPackageInstaller
{
    private readonly IWebDownloader _webDownloader;
    private string _currentPackage = string.Empty;

    private readonly Subject<string> _whenInstallStarted = new();
    public IObservable<string> WhenInstallStarted => _whenInstallStarted.AsObservable();
    public IObservable<string> WhenDownloadStarted { get; }
    public IObservable<double?> WhenDownloadProgressReceived { get; }


    public MyAppsInstaller(IWebDownloader webDownloader)
    {
        _webDownloader = webDownloader;
        WhenDownloadStarted = _webDownloader.WhenDownloadStarted;
        WhenDownloadProgressReceived = _webDownloader.WhenDownloadProgressReceived;
    }

    public async Task InstallPackages()
    {
        _currentPackage = "Nandeck";
        _whenInstallStarted.OnNext(_currentPackage);

        await _webDownloader.ExtractLinkAndDownloadFile(
            new WebFileDownload("""https://www.nandeck.com/""",
                $"""{Constants.Paths.ProgramFilesX86}\Nandeck\Nandeck.zip"""),
            new RegexGroupNameMatch(
                """Version\s\d+\.\d+\.\d+"\shref="(?<downloadurl>.*)"\srel="nofollow""",
                "downloadurl"));


        _currentPackage = "Resolume";
        _whenInstallStarted.OnNext(_currentPackage);

        await _webDownloader.ExtractLinkAndDownloadFile(
            new WebFileDownload("""https://resolume.com/download/?file=latest_arena""",
                $"""{Constants.Paths.Software}\resolume-arena-installer.exe"""),
            new RegexGroupNameMatch(
                """<iframe\ssrc="//(?<downloadurl>.*)"\sstyle""",
                "downloadurl"), "https://");
    }
}