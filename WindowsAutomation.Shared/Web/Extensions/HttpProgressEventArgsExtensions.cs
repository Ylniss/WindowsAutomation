using System.Net.Http.Handlers;

namespace WindowsAutomation.Shared.Web.Extensions;

public static class HttpProgressEventArgsExtensions
{
    public static double? PreciseProgressPercentage(this HttpProgressEventArgs progress)
    {
        if (progress.TotalBytes == null) return null;

        return (double)(progress.BytesTransferred / progress.TotalBytes);
    }
}