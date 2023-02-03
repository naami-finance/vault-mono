namespace Naami.Distributor.GraphQL.Schema.ShareRegistry;

public static class ShareRegistryMapperExtensions
{
    public static ShareRegistry ToObjectType(this Services.ShareRegistry.ShareRegistry shareRegistry)
        => new(
            shareRegistry.Id,
            shareRegistry.TotalSupply.ToString(),
            shareRegistry.ShareType,
            shareRegistry.CoinType
        );
}