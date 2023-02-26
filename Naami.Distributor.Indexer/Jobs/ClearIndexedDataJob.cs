using Microsoft.EntityFrameworkCore;
using Naami.Distributor.Data;

namespace Naami.Distributor.Indexer.Jobs;

public class ClearIndexedDataJob
{
    private readonly VaultContext _vaultContext;

    public ClearIndexedDataJob(VaultContext vaultContext)
    {
        _vaultContext = vaultContext;
    }

    public async Task RunAsync()
    {
        await _vaultContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(_vaultContext.ShareTypes)}\"");
        await _vaultContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(_vaultContext.EventSourcingSnapshots)}\"");
        await _vaultContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{nameof(_vaultContext.Distributions)}\" CASCADE");
    }
}