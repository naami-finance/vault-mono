using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

[PrimaryKey(nameof(ObjectType))]
public class CoinType
{
    public string ObjectType { get; set; }
    public string PackageId { get; set; }
    public string Module { get; set; }
    public string Struct { get; set; }
    public byte Decimals { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string IconUrl { get; set; }
    public string Description { get; set; }
}