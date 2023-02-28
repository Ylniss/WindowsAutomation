using Microsoft.Win32;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.StartupAppsAdder;

public class StartupAppsAdder : IStartupAppsAdder
{
    public RxEvent<string> WhenAppStartupAdd { get; } = new();

    public void AddApp(string appPath)
    {
        WhenAppStartupAdd.Act(appPath, path =>
        {
            if (!OperatingSystem.IsWindows()) return;
            if (!File.Exists(path)) throw new FileNotFoundException($"File not found: '{path}'");

            var fileInfo = new FileInfo(path);

            var regKey = Registry.CurrentUser.OpenSubKey("""SOFTWARE\Microsoft\Windows\CurrentVersion\Run""", true);
            regKey?.SetValue(Path.GetFileNameWithoutExtension(fileInfo.Name), path);
        });
    }
}