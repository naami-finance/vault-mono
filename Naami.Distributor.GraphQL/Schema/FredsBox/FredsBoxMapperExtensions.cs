namespace Naami.Distributor.GraphQL.Schema.Vault;

public static class FredsBoxMapperExtensions
{
    public static FredsBox ToObjectType(this Services.Vault.Vault vault)
        => new(vault.Id, vault.Balance.ToString(), vault.ShareType, vault.CoinType);
}