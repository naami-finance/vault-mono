using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

[PrimaryKey(nameof(ObjectType))]
public class ShareType : IHasEventId, IHasNextEntry
{
    public string ObjectType { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public ulong TotalSupply { get; set; }

    public string RegistryObjectId { get; set; }
    public string MetadataObjectId { get; set; }
    
    public string? TxDigest { get; set; }
    public ulong? EventSeq { get; set; }
    public string? NextTxDigest { get; set; }
    public ulong? NextEventSeq { get; set; }
}