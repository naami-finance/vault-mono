namespace Naami.Distributor.GraphQL.Services.Vault;

public record Vault(string Id, ulong Balance, string ShareType, string CoinType);