// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Naami.Distributor.Data;
using Naami.Distributor.GraphQL;
using Naami.Distributor.GraphQL.Services.CoinType;
using Naami.Distributor.GraphQL.Services.Distribution;
using Naami.Distributor.GraphQL.Services.ShareType;
using Naami.SuiNet.Apis.Governance;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.JsonRpc;

var builder = WebApplication.CreateBuilder(args);
var configuration = LoadConfiguration();

builder.Services
    .AddSingleton<IJsonRpcClient>(new JsonRpcClient(configuration.RpcNodeUrl))
    .AddSingleton<IReadApi, ReadApi>()
    .AddSingleton<IGovernanceApi, GovernanceApi>()
    .AddTransient<IShareTypeQueryService, ShareTypeMockQueryService>()
    .AddTransient<IDistributionQueryService, DistributionMockQueryService>()
    .AddTransient<ICoinTypeQueryService, CoinTypeMockQueryService>()
    ;

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var efConnectionString =
    $"Server={configuration.PostgreHost};" +
    $"Port={configuration.PostgrePort};" +
    $"Database={configuration.Database};" +
    $"User Id={configuration.PostgreUsername};" +
    $"Password={configuration.PostgrePassword};";

builder.Services.AddDbContext<VaultContext>(
    o => o.UseNpgsql(efConnectionString)
);

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