namespace Naami.Distributor.GraphQL.Services.Aggregator;

public record Aggregator(string Id, string ShareType, string CoinType, ulong Balance);

public interface IAggregatorQueryService
{
    public IEnumerable<Aggregator> QueryAggregators(AggregatorQueryOptions queryOptions);
}

public record AggregatorQueryOptions(
    string? ShareType = null,
    string? CoinType = null
);

public class AggregatorMockQueryService : IAggregatorQueryService
{
    public IEnumerable<Aggregator> QueryAggregators(AggregatorQueryOptions queryOptions)
    {
        return new Aggregator[]
        {
            new("0x1234", "0x0::mock::MOCK", "0x2::sui::SUI", 51451),
            new("0x2234", "0x0::mock::MOCK", "0x2::btc::BTC", 591)
        };
    }
}