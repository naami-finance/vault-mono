using Naami.Distributor.Data;
using Naami.Distributor.SDK;

namespace Naami.Distributor.Indexer.Jobs;

public class AddSuiCoinJob
{
    private readonly VaultContext _vaultContext;

    public AddSuiCoinJob(VaultContext vaultContext)
    {
        _vaultContext = vaultContext;
    }

    public async Task RunAsync()
    {
        if (_vaultContext.CoinTypes.Any(x => x.ObjectType == $"{SuiConstants.SuiCoinIdentifier}"))
            return;

        await _vaultContext.CoinTypes.AddAsync(new CoinType
        {
            Decimals = SuiConstants.SuiDecimals,
            Struct = SuiConstants.SuiStruct,
            Module = SuiConstants.SuiModule,
            PackageId = SuiConstants.SuiPackageId.AsFormattedAddress(),
            ObjectType = SuiConstants.SuiCoinIdentifier,
            
            Description = "Official SUI Network token",
            Name = "Sui",
            IconUrl = "",
            Symbol = "SUI"
        });

        await _vaultContext.SaveChangesAsync();
    }
}