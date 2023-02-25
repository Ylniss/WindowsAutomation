using Microsoft.Extensions.DependencyInjection;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

namespace WindowsAutomation.InitAll.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddInitAllServices(this IServiceCollection services)
    {
        services.AddScoped<IInitAllRunner, WindowsInitAllRunner>();

        //services.AddScoped<IPackageInstaller, ChocoPackageInstaller>();
        services.AddScoped<IPackageInstaller, MyPackageInstaller>();

        //services.AddScoped<AppInstaller, NandeckAppInstaller>();
        //services.AddScoped<AppInstaller, ResolumeAppInstaller>();
        services.AddScoped<AppInstaller, GmicAppInstaller>();

        return services;
    }
}