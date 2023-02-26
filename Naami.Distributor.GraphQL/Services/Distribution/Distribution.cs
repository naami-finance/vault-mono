namespace Naami.Distributor.GraphQL.Services.Distribution;

public record Distribution(string Id, ulong InitialAmount, ulong RemainingAmount, string CoinType);