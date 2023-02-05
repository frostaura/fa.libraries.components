using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FrostAura.Libraries.Components.Client;
using FrostAura.Libraries.Components.Models.Configuration;
using Newtonsoft.Json;
using FrostAura.Libraries.Components.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Client app config.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HTTP services.
builder
    .Services
    .AddHttpClient("default", c =>
    {
        c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    });
// Additional services.
builder
    .Services
    .AddSingleton<FrostAuraApplicationConfiguration>(sp =>
    {
        return new FrostAuraApplicationConfiguration();

        var httpClient = sp
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("default");
        var settingsStringTask = httpClient
            .GetStringAsync("settings.json");

        settingsStringTask.Wait();

        var settings = JsonConvert.DeserializeObject<FrostAuraApplicationConfiguration>(settingsStringTask.Result);

        return (FrostAuraApplicationConfiguration)settings;
    })
    .AddFrostAuraComponents(c => {});

await builder.Build().RunAsync();
