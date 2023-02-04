namespace Naami.Distributor.Data;

public interface IHasNextEntry
{
    public string? NextTxDigest { get; set; }
    public ulong? NextEventSeq { get; set; }
}