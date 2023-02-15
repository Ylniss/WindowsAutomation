namespace WindowsAutomation.Shared.Web;

public class WebDownloader : IWebDownloader
{
    private readonly HttpClient _HttpClient = new();

    public async Task<string> DownloadContent(string uri)
    {
        var response = await _HttpClient.GetAsync(uri);
        return await response.Content.ReadAsStringAsync();
    }
}