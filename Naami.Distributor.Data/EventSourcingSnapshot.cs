using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

[PrimaryKey(nameof(Id))]
public class EventSourcingSnapshot
{
    public long Id { get; set; }
    
    public string? ShareTypeTxDigest { get; set; }
    public ulong? ShareTypeEventSeq { get; set; }
    
    public string? DistributionTxDigest { get; set; }
    public ulong? DistributionEventSeq { get; set; }
    
    public string? CoinTypeTxDigest { get; set; }
    public ulong? CoinTypeEventSeq { get; set; }
}