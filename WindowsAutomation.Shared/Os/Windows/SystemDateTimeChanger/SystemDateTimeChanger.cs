using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

public class SystemDateTimeChanger : ISystemDateTimeChanger
{
    public RxEvent<string> WhenTimeZoneChange { get; } = new();

    public RxEvent<(Locale locale, string format)> WhenFormatChange { get; } = new();

    private const uint WmSettingChange = 0x001A;
    private readonly nint _hwndBroadcast = new(0xFFFF);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
    private static extern bool SetLocaleInfoA(uint locale, uint lcType, string lpLcData);

    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern nint SendMessage(
        nint hWnd, uint msg, nuint wParam, nint lParam);

    public void ChangeFormat(Locale locale, string format)
    {
        WhenFormatChange.Act((locale, format), data =>
        {
            if (data.locale is Locale.ShortTimeFormat)
            {
                ChangeFormatForShortTime(data.format);
                return;
            }

            var success = SetLocaleInfoA((uint)Locale.SystemDefault, (uint)data.locale, data.format);

            if (!success)
            {
                var error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException(
                    $"Error setting '{data.locale}' information with format '{data.format}'",
                    new Win32Exception(error));
            }


            SendMessage(_hwndBroadcast, WmSettingChange, nuint.Zero, nint.Zero);
        });
    }

    private void ChangeFormatForShortTime(string format)
    {
        if (!OperatingSystem.IsWindows()) return;
        var regKey =
            Registry.CurrentUser.OpenSubKey("""Control Panel\International""", true);
        if (regKey is null) throw new ApplicationException("Registry key not found");

        regKey.SetValue("sShortTime", format);

        var error = SystemParametersInfo(0x002A, 0, default, 0);
        if (error)
        {
            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode);
        }

        SendMessage(_hwndBroadcast, WmSettingChange, nuint.Zero, nint.Zero);
    }

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