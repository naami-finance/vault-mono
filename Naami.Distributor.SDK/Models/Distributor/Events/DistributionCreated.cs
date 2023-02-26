using Naami.SuiNet.Types;
using Naami.SuiNet.Types.Numerics;

namespace Naami.Distributor.SDK.Models.Distributor.Events;

public record DistributionCreated(byte[] CoinType, byte[] ShareType, ObjectId DistributionId, U64 Amount, U64 Timestamp);
public record DistributionClaimed();
public record DistributionCleanedUp();