namespace Naami.Distributor.GraphQL.Schema.Vault;

public record ShareType;

public record ShareBucket(string Id, string ShareObjectType, string Shares);


public record Distribution;

public record CoinType;

// public record Coin;

public record FredsBox(string Id, string Balance, string ShareObjectType, string CoinType)
{
    
}