using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Shiny.Extensions.Configuration.Remote;

namespace Sample;


public partial class MainViewModel : ObservableObject
{
    readonly IRemoteConfigurationProvider remoteProvider;
    
    public MainViewModel(
        IRemoteConfigurationProvider remoteProvider, 
        IOptionsMonitor<MyConfig> cfg
    )
    {
        this.remoteProvider = remoteProvider;
        cfg.OnChange(this.Update);
        this.Update(cfg.CurrentValue);
    }

    
    [ObservableProperty] string accessToken;
    [ObservableProperty] string configurationUri;
    [ObservableProperty] string? theValue;
    [ObservableProperty] string valueFrom;
    [ObservableProperty] string lastLoad;

    void Update(MyConfig config)
    {
        this.AccessToken = config.AccessToken;
        this.ConfigurationUri = config.ConfigurationUri;
        this.ValueFrom = config.ValueFrom;
        this.TheValue = config.TheValue;

        this.LastLoad = this.remoteProvider
            .LastLoaded?
            .ToLocalTime()
            .ToString("yyyy MMMM dd - h:mm:ss tt") ?? "Never";
    }


    [RelayCommand]
    async Task Refresh(CancellationToken cancellationToken)
    {
        try
        {
            await this.remoteProvider.LoadAsync(cancellationToken);
        }
        catch (OperationCanceledException) {}
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(ex.ToString(), "ERROR", "OK");
        }
    }
}