using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Configuration.Remote.Infrastructure;

namespace Shiny.Extensions.Configuration.Remote;


public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds remote configuration to the configuration pipeline
    /// </summary>
    /// <param name="builder">The configuration builder</param>
    /// <param name="configurationFilePath">The location of where the remote settings should be persisted</param>
    /// <param name="getConfigurationUri">This allows you to get the configuration URI from the previous remote call & allows you to update the ConfigurationUri if needed</param>
    /// <param name="getData">If you wish to control how/what data is returned, pass this function</param>
    /// <param name="waitForRemoteLoad">If you want the network call to be waited until completion before returning</param>
    /// <param name="services">If presented to the extension method, IRemoteConfigurationProvider is installed to the service container</param>
    /// <returns>The current configuration builder to allow for chaining</returns>
    public static IConfigurationBuilder AddRemote(
        this IConfigurationBuilder builder, 
        string configurationFilePath, 
        string configurationUri,
        Func<RemoteConfig, CancellationToken, Task<object>>? getData = null,
        bool waitForRemoteLoad = true,
        IServiceCollection? services = null
    )
    {
        builder.AddJsonFile(configurationFilePath, true, true);
        var configuration = builder.Build();
        builder.Add(new RemoteConfigurationSource(new RemoteConfig(
            configurationUri,
            configuration,
            waitForRemoteLoad,
            configurationFilePath
        ), getData, services));
        
        return builder;
    }
}