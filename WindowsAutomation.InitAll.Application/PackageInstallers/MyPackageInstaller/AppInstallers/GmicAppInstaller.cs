using CliWrap;
using CliWrap.Buffered;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public class GmicAppInstaller : AppInstaller
{
    private readonly IRegexExtractor _regexExtractor;
    public override string AppName => "Gmic";
    public override string SetupPath => $"""{Constants.Paths.Software}\GMIC.exe""";

    public GmicAppInstaller(IWebDownloader webDownloader, IRegexExtractor regexExtractor) : base(webDownloader)
    {
        _regexExtractor = regexExtractor;
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
    }

    private async Task DownloadApp()
    {
        var html = await _webDownloader.DownloadContent("""https://www.fosshub.com/GMIC.html""");

        var projectId = _regexExtractor.Extract(html,
            new RegexGroupNameMatch("""GMIC","projectId":"(?<projectid>.*)","pool":{""", "projectid"));
        var releaseId = _regexExtractor.Extract(html,
            new RegexGroupNameMatch("""_win64.exe","r":"(?<releaseid>.*)","hash":{"md5":""",
                "releaseid"))[..24];
        var fileName = _regexExtractor.Extract(html,
            new RegexGroupNameMatch("""_win64.exe"\sdata-file="(?<filename>.*)"\srel="nofollow""",
                "filename"));
    }
}