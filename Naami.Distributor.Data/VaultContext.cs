using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

public class VaultContext : DbContext
{
    public DbSet<ShareType> ShareTypes { get; set; }

    public VaultContext() { }
    public VaultContext(DbContextOptions<VaultContext> contextOptions) : base(contextOptions){ }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Server=127.0.0.1;Port=5432;Database=vault;User Id=postgres;Password=secret;");
    }
}