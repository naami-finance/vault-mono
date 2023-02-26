namespace Naami.Distributor.GraphQL.Schema.CoinType;

public record CoinType(string Type, string Name, string Symbol, byte Decimals, string? IconUrl);