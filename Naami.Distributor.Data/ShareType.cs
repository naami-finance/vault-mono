using Microsoft.EntityFrameworkCore;

namespace Naami.Distributor.Data;

[PrimaryKey(nameof(ObjectType))]
public class ShareType
{
    public string ObjectType { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public ulong TotalSupply { get; set; }

    public string RegistryObjectId { get; set; }
    public string MetadataObjectId { get; set; }
   
}