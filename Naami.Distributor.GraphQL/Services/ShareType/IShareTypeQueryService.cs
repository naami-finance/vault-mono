using Naami.Distributor.Data;

namespace Naami.Distributor.GraphQL.Services.ShareType;

public interface IShareTypeQueryService
{
    public IQueryable<ShareType> QueryShareTypes();
}

public class ShareTypeQueryService : IShareTypeQueryService
{
    private readonly VaultContext _vaultContext;

    public ShareTypeQueryService(VaultContext vaultContext)
    {
        _vaultContext = vaultContext;
    }

    public IQueryable<ShareType> QueryShareTypes()
    {
        return _vaultContext.ShareTypes.AsQueryable().Select(x => x.ToServiceObject());
    }
}

public static class ShareTypeMapperExtensions
{
    public static ShareType ToServiceObject(this Data.ShareType shareType)
        => new(shareType.ObjectType, shareType.Name);
}

public record ShareType(string ObjectType, string Name);