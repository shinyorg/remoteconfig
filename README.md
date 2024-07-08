# Remote Configuration Provider

Work with the power of Microsoft.Extensions.Configuration while still being able to retrieve most configuration from a server (much like secrets).  For mobile, get the added benefit of having "cached" configuration in cases where connectivity is not available.  This allows you to update things like license keys, data endpoints, & more.  Could you do this without this library?  Yes - BUT you don't get IOptionsMonitor and have to work with another mediation service everywhere (if remote, use that value, else pull from local)

This library works great with .NET MAUI and ASP.NET Core applications
