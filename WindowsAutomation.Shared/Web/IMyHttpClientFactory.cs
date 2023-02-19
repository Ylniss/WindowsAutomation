namespace WindowsAutomation.Shared.Web;

public interface IMyHttpClientFactory
{
    IObservable<double?> WhenDownloadProgressReceived { get; }
    IObservable<double?> WhenUploadProgressReceived { get; }
    HttpClient CreateWithProgress();
}