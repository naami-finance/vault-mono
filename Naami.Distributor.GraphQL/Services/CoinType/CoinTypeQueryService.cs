using Microsoft.EntityFrameworkCore;
using Naami.Distributor.Data;
using Naami.SuiNet.Apis.CoinRead;

namespace Naami.Distributor.GraphQL.Services.CoinType;

public class CoinTypeQueryService : ICoinTypeQueryService, IAsyncDisposable
{
    private readonly VaultContext _vaultContext;

    public CoinTypeQueryService(VaultContext vaultContext)
    {
        _vaultContext = vaultContext;
    }

    public IEnumerable<CoinType> GetCoinTypesByIds(string[] ids)
    {
        return _vaultContext.CoinTypes.AsEnumerable()
            .Where(x => ids.Contains(x.ObjectType))
            .Select(x => x.ToCoinType());
    }

    public ValueTask DisposeAsync()
    {
        return _vaultContext.DisposeAsync();
    }
}