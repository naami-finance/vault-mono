namespace Naami.Distributor.GraphQL.Schema.CoinType;

public static class CoinTypeMapperExtensions
{
    public static CoinType ToCoinType(this Services.CoinType.CoinType coinType)
        => new($"{coinType.Package}::{coinType.Module}::{coinType.StructName}",
            coinType.Name, coinType.Symbol, coinType.Decimals, coinType.IconUrl);
}