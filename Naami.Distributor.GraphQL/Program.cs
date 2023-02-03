// See https://aka.ms/new-console-template for more information

using Naami.Distributor.GraphQL;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.JsonRpc;

var builder = WebApplication.CreateBuilder(args);
var configuration = LoadConfiguration();

builder.Services
    .AddSingleton<IJsonRpcClient>(new JsonRpcClient(configuration.RpcNodeUrl))
    .AddSingleton<IReadApi, ReadApi>()
    ;

var app = builder.Build();
app.MapGraphQL(PathString.Empty);
app.Run();

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