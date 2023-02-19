namespace WindowsAutomation.Shared.Serializers;

public interface IFileSerializer
{
    T? DeserializeFromFile<T>(string path);
}