namespace Shiny.Extensions.Configuration.Remote.Infrastructure;


public class RemoteConfigurationProvider(RemoteConfig config) : ConfigurationProvider, IRemoteConfigurationProvider
{
    const string LAST_LOAD_KEY = "__LastLoaded";
    public override void Load()
    {
        base.Load();
        if (config.WaitForLoadOnStartup)
        {
            this.LoadAsync().GetAwaiter().GetResult();
        }
        else
        {
            this.LoadAsync().ContinueWith(x =>
            {
                if (x.IsFaulted)
                    Console.WriteLine(x.Exception);
            });
        }
    }


    public DateTimeOffset? LastLoaded 
    {
        get
        {
            if (!this.Data.ContainsKey(LAST_LOAD_KEY))
                return null;

            return DateTimeOffset.Parse(this.Data[LAST_LOAD_KEY]);
        }
        private set => this.Data[LAST_LOAD_KEY] = value.ToString();
    }
    
    
    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-KEY", config.AccessToken);
        var content = await httpClient.GetStringAsync(config.Uri, cancellationToken);

        // TODO: this is not triggering changes from the json provider - could be the options monitor too?
        await File
            .WriteAllTextAsync(config.ConfigurationFilePath, content, cancellationToken)
            .ConfigureAwait(false);

        this.LastLoaded = DateTimeOffset.UtcNow;
        // this.OnReload();
    }
}