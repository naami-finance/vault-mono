namespace Naami.Distributor.GraphQL.Services.Distribution;

public class DistributionMockQueryService : IDistributionQueryService
{
    public IEnumerable<Distribution> QueryDistributions(DistributionQueryOptions queryOptions)
    {
        return new List<Distribution>()
        {
            new("0000000000000000000000000000", 10, 10, "0x2::sui::SUI")
        }.AsQueryable();
    }
}