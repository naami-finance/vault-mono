using Naami.SuiNet.Types;

namespace Naami.Distributor.GraphQL.Services.Vault;

public static class VaultMapperExtensions
{
    public static Vault ToServiceObject(
        this (SuiObjectType coinType, SuiObjectType shareType, SDK.Models.Vault.Vault vault) extendedVault
    ) => new(
        extendedVault.vault.Id.Id,
        ulong.Parse(extendedVault.vault.Balance),
        extendedVault.coinType,
        extendedVault.shareType
    );
}