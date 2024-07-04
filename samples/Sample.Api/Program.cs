using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapGet(
    "/configuration", 
    async (
        [FromServices] IHttpContextAccessor http,
        [FromServices] IConfiguration cfg
    ) =>
    {
        var accessKey = http.HttpContext!.Request.Headers["X-Key"];
        if (accessKey != cfg["AccessKey"])
            throw new InvalidCredentialException("You do not belong here");
        
        await Task.Delay(5000);
        return new
        {
            ValueFrom = "Server",
            TheValue = DateTimeOffset.Now.ToString("yyyy MMMM dd - h:mm:ss tt")
        };
    }
);

app.Run();
