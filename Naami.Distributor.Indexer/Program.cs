﻿// See https://aka.ms/new-console-template for more information

using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Naami.Distributor.Data;
using Naami.Distributor.Indexer;
using Naami.Distributor.Indexer.Jobs;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.JsonRpc;


var configuration = LoadConfiguration();
var hangfireConnectionString =
    $"Server={configuration.PostgreHost};" +
    $"Port={configuration.PostgrePort};" +
    $"Database={configuration.HangfireDatabase};" +
    $"User Id={configuration.PostgreUsername};" +
    $"Password={configuration.PostgrePassword};";

var efConnectionString =
    $"Server={configuration.PostgreHost};" +
    $"Port={configuration.PostgrePort};" +
    $"Database={configuration.IndexingDatabase};" +
    $"User Id={configuration.PostgreUsername};" +
    $"Password={configuration.PostgrePassword};";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews();

builder.Services
    .AddSingleton(configuration)
    .AddSingleton<IJsonRpcClient, JsonRpcClient>(_ => new JsonRpcClient(configuration.FullNodeRpcUrl))
    .AddTransient<IEventApi, EventApi>()
    .AddTransient<IReadApi, ReadApi>();

builder.Services
    .AddHangfire(c =>
        c.UsePostgreSqlStorage(hangfireConnectionString)
    )
    .AddHangfireServer();

builder.Services.AddDbContext<VaultContext>(
    o => o.UseNpgsql(efConnectionString)
);



var app = builder.Build();

// update db schema (put to cicd later)
var ctx = app.Services.GetService<VaultContext>();
await ctx!.Database.MigrateAsync();

app.UseStaticFiles();
app.UseHangfireDashboard();

app.UseRouting();
app.UseEndpoints(e =>
{
    e.MapControllers();
    e.MapHangfireDashboard();
});


RecurringJob.AddOrUpdate<IndexSharesJob>("Index Shares", job => job.RunAsync(), Cron.Hourly());
RecurringJob.AddOrUpdate<IndexDistributionsJob>("Index Distributions", job => job.RunAsync(), Cron.Hourly());
RecurringJob.AddOrUpdate<ClearIndexedDataJob>("Clear Indexed Data", job => job.RunAsync(), Cron.Yearly());
RecurringJob.AddOrUpdate<AddSuiCoinJob>(
    "Add SUI Coin",
    job => job.RunAsync(),
    "0 0 1 1 *"
);
RecurringJob.AddOrUpdate<CoinTypeIndexerJob>(
    "Index Coin Types",
    job => job.RunAsync(),
    "30 */12 * * *"
);

app.Run("http://*:80");

ApplicationConfiguration LoadConfiguration()
{
    var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
    var builder = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json")
        .AddJsonFile($"appsettings.{currentEnvironment}.json", true)
        .AddEnvironmentVariables();

    var configurationRoot = builder.Build();
    var configuration = new ApplicationConfiguration();
    configurationRoot.Bind(configuration);

    return configuration;
}