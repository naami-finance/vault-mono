using Naami.Distributor.GraphQL.Schema.Vault;
using Naami.Distributor.GraphQL.Services.Vault;
using Vault = Naami.Distributor.GraphQL.Schema.Vault;

namespace Naami.Distributor.GraphQL;

public class Query
{
    public async Task<Vault.FredsBox> GetVault([Service] IVaultQueryService vaultQueryService, string id)
    {
        var vault = await vaultQueryService.GetVaultById(id);
        return vault.ToObjectType();
    }

    public async Task<Vault.FredsBox[]> GetVaults([Service] IVaultQueryService vaultQueryService, string shareType)
    {
        var vaults = await vaultQueryService.GetVaultsForShareType(shareType);
        return vaults.Select(x => x.ToObjectType()).ToArray();
    }
}