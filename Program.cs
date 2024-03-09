using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DemoSeriLogDB");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq("http://localhost:5341/")
    .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions { TableName = "Log" }, null, null,
        LogEventLevel.Information, null, null, null, null)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger); // Configure Serilog at the host level 

// Add services to the container. 
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var cultures = new[]
        {
            new CultureInfo("ru-Ru"),
            new CultureInfo("en-US"),
            new CultureInfo("kk-Kz")
        };
        options.DefaultRequestCulture = new RequestCulture(culture: "ru-Ru", uiCulture: "ru-Ru");
        options.SupportedCultures= cultures;
        options.SupportedUICultures= cultures;
        {

        };
    });
builder.Services.Configure<CookieTempDataProviderOptions>(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Domain = "localhost:62882";
    options.Cookie.Expiration = TimeSpan.FromSeconds(160); 
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.Name = ".HotelATR.Session";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>

    {
        options.LoginPath = "/Home/Login";

    });

var app = builder.Build();

// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
var localOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localOptions.Value); 

app.UseAuthorization();
app.UseAuthentication();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();