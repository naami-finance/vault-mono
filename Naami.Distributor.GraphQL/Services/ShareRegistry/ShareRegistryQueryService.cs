using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.Types;

namespace Naami.Distributor.GraphQL.Services.ShareRegistry;

public class ShareRegistryQueryService : IShareRegistryQueryService
{
    private readonly IReadApi _readApi;

    public ShareRegistryQueryService(IReadApi readApi)
    {
        _readApi = readApi;
    }

    public async Task<ShareRegistry> GetShareRegistryById(string objectId)
    {
        var readResult = await _readApi.GetObject<SDK.Models.Share.ShareRegistry>(objectId);

        // opt: throw custom error if object is not available

        var type = new SuiObjectType(readResult.ExistsResult!.Data.Type!);
        var shareRegistryObject = readResult.ExistsResult!.Data.Fields!;

        return (
            // TCoin
            type.Struct.GenericTypes[0],
            
            // TShare
            type.Struct.GenericTypes[1],
            
            shareRegistryObject
        ).ToServiceObject();
    }

    public Task<ShareRegistry> GetShareRegistryByShareType(string shareType)
    {
        // requires indexed ShareRegistries (like there is no "globally" easily accessable storage on-chain
        throw new NotImplementedException();
    }
}