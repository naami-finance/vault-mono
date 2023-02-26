namespace Naami.Distributor.GraphQL.Services.Distribution;

public interface IDistributionQueryService
{
    public IEnumerable<Distribution> QueryDistributions(DistributionQueryOptions queryOptions);
}