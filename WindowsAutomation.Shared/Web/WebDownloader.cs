using System.Net.Http.Handlers;
using WindowsAutomation.Shared.RegularExpression;

namespace WindowsAutomation.Shared.Web;

public record WebFileDownload(string Uri, string Destination, IProgress<double>? Progress);

public class WebDownloader : IWebDownloader
{
    private readonly IRegexExtractor _regexExtractor;
    private readonly HttpClient _HttpClient;

    private IProgress<double> _progress;

    public WebDownloader(IRegexExtractor regexExtractor)
    {
        _regexExtractor = regexExtractor;
        _HttpClient = SetupProgressHandlers();
    }

    public async Task<string> DownloadContent(string uri)
    {
        var response = await _HttpClient.GetAsync(uri);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task DownloadFile(WebFileDownload webFileDownload)
    {
        if (webFileDownload.Progress is not null)
            _progress = webFileDownload.Progress;

        var stream = await _HttpClient.GetStreamAsync(webFileDownload.Uri);

        await using var fileStream = new FileStream(webFileDownload.Destination, FileMode.OpenOrCreate);
        await stream.CopyToAsync(fileStream);
    }

    public async Task ExtractLinkAndDownloadFile(WebFileDownload webFileDownload, RegexGroupNameMatch regex)
    {
        var html = await DownloadContent(webFileDownload.Uri);
        var downloadUrl = _regexExtractor.Extract(html, regex);

        await DownloadFile(webFileDownload with { Uri = downloadUrl });
    }

    private HttpClient SetupProgressHandlers()
    {
        var handler = new HttpClientHandler { AllowAutoRedirect = true };
        var progressHandler = new ProgressMessageHandler(handler);

        progressHandler.HttpSendProgress += (_, args) =>
        {
            _progress?.Report((double)args.BytesTransferred / args.TotalBytes ?? 0);
        };

        progressHandler.HttpReceiveProgress += (_, args) =>
        {
            _progress?.Report((double)args.BytesTransferred / args.TotalBytes ?? 0);
        };

        return new HttpClient(progressHandler);
    }
}