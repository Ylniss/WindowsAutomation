namespace WindowsAutomation.Shared.Web.Dtos;

public record WebFileDownload(string Uri, string Destination, IProgress<double>? Progress);