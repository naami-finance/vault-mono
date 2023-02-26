namespace Naami.Api.Services.CoinType;

public static class CoinTypeMapperExtensions
{
    public static CoinType ToCoinType(this Distributor.Data.CoinType coinType)
        => new(coinType.ObjectType, coinType.PackageId, coinType.Module, coinType.Struct, coinType.Decimals,
            coinType.Name, coinType.Symbol, coinType.Description, coinType.IconUrl);
}