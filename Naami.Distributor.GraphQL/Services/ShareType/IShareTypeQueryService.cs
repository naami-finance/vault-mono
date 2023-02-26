namespace Naami.Distributor.GraphQL.Services.ShareType;

public interface IShareTypeQueryService
{
    public IQueryable<ShareType> QueryShareTypes();
}