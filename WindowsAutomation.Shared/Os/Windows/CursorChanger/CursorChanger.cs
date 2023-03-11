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
        var regKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\\Cursors", true);
        if (regKey is null) throw new ApplicationException("Registry key not found");

        WhenCursorThemeSet.Act(theme, t =>
        {
            if (theme == Theme.Dark)
            {
                regKey.SetValue(string.Empty, "Windows Black");
                regKey.SetValue("AppStarting", @"%SystemRoot%\cursors\wait_r.cur");
                regKey.SetValue("Arrow", @"%SystemRoot%\cursors\arrow_r.cur");
                regKey.SetValue("Crosshair", @"%SystemRoot%\cursors\cross_r.cur");
                regKey.SetValue("Hand", string.Empty);
                regKey.SetValue("Help", @"%SystemRoot%\cursors\help_r.cur");
                regKey.SetValue("IBeam", @"%SystemRoot%\cursors\beam_r.cur");
                regKey.SetValue("No", @"%SystemRoot%\cursors\no_r.cur");
                regKey.SetValue("NWPen", @"%SystemRoot%\cursors\pen_r.cur");
                regKey.SetValue("Person", @"%SystemRoot%\cursors\person_r.cur");
                regKey.SetValue("Pin", @"%SystemRoot%\cursors\pin_r.cur");
                regKey.SetValue("SizeAll", @"%SystemRoot%\cursors\move_r.cur");
                regKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\size1_r.cur");
                regKey.SetValue("SizeNS", @"%SystemRoot%\cursors\size4_r.cur");
                regKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\size2_r.cur");
                regKey.SetValue("SizeWE", @"%SystemRoot%\cursors\size3_r.cur");
                regKey.SetValue("UpArrow", @"%SystemRoot%\cursors\up_r.cur");
                regKey.SetValue("Wait", @"%SystemRoot%\cursors\busy_r.cur");
            }
            else if (theme == Theme.Light)
            {
                regKey.SetValue(string.Empty, "Windows Default");
                regKey.SetValue("AppStarting", @"%SystemRoot%\cursors\aero_working.ani");
                regKey.SetValue("Arrow", @"%SystemRoot%\cursors\aero_arrow.cur");
                regKey.SetValue("Crosshair", string.Empty);
                regKey.SetValue("Hand", @"%SystemRoot%\cursors\aero_link.cur");
                regKey.SetValue("Help", @"%SystemRoot%\cursors\aero_helpsel.cur");
                regKey.SetValue("IBeam", string.Empty);
                regKey.SetValue("No", @"%SystemRoot%\cursors\aero_unavail.cur");
                regKey.SetValue("NWPen", @"%SystemRoot%\cursors\aero_pen.cur");
                regKey.SetValue("Person", @"%SystemRoot%\cursors\aero_person.cur");
                regKey.SetValue("Pin", @"%SystemRoot%\cursors\aero_pin.cur");
                regKey.SetValue("SizeAll", @"%SystemRoot%\cursors\aero_move.cur");
                regKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\aero_nesw.cur");
                regKey.SetValue("SizeNS", @"%SystemRoot%\cursors\aero_ns.cur");
                regKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\aero_nwse.cur");
                regKey.SetValue("SizeWE", @"%SystemRoot%\cursors\aero_ew.cur");
                regKey.SetValue("UpArrow", @"%SystemRoot%\cursors\aero_up.cur");
                regKey.SetValue("Wait", @"%SystemRoot%\cursors\aero_busy.cur");
            }

            SystemParametersInfo((uint)SysFlag.SpiSetcursors, 0, default,
                (uint)SysFlag.SpifUpdateinifile | (uint)SysFlag.SpifSendchange);
        });
    }
}