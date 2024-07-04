namespace Shiny.Extensions.Configuration.RemoteHosting;

public interface IConfigurationResponder
{
    Task Get(string accessToken);
}