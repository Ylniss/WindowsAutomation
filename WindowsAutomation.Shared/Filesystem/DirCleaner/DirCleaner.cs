using System.Runtime.InteropServices;
using WindowsAutomation.Shared.Rx;

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
    public RxEvent<string> WhenRemove { get; } = new();

    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    private static extern uint SHEmptyRecycleBin(nint hwnd, string? pszRootPath, RecycleFlags dwFlags);

    public void RemoveAllFilesInDir(string directory)
    {
        WhenRemove.Act(directory, dirArg =>
        {
            DirectoryInfo directoryInfo = new(dirArg);
            foreach (var file in directoryInfo.EnumerateFiles())
                file.Delete();

            foreach (var dir in directoryInfo.EnumerateDirectories())
                dir.Delete(true);
        });
    }

    public void RemoveFileIfExists(string directory)
    {
        WhenRemove.Act(directory, dir =>
        {
            if (File.Exists(dir))
                File.Delete(dir);
        });
    }

    public void CleanRecycleBin()
    {
        WhenRemove.Act("Recycle Bin", _ =>
        {
            SHEmptyRecycleBin(nint.Zero, null,
                RecycleFlags.ShrbNoconfirmation | RecycleFlags.ShrbNoprogressui | RecycleFlags.ShrbNosound);
        });
    }
}