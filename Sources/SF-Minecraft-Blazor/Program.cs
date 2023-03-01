using System.Globalization;
using Blazored.Modal;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
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

// Add the controller of the app
    builder.Services.AddControllers();

// Add the localization to the app and specify the resources path
    builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

// Configure the localtization
    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        // Set the default culture of the web site
        options.DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"));

        // Declare the supported culture
        options.SupportedCultures = new List<CultureInfo> { new("en-US"), new("fr-FR") };
        options.SupportedUICultures = new List<CultureInfo> { new("en-US"), new("fr-FR") };
    });

    builder.Services.AddScoped<HttpClient>(_ => new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["ApiUrl"])
    });

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

// Get the current localization options
    var options = ((IApplicationBuilder)app).ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

    if (options?.Value != null)
    {
        // use the default localization
        app.UseRequestLocalization(options.Value);
    }

// Add the controller to the endpoint
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

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