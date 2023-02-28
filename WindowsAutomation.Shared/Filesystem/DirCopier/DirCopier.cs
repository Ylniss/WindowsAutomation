using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Filesystem.DirCopier;

public class DirCopier : IDirCopier
{
    public RxEvent<(string source, string destination)> WhenCopy { get; } = new();

    private bool _eventRaised;

    public void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        WhenCopy.Act((sourceDir, destinationDir), _ =>
        {
            if (!_eventRaised) _eventRaised = true;

            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
            {
                _eventRaised = false;
                throw new DirectoryNotFoundException(dir.FullName);
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
        });
    }
}