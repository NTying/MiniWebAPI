using MiniWebAPI;
using MiniWebAPI.Filter;
using MiniWebAPIDemo.Filter;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
ActionLocator locator = new ActionLocator(services, Assembly.GetEntryAssembly()!);
services.AddSingleton(locator);
services.AddMemoryCache();
ActionFilters.filters.Add(new MyActionFilter1());
var app = builder.Build();

app.UseMiddleware<MyStaticFilesMiddleware>();
app.UseMiddleware<MyWebAPIMiddleware>();
app.UseMiddleware<NotFoundMiddleware>();

app.Run();
