using Microsoft.Extensions.DependencyInjection;
using WindowsAutomation.InitAll.Application.PackageInstallers;

namespace WindowsAutomation.InitAll.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddInitAllServices(this IServiceCollection services)
    {
        services
            .AddScoped<IInitAllRunner, WindowsInitAllRunner>()
            .AddScoped<IPackageInstaller, ChocoPackageInstaller>();
        //.AddScoped<IPackageInstaller, MyPackageInstaller>()

        //.AddScoped<AppInstaller, NandeckAppInstaller>()
        //.AddScoped<AppInstaller, ResolumeAppInstaller>()
        //.AddScoped<AppInstaller, GmicAppInstaller>();

        return services;
    }
}