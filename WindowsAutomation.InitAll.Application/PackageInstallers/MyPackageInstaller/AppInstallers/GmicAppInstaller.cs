using CliWrap;
using CliWrap.Buffered;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.RegularExpression;
using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

internal record Data(string Url);

internal record GmicServerResponse(Data Data);

public class GmicAppInstaller : AppInstaller
{
    private readonly IRegexExtractor _regexExtractor;
    public override string AppName => "Gmic";
    public override string SetupPath => $"""{Constants.CommonPaths.Software}\GMIC.exe""";

    public GmicAppInstaller(IWebDownloader webDownloader, IRegexExtractor regexExtractor) : base(webDownloader)
    {
        _regexExtractor = regexExtractor;
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
        var html = await WebDownloader.DownloadContent("""https://www.fosshub.com/GMIC.html""");

        var projectId = _regexExtractor.Extract(html,
            new RegexGroupNameMatch("""GMIC","projectId":"(?<projectid>.*)","pool":{""", "projectid"));
        var releaseId = _regexExtractor.Extract(html,
            new RegexGroupNameMatch("""_win64.exe","r":"(?<releaseid>.*)","hash":{"md5":""",
                "releaseid"))[..24];
        var fileName = _regexExtractor.Extract(html,
            new RegexGroupNameMatch("""_win64.exe"\sdata-file="(?<filename>.*)"\srel="nofollow""",
                "filename"));

        var response = await WebDownloader.HttpClient.PostAsync("""https://api.fosshub.com/download/""",
            new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("fileName", fileName),
                new KeyValuePair<string, string>("projectUri", "GMIC.html"),
                new KeyValuePair<string, string>("source", "CF"),
                new KeyValuePair<string, string>("projectId", projectId),
                new KeyValuePair<string, string>("releaseId", releaseId)
            }));

        var content = await response.Content.ReadAsAsync<GmicServerResponse>();

        await WebDownloader.DownloadFile(new WebFileDownload(content.Data.Url, SetupPath));
    }
}