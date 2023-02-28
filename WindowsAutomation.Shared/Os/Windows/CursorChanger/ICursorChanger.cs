using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.CursorChanger;

public interface ICursorChanger
{
    RxEvent<Theme> WhenCursorThemeSet { get; }
    void SetCursorTheme(Theme theme);
}