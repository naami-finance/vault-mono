namespace Naami.Distributor.GraphQL.Services.Vault;

public interface IVaultQueryService
{
    public Task<Vault> GetVaultById(string objectId);
    public Task<Vault[]> GetVaultsByIds(string[] objectIds);
    public Task<Vault[]> GetVaultsForShareType(string shareType);
}