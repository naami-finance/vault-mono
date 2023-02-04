namespace Naami.Distributor.GraphQL.Services.ShareRegistry;

public interface IShareRegistryQueryService
{
    public Task<ShareRegistry> GetShareRegistryById(string objectId);
    public Task<ShareRegistry> GetShareRegistryByShareType(string shareType);
}