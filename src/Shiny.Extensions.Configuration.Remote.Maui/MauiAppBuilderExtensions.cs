namespace Shiny.Extensions.Configuration.Remote.Maui;


public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddRemoteMaui(this MauiAppBuilder builder, string configurationFileName = "remotesettings.json")
    {
        builder.Configuration.AddRemote(
            Path.Combine(FileSystem.AppDataDirectory, configurationFileName),
            config => config["ConfigurationUri"]!,
            false,
            builder.Services
        );
        return builder;
    }   
}