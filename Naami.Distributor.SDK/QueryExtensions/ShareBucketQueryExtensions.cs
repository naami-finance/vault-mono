using Naami.Distributor.SDK.Models.Share;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.Types;

namespace Naami.Distributor.SDK.QueryExtensions;

public static class ShareBucketQueryExtensions
{
    public static async Task<(SuiObjectType ShareType, ShareBucket Bucket)[]> GetOwnedShares(
        this IReadApi readApi,
        SuiAddress owner
    )
    {
        var objects = await readApi.GetObjectsOwnedByAddress(owner);
        var shareIds = objects.Where(x =>
            x.Type.Package == Constants.NetworkConstants.SharePackageId &&
            x.Type.Module == Constants.ModuleConstants.ShareBucketModule &&
            x.Type.Struct.Name == Constants.ModuleConstants.ShareBucketStruct
        ).Select(x => x.ObjectId);

        var objectTasks = shareIds.Select(readApi.GetObject<ShareBucket>).ToArray();
        await Task.WhenAll(objectTasks);

        return objectTasks.Select(x =>
        {
            var type = new SuiObjectType(x.Result.ExistsResult!.Data.Type!).Struct.GenericTypes.First();
            var bucket = x.Result.ExistsResult.Data.Fields!;
            return (type, bucket);
        }).ToArray();
    }
}