using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Downloader.Dtos;

namespace WindowsAutomation.Shared.Web.Downloader;

public interface IWebDownloader
{
    public IObservable<string> WhenDownloadStarted { get; }
    public IObservable<double?> WhenDownloadProgressReceived { get; }

    Task<string> DownloadContent(string uri);
    Task DownloadFile(WebFileDownload webFileDownload);
    Task ExtractLinkAndDownloadFile(WebFileDownload webFileDownload, RegexGroupNameMatch regex, string prependUri = "");
}