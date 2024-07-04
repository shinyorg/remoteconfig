namespace Shiny.Extensions.Configuration.Remote.Infrastructure;


public class RemoteConfigurationProvider(RemoteConfig config) : ConfigurationProvider, IRemoteConfigurationProvider
{
    const string LAST_LOAD_KEY = "__LastLoaded";
    public override void Load()
    {
        base.Load();
        if (File.Exists(config.ConfigurationFilePath))
            this.LastLoaded = File.GetLastWriteTime(config.ConfigurationFilePath);
        
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


    readonly SemaphoreSlim semaphore = new(1);
    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // var wasWaiting = this.semaphore.CurrentCount == 0;
            // there can only be one!
            await this.semaphore.WaitAsync(cancellationToken);

            if (this.LastLoaded != null)
            {
                var ts = DateTimeOffset.UtcNow.Subtract(this.LastLoaded.Value);
                if (ts.TotalSeconds < 30)
                    return;
            }
            // // if I was waiting, just return, the config will have been called and loaded
            // if (wasWaiting)
            //     return;

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(config.Uri, cancellationToken);

            await File
                .WriteAllTextAsync(config.ConfigurationFilePath, content, cancellationToken)
                .ConfigureAwait(false);

            this.LastLoaded = DateTimeOffset.UtcNow;
        }
        finally
        {
            this.semaphore.Release();
        }
    }
}