namespace Naami.Distributor.GraphQL.Services.Distribution;

public record Distribution(string Id, ulong Balance, string ShareType, string CoinType);

// Indexing is key here as well as there is no way to query all Distributions on-chain (easily)
// unless you want to event-source everything everytime the service is being start-up

public record DistributionQueryOptions(string? ShareType = null, string? CoinType = null);

public interface IDistributionQueryService
{
    public Task<Distribution[]> QueryDistributions(DistributionQueryOptions queryOptions);
}