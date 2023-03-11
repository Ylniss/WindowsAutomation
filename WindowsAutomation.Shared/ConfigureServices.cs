using Microsoft.Extensions.DependencyInjection;
using WindowsAutomation.Shared.Filesystem.Compression;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirCopier;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.Filesystem.Serializers;
using WindowsAutomation.Shared.Git;
using WindowsAutomation.Shared.Os.Windows.CursorChanger;
using WindowsAutomation.Shared.Os.Windows.Pinner;
using WindowsAutomation.Shared.Os.Windows.StartupAppsAdder;
using WindowsAutomation.Shared.Os.Windows.StartupAppsRemover;
using WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;
using WindowsAutomation.Shared.RegularExpression;
using WindowsAutomation.Shared.Shell;
using WindowsAutomation.Shared.Web;
using WindowsAutomation.Shared.Web.Downloader;

namespace WindowsAutomation.Shared;

public static class ConfigureServices
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services
            .AddScoped<IMyHttpClientFactory, MyHttpClientFactory>()
            .AddScoped<IShellRunner, PowerShellRunner>()
            .AddScoped<IWebDownloader, WebDownloader>()
            .AddScoped<IRegexExtractor, RegexExtractor>()
            .AddScoped<IFileSerializer, JsonFileSerializer>()
            .AddScoped<IZipper, Zipper>()
            .AddScoped<IDirMaker, DirMaker>()
            .AddScoped<IDirCleaner, DirCleaner>()
            .AddScoped<IDirCopier, DirCopier>()
            .AddScoped<IGitClient, GitClient>()
            .AddScoped<ICursorChanger, CursorChanger>()
            .AddScoped<IPinner, Pinner>()
            .AddScoped<IStartupAppsAdder, StartupAppsAdder>()
            .AddScoped<IStartupAppsRemover, StartupAppsRemover>()
            .AddScoped<ISystemDateTimeChanger, SystemDateTimeChanger>();

        return services;
    }
}