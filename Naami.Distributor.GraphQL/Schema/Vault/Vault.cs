using Naami.Distributor.GraphQL.Services.Distribution;
using Naami.SuiNet.Apis.Governance;
using Naami.SuiNet.Apis.Read;

namespace Naami.Distributor.GraphQL.Schema.Vault;

public record Vault(string Name, string ShareType)
{
    public const byte MergingEpochDuration = 10;
    
    public Task<Balance[]> UserClaimableBalance()
    {
        return null;
    }

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
        [Service] IDistributionQueryService distributionQueryService,
        [Service] IGovernanceApi governanceApi
    )
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

    public Task<Balance[]> UserLockedBalance()
    {
        return null;
    }
}