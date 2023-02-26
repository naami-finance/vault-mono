using Naami.Distributor.Data;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Event.Query;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.Extensions.ApiStreams;
using Naami.SuiNet.Types;
using CoinMetadata = Naami.Sdk.Objects.CoinMetadata;
using EventId = Naami.SuiNet.Types.EventId;

namespace Naami.Distributor.Indexer.Jobs;

public class CoinTypeIndexerJob
{
    private readonly IEventApi _eventApi;
    private readonly IReadApi _readApi;
    private readonly VaultContext _vaultContext;

    public CoinTypeIndexerJob(IEventApi eventApi, IReadApi readApi, VaultContext vaultContext)
    {
        _eventApi = eventApi;
        _readApi = readApi;
        _vaultContext = vaultContext;
    }

    public async Task RunAsync()
    {
        var metadataIds = new List<ObjectId>();

        var eventSnapshot = _vaultContext.EventSourcingSnapshots.FirstOrDefault() ?? new EventSourcingSnapshot();
        var lastIndexedEventId = eventSnapshot.CoinTypeEventSeq.HasValue &&
                                 !string.IsNullOrEmpty(eventSnapshot.CoinTypeTxDigest)
            ? new EventId(eventSnapshot.CoinTypeTxDigest, (long)eventSnapshot.CoinTypeEventSeq)
            : null;

        var eventStream = lastIndexedEventId == null
            ? _eventApi.GetEventStream(new EventTypeQuery(EventType.Publish), pageSize: 100)
            : _eventApi.GetEventStream(new EventTypeQuery(EventType.Publish), lastIndexedEventId, pageSize: 100);


        var lastEventId = new EventId(null, 0);
        await foreach (var suiEventEnvelopes in eventStream)
        {
            var eventsReponse = await _readApi.GetTransactions(suiEventEnvelopes
                .Where(x => x.Event.Publish!.Sender != "0x0000000000000000000000000000000000000000")
                .Select(x => x.TxDigest.Value).ToArray());

            metadataIds.AddRange(eventsReponse
                .SelectMany(x => x.Effects.Events)
                .Where(x => x.NewObject != null)
                .Where(x => x.NewObject.ObjectType.StartsWith("0x2::coin::CoinMetadata"))
                .Select(x => x.NewObject!.ObjectId));

            lastEventId = suiEventEnvelopes.Last().Id;
        }

        var coinTypes = new List<CoinType>();
        foreach (var objectIds in metadataIds.Chunk(100))
        {
            var objects = await _readApi.GetObjects<CoinMetadata>(objectIds);
            var metadataObjects =
                objects
                    .Where(x => x.ObjectStatus == ObjectStatus.Exists)
                    .Select(x => (
                        new SuiObjectType(x.ExistsResult!.Data.Type!),
                        x.ExistsResult!.Data.Fields!
                    ));

            coinTypes.AddRange(metadataObjects.Select(x => new CoinType
            {
                Decimals = x.Item2.Decimals,
                Description = x.Item2.Description,
                Name = x.Item2.Name,
                IconUrl = x.Item2.IconUrl.HasValue ? x.Item2.IconUrl.Value : string.Empty,
                Symbol = x.Item2.Symbol,
                ObjectType = x.Item1.Struct.GenericTypes[0],
                Module = x.Item1.Struct.GenericTypes[0].Module,
                Struct = x.Item1.Struct.GenericTypes[0].Struct,
                PackageId = x.Item1.Struct.GenericTypes[0].Package,
            }));
        }

        var existingTypes = _vaultContext.CoinTypes.Select(x => x.ObjectType).ToArray();
        var newTypes = coinTypes.Where(x => existingTypes.All(e => e != x.ObjectType));

        await _vaultContext.CoinTypes.AddRangeAsync(newTypes);
        
        if (_vaultContext.EventSourcingSnapshots.Any())
        {
            var snapshot = _vaultContext.EventSourcingSnapshots.Single();
            snapshot.CoinTypeEventSeq = (ulong)lastEventId.EventSeq;
            snapshot.CoinTypeTxDigest = lastEventId.TxDigest;
            _vaultContext.Update(snapshot);
        }
        else
        {
            var snapshot = new EventSourcingSnapshot()
            {
                CoinTypeEventSeq = (ulong)lastEventId.EventSeq,
                CoinTypeTxDigest = lastEventId.TxDigest
            };
            await _vaultContext.EventSourcingSnapshots.AddAsync(snapshot);
        }
        
        await _vaultContext.SaveChangesAsync();
    }
}