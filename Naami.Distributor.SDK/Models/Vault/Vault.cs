using Naami.SuiNet.SuiTypes;
using Balance = Naami.SuiNet.Types.Balance;

namespace Naami.Distributor.SDK.Models.Vault;

public record Vault(Uid Id, SuiNet.Extensions.ModuleTypes.Sui.Balance Balance);
