namespace Naami.Distributor.GraphQL.Services.ShareType;

public class ShareTypeMockQueryService : IShareTypeQueryService
{
    public IQueryable<ShareType> QueryShareTypes()
    {
        return new List<ShareType>()
        {
            new("0x0::mock::MOCK", "Mock Share"),
            new("0x0::tom::TOM", "Toms Share"),
        }.AsQueryable();
    }
}