namespace Shiny.Extensions.Configuration.Remote.Maui;


public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddRemoteConfigurationMaui(
        this MauiAppBuilder builder, 
        Func<CancellationToken, Task<object>>? getData = null, 
        string configurationFileName = "remotesettings.json"
    )
    {
        builder.Configuration.AddRemote(
            Path.Combine(FileSystem.AppDataDirectory, configurationFileName),
            config => config["ConfigurationUri"]!,
            getData,
            false,
            builder.Services
        );
        return builder;
    }   
}