using Microsoft.Extensions.DependencyInjection;
using WindowsAutomation.Shared.Filesystem.Compression;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.RegularExpression;
using WindowsAutomation.Shared.Shell;
using WindowsAutomation.Shared.Web;
using WindowsAutomation.Shared.Web.Downloader;

namespace WindowsAutomation.Shared;

public static class ConfigureServices
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IMyHttpClientFactory, MyHttpClientFactory>();
        services.AddScoped<IShellRunner, PowerShellRunner>();
        services.AddScoped<IWebDownloader, WebDownloader>();
        services.AddScoped<IRegexExtractor, RegexExtractor>();
        services.AddScoped<IZipper, Zipper>();
        services.AddScoped<IDirMaker, DirMaker>();
        services.AddScoped<IDirCleaner, DirCleaner>();

        return services;
    }
}