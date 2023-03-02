namespace WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

public enum Locale : uint
{
    SystemDefault = 0x0800,
    ShortDateFormat = 0x0000001F,
    LongDateFormat = 0x00000020,
    ShortTimeFormat = 0x00000079,
    LongTimeFormat = 0x00001003,
    FirstDayOfWeek = 0x0000100C
}