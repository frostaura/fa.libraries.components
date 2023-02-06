using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FrostAura.Libraries.Components.Client;
using FrostAura.Libraries.Components.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder
    .Services
    .AddFrostAuraComponents(c =>
    {
        c.AppBaseUrl = builder.HostEnvironment.BaseAddress;
    });

await builder
    .Build()
    .RunAsync();
