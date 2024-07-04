namespace Shiny.Extensions.Configuration.Remote;

public record RemoteConfig(
    string Uri, 
    bool WaitForLoadOnStartup = false,
    string ConfigurationFilePath = "remotesettings.json"
);