using Shiny.Extensions.Configuration.Remote.Maui;

namespace Sample;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiApp<App>();

        // we load the default/builtin accesstoken here
        builder.Configuration.AddJsonPlatformBundle();

        // if remote has requested past values, those will be available here through configuration
        // so you could basically change the URI & AccessKey below
        // calling this method adds IRemoteConfigurationProvider to DI which allows you to await the configuration in a startup page if needed
        builder.AddRemoteMaui();

        // The extension method `BindConfiguration` is part of Microsoft.Extensions.Options.ConfigurationExtensions - don't hate the messenger
        builder.Services.AddOptions<MyConfig>().BindConfiguration("");
#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainViewModel>();
        var app = builder.Build();

        return app;
    }
}