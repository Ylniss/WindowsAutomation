namespace WindowsAutomation.Shared.Filesystem.DirCopier;

public interface IDirCopier
{
    IObservable<(string source, string destination)> WhenCopyStarted { get; }
    IObservable<string> WhenSourceDirNotFound { get; }
    void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true);
}