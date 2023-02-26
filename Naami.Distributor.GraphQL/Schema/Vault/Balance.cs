using Naami.Distributor.GraphQL.DataLoader;
using Naami.Distributor.GraphQL.Schema.CoinType;

namespace Naami.Distributor.GraphQL.Schema.Vault;

public record Balance(string CoinTypeIdentifier, long Amount)
{
    public async Task<CoinType.CoinType> CoinType(CoinTypeDataLoader dataLoader)
    {
        var type = await dataLoader.LoadAsync(CoinTypeIdentifier);
        return type.ToCoinType();
    }
}