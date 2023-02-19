using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Events;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web;
using WindowsAutomation.Shared.Web.Dtos;

namespace WindowsAutomation.InitAll.Application.Installers;

public class MyAppsInstaller : IPackageInstaller
{
    private readonly IWebDownloader _webDownloader;
    private string _currentPackage = string.Empty;


    public MyAppsInstaller(IWebDownloader webDownloader)
    {
        _webDownloader = webDownloader;
    }

    public async Task InstallPackages(ProgressActionEvents? events)
    {
        _currentPackage = "nandeck";
        events?.Before?.Invoke(this, $"Downloading {_currentPackage}...");

        Progress<double> progress = new();
        progress.ProgressChanged += events?.Progress;

        // await _webDownloader.ExtractLinkAndDownloadFile(
        //     new WebFileDownload("""https://www.nandeck.com/""",
        //         $"""{Constants.Paths.ProgramFilesX86}\Nandeck\Nandeck.zip""", progress),
        //     new RegexGroupNameMatch(
        //         """Version\s\d+\.\d+\.\d+"\shref="(?<downloadurl>.*)"\srel="nofollow""",
        //         "downloadurl"));
        try
        {
            await _webDownloader.ExtractLinkAndDownloadFile(
                new WebFileDownload("""https://www.nandeck.com/""",
                    $"""{Constants.Paths.ProgramFilesX86}\Nandeck\Nandeck.zip""", progress),
                new RegexGroupNameMatch(
                    """Version\s\d+\.\d+\.\d+"\shref="(?<downloadurle>.*)"\srel="nofollow""",
                    "downloadurl"));
        }
        catch (InvalidOperationException ex)
        {
            events?.Error();
        }


        var abc = 3;
    }
}