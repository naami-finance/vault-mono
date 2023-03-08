using Naami.Distributor.GraphQL.Services.CoinType;

namespace Naami.Distributor.GraphQL.DataLoader;

public class CoinTypeDataLoader : BatchDataLoader<string, CoinType>
{
    private readonly ICoinTypeQueryService _coinTypeQueryService;

    public CoinTypeDataLoader(IBatchScheduler batchScheduler, ICoinTypeQueryService coinTypeQueryService,
        DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _coinTypeQueryService = coinTypeQueryService;
    }

    protected override async Task<IReadOnlyDictionary<string, CoinType>> LoadBatchAsync(IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var coins = _coinTypeQueryService.GetCoinTypesByIds(keys.ToArray());
        return coins.ToDictionary(x => x.Id.ToString());
    }
}