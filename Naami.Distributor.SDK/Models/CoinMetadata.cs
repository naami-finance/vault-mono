using System.Runtime.Serialization;
using Naami.SuiNet.Extensions.ModuleTypes.Sui;
using Uid = Naami.SuiNet.SuiTypes.Uid;

namespace Naami.Sdk.Objects;

[DataContract]
public record CoinMetadata(
    [property: DataMember(Name = "id")] Uid Id,
    [property: DataMember(Name = "decimals")] byte Decimals,
    [property: DataMember(Name = "name")] string Name,
    [property: DataMember(Name = "symbol")] string Symbol,
    [property: DataMember(Name = "description")] string Description)
{
    [property: DataMember(Name = "icon_url")]
    public Url? IconUrl { get; set; }
}