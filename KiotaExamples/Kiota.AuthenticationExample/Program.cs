using Azure.Identity;
using Kiota.AuthExample.ApiClient;
using Microsoft.Kiota.Authentication.Azure;
using Microsoft.Kiota.Bundle;
using Spectre.Console;

AnsiConsole.WriteLine("Reading from Azure Graph!");
var appId = Environment.GetEnvironmentVariable("AZURE_APP_ID");
if (string.IsNullOrEmpty(appId))
{
    AnsiConsole.Write(new Markup("Please set the [red]AZURE_APP_ID[/] environment variable to the correct value"));
    return;
}

AnsiConsole.WriteLine($"Azure App ID: {appId}");
// kiota generate -l csharp -d ./OpenApiDefinition/get-me.yaml -c GetUserApiClient -n Kiota.AuthExample.ApiClient -o ./Services

//get user reader with opening auth page
var allowedHosts = new[] { "graph.microsoft.com" };
var graphScopes = new[] { "User.Read" };

var options = new DeviceCodeCredentialOptions
{
    ClientId = appId,
    DeviceCodeCallback = (code, _) =>
    {
        AnsiConsole.WriteLine(code.Message);
        return Task.FromResult(0);
    },
};
var credential = new DeviceCodeCredential(options);
var authProvider = new AzureIdentityAuthenticationProvider(credential, allowedHosts, scopes: graphScopes);
var requestAdapter = new DefaultRequestAdapter(authProvider);
var client = new GetUserApiClient(requestAdapter);
var me = await client.Me.GetAsync();
ArgumentNullException.ThrowIfNull(me);
AnsiConsole.Write(new Markup($"Hello [bold red]{me.DisplayName}[/], your ID is [bold blue]{me.Id}[/]."));