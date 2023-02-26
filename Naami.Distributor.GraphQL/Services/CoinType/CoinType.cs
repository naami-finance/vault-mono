namespace Naami.Api.Services.CoinType;

public record CoinType(
    string Id,
    string Package,
    string Module,
    string StructName,
    byte Decimals,
    string Name,
    string Symbol,
    string Description,
    string? IconUrl);