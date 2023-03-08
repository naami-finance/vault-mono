using Naami.Distributor.GraphQL.Services.Aggregator;
using Naami.Distributor.GraphQL.Services.Distribution;
using Naami.SuiNet.Apis.Governance;

namespace Naami.Distributor.GraphQL.Schema.Vault;

public record Vault(string Name, string ShareType)
{
    public Task<Balance[]> UserClaimableBalance(
        [Service] IDistributionQueryService distributionQueryService,
        [Service] IGovernanceApi governanceApi
    ) => TotalClaimableBalance(distributionQueryService, governanceApi);

    public async Task<Balance[]> TotalClaimableBalance(
        [Service] IDistributionQueryService distributionQueryService,
        [Service] IGovernanceApi governanceApi)
    {
        var currentEpoch = (await governanceApi.GetSuiSystemState()).Epoch;
        var distributions = distributionQueryService.QueryDistributions(
            new DistributionQueryOptions(ShareType)
        );

        // TODO: can we somehow make use of ulong here
        return distributions.GroupBy(x => x.CoinType)
            .Select(g => new Balance(g.Key, g.Sum(x => (long)x.RemainingAmount)))
            .ToArray();
    }

    public async Task<Balance[]> TotalLockedBalance(
        [Service] IAggregatorQueryService aggregatorQueryService,
        [Service] IGovernanceApi governanceApi
    )
    {
        await Task.CompletedTask;

        var aggregators = aggregatorQueryService.QueryAggregators(
            new AggregatorQueryOptions(ShareType)
        );

        // TODO: can we somehow make use of ulong here
        return aggregators.GroupBy(x => x.CoinType)
            .Select(g => new Balance(g.Key, g.Sum(x => (long)x.Balance)))
            .ToArray();
    }
}