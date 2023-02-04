
using Naami.Distributor.GraphQL.Schema.Vault;
using Naami.Distributor.GraphQL.Services.ShareType;

namespace Naami.Distributor.GraphQL;

public class Query
{
    [UsePaging]
    public IQueryable<Vault> GetVaults([Service] IShareTypeQueryService shareQueryService)
        => shareQueryService.QueryShareTypes().Select(x => x.ToVaultObjectType());
}