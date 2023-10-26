using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Welfony.App;
using Welfony.App.Common.Configuration;
using Welfony.App.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<ConfigurationBase>();
builder.Services.AddSingleton<AppState>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7056/api/") });

await builder.Build().RunAsync();
