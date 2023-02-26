using Naami.Distributor.Data;

namespace Naami.Distributor.GraphQL.Services.ShareType;

public class ShareTypeQueryService : IShareTypeQueryService
{
    private readonly VaultContext _vaultContext;

    public ShareTypeQueryService(VaultContext vaultContext)
    {
        _vaultContext = vaultContext;
    }

    public IQueryable<ShareType> QueryShareTypes()
    {
        return _vaultContext.ShareTypes.AsQueryable().Select(x => x.ToServiceObject());
    }
}