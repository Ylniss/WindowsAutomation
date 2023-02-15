namespace WindowsAutomation.Shared.Web;

public interface IWebDownloader
{
    Task<string> DownloadContent(string uri);
}