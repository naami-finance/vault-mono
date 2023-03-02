namespace Naami.Distributor.GraphQL.Services.CoinType;

public class CoinTypeMockQueryService : ICoinTypeQueryService
{
    public IEnumerable<Api.Services.CoinType.CoinType> GetCoinTypesByIds(string[] id)
    {
        return new List<Api.Services.CoinType.CoinType>
        {
            new("0x2::sui::SUI", "0x2", "sui", "SUI", 6, "SUI", "SUI", "SUI COin", string.Empty)
        }.AsQueryable();
    }
}