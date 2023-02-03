using Naami.Distributor.SDK.Models.Vault;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.Extensions.ApiStreams;
using Naami.SuiNet.Types;

namespace Naami.Distributor.SDK.QueryExtensions;

public static class VaultQueryExtensions
{
    public static async IAsyncEnumerable<(SuiObjectType coinType, SuiObjectType shareType, Vault vault)[]> VaultsStream(
        this IReadApi readApi,
        ObjectId? cursor = null,
        int pageSize = 100
    )
    {
        var asyncEnumerable = readApi.GetDynamicFieldsStream(
            Constants.NetworkConstants.VaultRegistry,
            cursor,
            pageSize
        );

        await foreach (var fieldBatch in asyncEnumerable)
        {
            // we should do an objectType comparison here (we know there are only vaults stored as dof, but just to make sure)
            var vaultObjectIds = fieldBatch.Select(x => x.ObjectId);
            var objectLoadTask = vaultObjectIds.Select(readApi.GetObject<Vault>).ToArray();
            await Task.WhenAll(objectLoadTask);

            yield return objectLoadTask.Select(x =>
            {
                var type = new SuiObjectType(x.Result.ExistsResult!.Data.Type!);

                return (
                    type.Struct.GenericTypes[0],
                    type.Struct.GenericTypes[1],
                    x.Result.ExistsResult!.Data.Fields!
                );
            }).ToArray();
        }
    }
}