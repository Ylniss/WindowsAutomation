using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.CursorChanger;

public class CursorChanger : ICursorChanger
{
    public RxEvent<Theme> WhenCursorThemeSet { get; } = new();

    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public void SetCursorTheme(Theme theme)
    {
        if (!OperatingSystem.IsWindows()) return;
        var cursorsRegKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\\Cursors", true);
        if (cursorsRegKey is null) return;

        WhenCursorThemeSet.Act(theme, t =>
        {
            if (theme == Theme.Dark)
            {
                cursorsRegKey.SetValue(string.Empty, "Windows Black");
                cursorsRegKey.SetValue("AppStarting", @"%SystemRoot%\cursors\wait_r.cur");
                cursorsRegKey.SetValue("Arrow", @"%SystemRoot%\cursors\arrow_r.cur");
                cursorsRegKey.SetValue("Crosshair", @"%SystemRoot%\cursors\cross_r.cur");
                cursorsRegKey.SetValue("Hand", string.Empty);
                cursorsRegKey.SetValue("Help", @"%SystemRoot%\cursors\help_r.cur");
                cursorsRegKey.SetValue("IBeam", @"%SystemRoot%\cursors\beam_r.cur");
                cursorsRegKey.SetValue("No", @"%SystemRoot%\cursors\no_r.cur");
                cursorsRegKey.SetValue("NWPen", @"%SystemRoot%\cursors\pen_r.cur");
                cursorsRegKey.SetValue("Person", @"%SystemRoot%\cursors\person_r.cur");
                cursorsRegKey.SetValue("Pin", @"%SystemRoot%\cursors\pin_r.cur");
                cursorsRegKey.SetValue("SizeAll", @"%SystemRoot%\cursors\move_r.cur");
                cursorsRegKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\size1_r.cur");
                cursorsRegKey.SetValue("SizeNS", @"%SystemRoot%\cursors\size4_r.cur");
                cursorsRegKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\size2_r.cur");
                cursorsRegKey.SetValue("SizeWE", @"%SystemRoot%\cursors\size3_r.cur");
                cursorsRegKey.SetValue("UpArrow", @"%SystemRoot%\cursors\up_r.cur");
                cursorsRegKey.SetValue("Wait", @"%SystemRoot%\cursors\busy_r.cur");
            }
            else if (theme == Theme.Light)
            {
                cursorsRegKey.SetValue(string.Empty, "Windows Default");
                cursorsRegKey.SetValue("AppStarting", @"%SystemRoot%\cursors\aero_working.ani");
                cursorsRegKey.SetValue("Arrow", @"%SystemRoot%\cursors\aero_arrow.cur");
                cursorsRegKey.SetValue("Crosshair", string.Empty);
                cursorsRegKey.SetValue("Hand", @"%SystemRoot%\cursors\aero_link.cur");
                cursorsRegKey.SetValue("Help", @"%SystemRoot%\cursors\aero_helpsel.cur");
                cursorsRegKey.SetValue("IBeam", string.Empty);
                cursorsRegKey.SetValue("No", @"%SystemRoot%\cursors\aero_unavail.cur");
                cursorsRegKey.SetValue("NWPen", @"%SystemRoot%\cursors\aero_pen.cur");
                cursorsRegKey.SetValue("Person", @"%SystemRoot%\cursors\aero_person.cur");
                cursorsRegKey.SetValue("Pin", @"%SystemRoot%\cursors\aero_pin.cur");
                cursorsRegKey.SetValue("SizeAll", @"%SystemRoot%\cursors\aero_move.cur");
                cursorsRegKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\aero_nesw.cur");
                cursorsRegKey.SetValue("SizeNS", @"%SystemRoot%\cursors\aero_ns.cur");
                cursorsRegKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\aero_nwse.cur");
                cursorsRegKey.SetValue("SizeWE", @"%SystemRoot%\cursors\aero_ew.cur");
                cursorsRegKey.SetValue("UpArrow", @"%SystemRoot%\cursors\aero_up.cur");
                cursorsRegKey.SetValue("Wait", @"%SystemRoot%\cursors\aero_busy.cur");
            }

            SystemParametersInfo((uint)SysFlag.SpiSetcursors, 0, default,
                (uint)SysFlag.SpifUpdateinifile | (uint)SysFlag.SpifSendchange);
        });
    }
}