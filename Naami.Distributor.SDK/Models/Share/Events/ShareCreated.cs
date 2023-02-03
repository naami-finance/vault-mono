using Naami.SuiNet.Types;
using Naami.SuiNet.Types.Numerics;

namespace Naami.Distributor.SDK.Models.Share.Events;

public record ShareCreated(
    byte[] Type,
    byte[] Symbol,
    byte[] Name,
    U64 TotalSupply,
    ObjectId RegistryId,
    ObjectId MetadataId
);