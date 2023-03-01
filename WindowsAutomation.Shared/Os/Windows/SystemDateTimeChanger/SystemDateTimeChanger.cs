using System.Diagnostics;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

public class SystemDateTimeChanger : ISystemDateTimeChanger
{
    public RxEvent<string> WhenTimeZoneChange { get; } = new();

    public void ChangeTimeZone(string timeZoneId)
    {
        WhenTimeZoneChange.Act(timeZoneId, s =>
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "tzutil.exe",
                Arguments = $"""/s "{timeZoneId}" """,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process != null)
            {
                process.WaitForExit();
                TimeZoneInfo.ClearCachedData();
            }
        });
    }
}