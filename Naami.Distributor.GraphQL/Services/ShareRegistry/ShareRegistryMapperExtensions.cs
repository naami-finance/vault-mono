using Naami.SuiNet.Types;

namespace Naami.Distributor.GraphQL.Services.ShareRegistry;

public static class ShareRegistryMapperExtensions
{
    public static ShareRegistry ToServiceObject(
        this (SuiObjectType coinType, SuiObjectType shareType, SDK.Models.Share.ShareRegistry shareRegistry)
            extendedRegistry) => new(
        extendedRegistry.shareRegistry.Id.Id,
        ulong.Parse(extendedRegistry.shareRegistry.TotalSupply),
        extendedRegistry.coinType,
        extendedRegistry.shareType
    );
}