// See https://aka.ms/new-console-template for more information

using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Naami.Distributor.Data;

Console.WriteLine("Hello, World!");

var hangfireConnectionString =
    "Server=127.0.0.1;Port=5432;Database=hangfire_distributor;User Id=postgres;Password=secret;";
var efConnectionString =
    "Server=127.0.0.1;Port=5432;Database=vault;User Id=postgres;Password=secret;";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews();

builder.Services
    .AddHangfire(c => c.UsePostgreSqlStorage(hangfireConnectionString))
    .AddHangfireServer();

builder.Services.AddDbContext<VaultContext>(
    o => o.UseNpgsql(efConnectionString)
);


var app = builder.Build();

app.UseStaticFiles();
app.UseHangfireDashboard();

app.UseRouting();
app.UseEndpoints(e =>
{
    e.MapControllers();
    e.MapHangfireDashboard();
});

app.Run();