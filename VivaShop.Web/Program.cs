using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VivaShop.Web;
using VivaShop.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseUri = "https://localhost:7228";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUri)});
builder.Services.AddScoped<IProdutoService, ProdutoService>();

await builder.Build().RunAsync();
