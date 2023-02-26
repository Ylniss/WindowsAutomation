using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WindowsAutomation.Shared.Filesystem.DirCopier;

public class DirCopier : IDirCopier
{
    private readonly Subject<(string source, string destination)> _whenCopyStarted = new();
    public IObservable<(string source, string destination)> WhenCopyStarted => _whenCopyStarted.AsObservable();

    private readonly Subject<string> _whenSourceDirNotFound = new();
    public IObservable<string> WhenSourceDirNotFound => _whenSourceDirNotFound.AsObservable();

    private bool _eventRaised;

    public void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        if (!_eventRaised)
        {
            _whenCopyStarted.OnNext((sourceDir, destinationDir));
            _eventRaised = true;
        }

        var dir = new DirectoryInfo(sourceDir);

        if (!dir.Exists)
        {
            _whenSourceDirNotFound.OnNext(dir.FullName);
            _eventRaised = false;
            return;
        }

        var dirs = dir.GetDirectories();
        Directory.CreateDirectory(destinationDir);

        foreach (var fileInfo in dir.GetFiles())
        {
            var targetFilePath = Path.Combine(destinationDir, fileInfo.Name);
            fileInfo.CopyTo(targetFilePath, true);
        }

        if (!dirs.Any()) _eventRaised = false;
        if (!recursive) return;

        foreach (var subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir, true);
        }
    }
}