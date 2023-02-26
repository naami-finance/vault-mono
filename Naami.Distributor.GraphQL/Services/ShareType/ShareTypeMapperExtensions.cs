namespace Naami.Distributor.GraphQL.Services.ShareType;

public static class ShareTypeMapperExtensions
{
    public static ShareType ToServiceObject(this Data.ShareType shareType)
        => new(shareType.ObjectType, shareType.Name);
}