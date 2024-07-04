namespace Shiny.Extensions.Configuration.Remote;

public interface IRemoteConfigurationProvider : IConfigurationProvider
{
    DateTimeOffset? LastLoaded { get; }
    Task LoadAsync(CancellationToken cancellationToken = default);
}