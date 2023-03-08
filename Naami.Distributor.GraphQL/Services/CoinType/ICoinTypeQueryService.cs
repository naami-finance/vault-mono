namespace Naami.Distributor.GraphQL.Services.CoinType;

public interface ICoinTypeQueryService
{
    public IEnumerable<CoinType> GetCoinTypesByIds(string[] id);
}