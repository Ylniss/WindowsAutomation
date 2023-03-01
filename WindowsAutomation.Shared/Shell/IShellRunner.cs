using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Shell;

public interface IShellRunner
{
    public RxEvent<string> WhenOutputReceive { get; }
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    void RunScript(string script);
    Task RunScriptFromWeb(string scriptUri);
}