namespace Naami.Distributor.Data;

public interface IHasEventId
{
    public string? TxDigest { get; set; }
    public ulong? EventSeq { get; set; }
}