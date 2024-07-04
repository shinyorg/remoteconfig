using Shiny.Extensions.Configuration.Remote;

namespace Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiApp<App>();

        builder
            .Configuration
            // we load the default/builtin accesstoken here
            .AddJsonPlatformBundle()
            
            // if remote has requested past values, those will be available here through configuration
            // so you could basically change the URI & AccessKey below
            .AddRemote(
                Path.Combine(FileSystem.AppDataDirectory, "remotesettings.json"),
                cfg => (
                    cfg["ConfigurationUri"]!, 
                    cfg["AccessToken"]!
                ),
                false
           );

        builder.Services.Configure<MyConfig>(x =>
        {
            
            builder.Configuration.Bind(x);
        });
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