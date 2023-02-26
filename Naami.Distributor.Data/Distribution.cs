using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

[PrimaryKey(nameof(Id))]
public class Distribution
{
    public string Id { get; set; }
    public string ShareType { get; set; }
    public string CoinType { get; set; }
    public ulong CreatedAt { get; set; }
    public ulong InitialAmount { get; set; }
    public ulong RemainingAmount { get; set; }
}