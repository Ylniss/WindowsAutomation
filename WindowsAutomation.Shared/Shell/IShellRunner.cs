namespace WindowsAutomation.Shared.Shell;

public interface IShellRunner
{
    IObservable<string> WhenOutputReceived { get; }
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    void RunScript(string script);
    Task RunScriptFromWeb(string scriptUri);
}