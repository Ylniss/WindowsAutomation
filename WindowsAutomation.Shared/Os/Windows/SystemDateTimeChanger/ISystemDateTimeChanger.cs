using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

public interface ISystemDateTimeChanger
{
    RxEvent<string> WhenTimeZoneChange { get; }
    RxEvent<(Locale locale, string format)> WhenFormatChange { get; }
    void ChangeTimeZone(string timeZoneId);
    void ChangeFormat(Locale locale, string format);
}