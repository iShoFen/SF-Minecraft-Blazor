using Blazored.Modal;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Model.Services;
using SF_Minecraft_Blazor.Services;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();

    // Add Blazorise
    builder.Services
           .AddBlazorise()
           .AddBootstrapProviders()
           .AddFontAwesomeIcons();

    builder.Services.AddBlazoredModal();

    builder.Services.AddScoped<HttpClient>(_ => new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration["ApiUrl"])
        }
    );

    builder.Services.AddScoped<IDataItemListService, DataItemListService>();
    builder.Services.AddScoped<IDataInventoryService, DataInventoryService>();
    
    // Setup NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}