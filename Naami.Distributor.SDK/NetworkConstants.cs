using Naami.SuiNet.Types;

namespace Naami.Distributor.SDK;

public record NetworkConstants(
    ObjectId SharePackageId,
    ObjectId DistributorPackageId,
    ObjectId VaultRegistry
);