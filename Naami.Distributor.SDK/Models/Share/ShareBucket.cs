using Naami.SuiNet.Extensions.ModuleTypes.Sui;
using Naami.SuiNet.Types.Numerics;

namespace Naami.Distributor.SDK.Models.Share;

public record ShareBucket(Uid Id, U64 Shares, U64 LastModification);