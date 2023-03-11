using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.StartupAppsRemover;

public interface IStartupAppsRemover
{
    RxEvent<string> WhenAppStartupRemove { get; }
    void RemoveApp(string appPath);
}