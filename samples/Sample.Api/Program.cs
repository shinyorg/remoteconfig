var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapGet(
    "/configuration", 
    async (CancellationToken ct) =>
    {
        await Task.Delay(5000, ct);
        return new
        {
            ValueFrom = "Server",
            TheValue = DateTimeOffset.Now.ToString("yyyy MMMM dd - h:mm:ss tt")
        };
    }
);

app.Run();
