namespace Naami.Distributor.GraphQL.Services.CoinType;

public class CoinTypeMockQueryService : ICoinTypeQueryService
{
    public IEnumerable<CoinType> GetCoinTypesByIds(string[] id)
    {
        return new List<CoinType>
        {
            new("0x2::sui::SUI", "0x2", "sui", "SUI", 6, "SUI", "SUI", "SUI Coin", string.Empty),
            new("0x2::btc::BTC", "0x2", "btc", "BTC", 6, "Bitcoin", "BTC", "wrapped bitcoin", string.Empty),
        }.AsQueryable();
    }
}