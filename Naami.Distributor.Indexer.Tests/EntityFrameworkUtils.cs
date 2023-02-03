using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Naami.Distributor.Indexer.Tests;

public static class EntityFrameworkUtils
{
    public static DbSet<T> AsFakeDbSet<T>(this IEnumerable<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        var fakeDbSet = Substitute.For<DbSet<T>, IQueryable<T>>();
        ((IQueryable<T>)fakeDbSet).Provider.Returns(queryableData.Provider);
        ((IQueryable<T>)fakeDbSet).Expression.Returns(queryableData.Expression);
        ((IQueryable<T>)fakeDbSet).ElementType.Returns(queryableData.ElementType);
        ((IQueryable<T>)fakeDbSet).GetEnumerator().Returns(queryableData.GetEnumerator());

        // fakeDbSet.AsNoTracking().Returns(fakeDbSet);

        return fakeDbSet;
    }

    public static List<T> AsList<T>(this T obj) => new List<T> {obj};
}