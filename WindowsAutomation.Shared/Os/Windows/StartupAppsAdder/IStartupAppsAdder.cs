using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.StartupAppsAdder;

public interface IStartupAppsAdder
{
    RxEvent<string> WhenAppStartupAdd { get; }
    void AddApp(string appPath);
}