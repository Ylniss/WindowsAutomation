using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;

namespace WindowsAutomation.Shared.Filesystem.DirCleaner;

[Flags]
internal enum RecycleFlags : uint
{
    ShrbNoconfirmation = 0x00000001, // Don't ask confirmation
    ShrbNoprogressui = 0x00000002, // Don't show any windows dialog
    ShrbNosound = 0x00000004 // Don't make sound, ninja mode enabled :v
}

public class DirCleaner : IDirCleaner
{
    private readonly Subject<string> _whenRemoveStarted = new();
    public IObservable<string> WhenRemoveStarted => _whenRemoveStarted.AsObservable();

    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    private static extern uint SHEmptyRecycleBin(nint hwnd, string? pszRootPath, RecycleFlags dwFlags);

    public void RemoveAllFilesInDir(string directory)
    {
        _whenRemoveStarted.OnNext(directory);

        DirectoryInfo directoryInfo = new(directory);
        foreach (var file in directoryInfo.EnumerateFiles())
            file.Delete();

        foreach (var dir in directoryInfo.EnumerateDirectories())
            dir.Delete(true);
    }

    public void RemoveFileIfExists(string directory)
    {
        _whenRemoveStarted.OnNext(directory);

        if (File.Exists(directory))
            File.Delete(directory);
    }

    public void CleanRecycleBin()
    {
        SHEmptyRecycleBin(nint.Zero, null,
            RecycleFlags.ShrbNoconfirmation | RecycleFlags.ShrbNoprogressui | RecycleFlags.ShrbNosound);
    }
}