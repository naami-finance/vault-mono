using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

public class VaultContext : DbContext
{
    public DbSet<ShareType> ShareTypes { get; set; }
    public DbSet<Distribution> Distributions { get; set; }
    public DbSet<EventSourcingSnapshot> EventSourcingSnapshots { get; set; }
    public DbSet<CoinType> CoinTypes { get; set; }

    public VaultContext() { }
    public VaultContext(DbContextOptions<VaultContext> contextOptions) : base(contextOptions){ }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Server=127.0.0.1;Port=5432;Database=vault;User Id=postgres;Password=mysecretpassword;");
    }
}