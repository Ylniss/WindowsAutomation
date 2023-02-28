using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Filesystem.DirCopier;

public interface IDirCopier
{
    public RxEvent<(string source, string destination)> WhenCopy { get; }

    void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true);
}