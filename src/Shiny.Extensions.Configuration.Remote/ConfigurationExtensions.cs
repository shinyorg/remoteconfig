using Shiny.Extensions.Configuration.Remote.Infrastructure;

namespace Shiny.Extensions.Configuration.Remote;


public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds remote configuration to the configuration pipeline
    /// </summary>
    /// <param name="builder">The configuration builder</param>
    /// <param name="configurationFilePath">The location of where the remote settings should be persisted</param>
    /// <param name="configurator">This allows you to get the configuration values the previous remote call & allows you to update the ConfigurationUri & AccessToken</param>
    /// <param name="waitForRemoteLoad">If you want the network call to be waited until completion before returning</param>
    /// <returns>The current configuration builder to allow for chaining</returns>
    public static IConfigurationBuilder AddRemote(this IConfigurationBuilder builder, string configurationFilePath, Func<IConfiguration, (string ConfigurationUri, string AccessToken)> configurator, bool waitForRemoteLoad = false)
    {
        
        builder.AddJsonFile(configurationFilePath, true, true);
        var configuration = builder.Build();
        var values = configurator.Invoke(configuration);
        builder.Add(new RemoteConfigurationSource(new RemoteConfig(
            values.ConfigurationUri,
            values.AccessToken,
            waitForRemoteLoad,
            configurationFilePath
        )));
        
        // TODO: get remote source provider to allow a startup routine to call load OUTSIDE of DI (great for mobile where the app startup can't wait)
        return builder;
    }
}