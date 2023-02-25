namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public interface IDirMaker
{
    public IObservable<string> WhenMakeDirStarted { get; }
    void MakeDirForFileIfNotExists(string fileDestination);
    void MakeDirIfNotExists(string path);
}