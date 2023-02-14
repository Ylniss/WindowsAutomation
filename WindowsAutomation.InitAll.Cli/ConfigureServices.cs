using Microsoft.Extensions.DependencyInjection;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Application.Installers;
using WindowsAutomation.InitAll.Application.Installers.Choco;
using WindowsAutomation.Shared;

namespace WindowsAutomation.InitAll.Cli;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IInitAllRunner, WindowsInitAllRunner>();

        return services;
    }

    public static IServiceCollection AddInstallerServices(this IServiceCollection services)
    {
        services.AddScoped<IInstaller, ChocoAppsInstaller>();
        services.AddScoped<IPackageProvider, JsonFilePackageProvider>();

        return services;
    }

    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IFileSerializer, JsonFileSerializer>();

        return services;
    }
}