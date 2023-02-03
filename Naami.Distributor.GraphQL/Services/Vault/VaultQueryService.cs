using Naami.Distributor.SDK.QueryExtensions;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.Types;

namespace Naami.Distributor.GraphQL.Services.Vault;

public class VaultQueryService : IVaultQueryService
{
    private readonly IReadApi _readApi;

    public VaultQueryService(IReadApi readApi)
    {
        _readApi = readApi;
    }

    public async Task<Vault> GetVaultById(string objectId)
    {
        var readResult = await _readApi.GetObject<SDK.Models.Vault.Vault>(objectId);

        // opt: throw custom error if object is not available

        var type = new SuiObjectType(readResult.ExistsResult!.Data.Type!);
        var vaultObject = readResult.ExistsResult!.Data.Fields!;

        return (
            // TCoin
            type.Struct.GenericTypes[0],
            
            // TShare
            type.Struct.GenericTypes[1],
            
            vaultObject
        ).ToServiceObject();
    }

    public async Task<Vault[]> GetVaultsByIds(string[] objectIds)
    {
        // TODO: move to batch-requests as soon as SDK supports them (silly SDK devs. :]), 
        
        var results = objectIds.Select(GetVaultById).ToArray();
        await Task.WhenAll(results);
        return results.Select(x => x.Result).ToArray();
    }

    public async Task<Vault[]> GetVaultsForShareType(string shareType)
    {
        // TODO: this is where indexing becomes relevant!

        var vaults = new List<Vault>();

        await foreach (var wrappedVaults in _readApi.VaultsStream())
        {
            foreach (var wrappedVault in wrappedVaults)
            {
                if (wrappedVault.shareType == shareType)
                    vaults.Add(wrappedVault.ToServiceObject());
            }
        }

        return vaults.ToArray();
    }
}