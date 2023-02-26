namespace Naami.Distributor.GraphQL.Services.Distribution;

public record DistributionQueryOptions(
    string? ShareType = null,
    string? CoinType = null,
    ulong? EpochGt = null,
    ulong? EpochLt = null
);