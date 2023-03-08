using Naami.Distributor.Data;

namespace Naami.Distributor.GraphQL.Services.Distribution;

public class DistributionQueryService : IDistributionQueryService
{
    private readonly VaultContext _vaultContext;

    public DistributionQueryService(VaultContext vaultContext)
    {
        _vaultContext = vaultContext;
    }

    public IEnumerable<Distribution> QueryDistributions(DistributionQueryOptions queryOptions)
    {
        var distributions = _vaultContext.Distributions.AsQueryable();

        if (!string.IsNullOrEmpty(queryOptions.CoinType))
            distributions = distributions.Where(x => x.CoinType == queryOptions.CoinType);

        if (!string.IsNullOrEmpty(queryOptions.ShareType))
            distributions = distributions.Where(x => x.ShareType == queryOptions.ShareType);

        if (queryOptions.EpochGt.HasValue)
            distributions = distributions.Where(x => x.CreatedAt < queryOptions.EpochGt.Value);

        if (queryOptions.EpochLt.HasValue)
            distributions = distributions.Where(x => x.CreatedAt > queryOptions.EpochLt.Value);

        return distributions.AsEnumerable().Select(DistributionMapperExtensions.ToServiceObject).ToArray();
    }
}