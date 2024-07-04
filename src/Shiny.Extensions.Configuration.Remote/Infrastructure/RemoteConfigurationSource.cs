using Microsoft.Extensions.DependencyInjection;

namespace Shiny.Extensions.Configuration.Remote.Infrastructure;

public class RemoteConfigurationSource(RemoteConfig config, IServiceCollection? services) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        var provider = new RemoteConfigurationProvider(config);
        services?.AddSingleton<IRemoteConfigurationProvider>(provider);
        return provider;
    }
}