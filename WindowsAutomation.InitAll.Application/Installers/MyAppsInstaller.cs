using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression;
using WindowsAutomation.Shared.Web;

namespace WindowsAutomation.InitAll.Application.Installers;

public class MyAppsInstaller : IPackageInstaller
{
    private readonly IWebDownloader _webDownloader;
    private readonly IRegexExtractor _regexExtractor;
    private string _currentPackage = string.Empty;

    public event EventHandler<double>? OnDownloadProgress;

    public MyAppsInstaller(IWebDownloader webDownloader, IRegexExtractor regexExtractor)
    {
        _webDownloader = webDownloader;
        _regexExtractor = regexExtractor;
    }

    public async Task InstallPackages(EventHandler<string>? beforeDownload = null)
    {
        _currentPackage = "nandeck";
        beforeDownload?.Invoke(this, $"Downloading {_currentPackage}...");

        Progress<double> progress = new();
        progress.ProgressChanged += OnDownloadProgress;

        await _webDownloader.ExtractLinkAndDownloadFile(
            new WebFileDownload("""https://www.nandeck.com/""",
                $"""{Constants.Paths.ProgramFilesX86}\Nandeck\Nandeck.zip""", progress),
            new RegexGroupNameMatch(
                """Version\s\d+\.\d+\.\d+"\shref="(?<downloadurl>.*)"\srel="nofollow""",
                "downloadurl"));


        var abc = 3;
    }
}