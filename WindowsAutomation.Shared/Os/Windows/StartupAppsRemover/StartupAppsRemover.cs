using Microsoft.Win32;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.StartupAppsRemover;

public class StartupAppsRemover : IStartupAppsRemover
{
    public RxEvent<string> WhenAppStartupRemove { get; } = new();

    public void RemoveApp(string appPath)
    {
        WhenAppStartupRemove.Act(appPath, path =>
        {
            if (!OperatingSystem.IsWindows()) return;
            if (!File.Exists(path)) throw new FileNotFoundException($"File not found: '{path}'");

            var fileInfo = new FileInfo(path);

            var regKey =
                Registry.CurrentUser.OpenSubKey("""SOFTWARE\Microsoft\Windows\CurrentVersion\Run\AutorunsDisabled""",
                    true);
            regKey?.SetValue(Path.GetFileNameWithoutExtension(fileInfo.Name), path);
        });
    }
}