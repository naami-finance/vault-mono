namespace Naami.Distributor.GraphQL.Services.Distribution;

public static class DistributionMapperExtensions
{
    public static Distribution ToServiceObject(this Naami.Distributor.Data.Distribution distribution)
        => new(distribution.Id, distribution.InitialAmount, distribution.RemainingAmount, distribution.CoinType);
}