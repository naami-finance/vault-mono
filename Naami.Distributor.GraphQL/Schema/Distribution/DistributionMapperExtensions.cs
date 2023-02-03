namespace Naami.Distributor.GraphQL.Schema.Distribution;

public static class DistributionMapperExtensions
{
    public static Distribution ToObjectType(this Services.Distribution.Distribution distribution)
        => new(
            distribution.Id,
            distribution.Balance.ToString(),
            distribution.CoinType,
            distribution.ShareType
        );
}