using Naami.SuiNet.Extensions.ModuleTypes.Sui;
using Naami.SuiNet.Types.Numerics;
using Balance = Naami.SuiNet.SuiTypes.Balance;

namespace Naami.Distributor.SDK.Models.Distributor;

public record Distribution(Uid Id, Balance Balance, U64 Timestamp, U64 DecayPeriod);