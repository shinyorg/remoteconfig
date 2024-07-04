namespace Shiny.Extensions.Configuration.Remote.Infrastructure;

public class RemoteConfigurationSource(RemoteConfig config) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new RemoteConfigurationProvider(config);
}