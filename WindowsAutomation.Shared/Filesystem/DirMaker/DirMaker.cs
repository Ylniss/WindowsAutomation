using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public class DirMaker : IDirMaker
{
    public RxEvent<string> WhenMake { get; } = new();
    public RxEvent<(string source, string destination)> WhenShortcutMake { get; } = new();

    public void MakeDirForFileIfNotExists(string fileDestination)
    {
        var directory = Directory.GetParent(fileDestination)?.FullName;
        if (directory is not null)
            MakeDirIfNotExists(directory);
    }

    public void MakeDirIfNotExists(string path)
    {
        if (string.IsNullOrEmpty(path) || Directory.Exists(path)) return;

        WhenMake.Act(path, dirPath => Directory.CreateDirectory(dirPath));
    }

    public void MakeDirShortcut(string source, string destination)
    {
        WhenShortcutMake.Act((source, destination), tuple =>
            Directory.CreateSymbolicLink(tuple.destination, tuple.source));
    }
}