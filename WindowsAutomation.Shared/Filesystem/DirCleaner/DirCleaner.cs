using System.Runtime.InteropServices;

namespace WindowsAutomation.Shared.Filesystem.DirCleaner;

internal enum RecycleFlags : uint
{
    ShrbNoconfirmation = 0x00000001, // Don't ask confirmation
    ShrbNoprogressui = 0x00000002, // Don't show any windows dialog
    ShrbNosound = 0x00000004 // Don't make sound, ninja mode enabled :v
}

public class DirCleaner : IDirCleaner
{
    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    private static extern uint SHEmptyRecycleBin(nint hwnd, string? pszRootPath, RecycleFlags dwFlags);

    public void RemoveAllFilesInDir(string directory)
    {
        DirectoryInfo directoryInfo = new(directory);
        foreach (var file in directoryInfo.EnumerateFiles())
            file.Delete();

        foreach (var dir in directoryInfo.EnumerateDirectories())
            dir.Delete(true);
    }

    public void CleanRecycleBin()
    {
        SHEmptyRecycleBin(nint.Zero, null, RecycleFlags.ShrbNoconfirmation);
    }
}