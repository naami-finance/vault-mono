using Naami.Distributor.GraphQL.Services.ShareType;

namespace Naami.Distributor.GraphQL.Schema.Vault;

public static class VaultMapperExtensions
{
    public static Vault ToVaultObjectType(this ShareType shareType) => new(shareType.Name, shareType.ObjectType);
}