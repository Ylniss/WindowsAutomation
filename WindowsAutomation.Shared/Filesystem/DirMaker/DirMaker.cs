using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public class DirMaker : IDirMaker
{
    private readonly Subject<string> _whenMakeDirStarted = new();
    public IObservable<string> WhenMakeDirStarted => _whenMakeDirStarted.AsObservable();

    public void MakeDirForFileIfNotExists(string fileDestination)
    {
        var directory = Directory.GetParent(fileDestination)?.FullName;
        MakeDirIfNotExists(directory);
    }

    public void MakeDirIfNotExists(string? path)
    {
        if (string.IsNullOrEmpty(path) || Directory.Exists(path)) return;

        _whenMakeDirStarted.OnNext(path);
        Directory.CreateDirectory(path);
    }
}