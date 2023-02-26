namespace Naami.Distributor.GraphQL.Services.CoinType;

public interface ICoinTypeQueryService
{
    public IEnumerable<Api.Services.CoinType.CoinType> GetCoinTypesByIds(string[] id);
}