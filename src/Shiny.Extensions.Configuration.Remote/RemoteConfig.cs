namespace Shiny.Extensions.Configuration.Remote;

public record RemoteConfig(
    string Uri, 
    string AccessToken, 
    bool WaitForLoadOnStartup = false,
    string ConfigurationFilePath = "remotesettings.json"
);