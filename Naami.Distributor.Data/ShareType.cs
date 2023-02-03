using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

[PrimaryKey(nameof(ObjectType))]
public class ShareType : IHasEventId, IHasNextEntry
{
    // public long Id { get; set; }
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

public interface IHasEventId
{
    public string? TxDigest { get; set; }
    public ulong? EventSeq { get; set; }
}

public interface IHasNextEntry
{
    public string? NextTxDigest { get; set; }
    public ulong? NextEventSeq { get; set; }
}

public class VaultContext : DbContext
{
    public DbSet<ShareType> ShareTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Server=127.0.0.1;Port=5432;Database=vault;User Id=postgres;Password=secret;");
    }
}