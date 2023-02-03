using Naami.SuiNet.Extensions.ModuleTypes.Sui;
using Naami.SuiNet.Types.Numerics;

namespace Naami.Distributor.SDK.Models.Share;

public record ShareRegistry(Uid Id, U64 TotalSupply);