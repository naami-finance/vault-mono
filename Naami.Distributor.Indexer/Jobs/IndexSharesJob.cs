using System.Text;
using Naami.Distributor.Data;
using Naami.Distributor.SDK.Models.Share.Events;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Event.Query;
using Naami.SuiNet.Extensions.ApiStreams;
using ServiceStack;

namespace Naami.Distributor.Indexer.Jobs;

public class IndexSharesJob
{
    private readonly IEventApi _eventApi;
    private readonly VaultContext _vaultContext;

    public IndexSharesJob(IEventApi eventApi, VaultContext vaultContext)
    {
        _eventApi = eventApi;
        _vaultContext = vaultContext;
    }

    public async Task RunAsync()
    {
        // TODO: application configuration
        const string EventType = "0x6a16517208e903f86a3e52a56a336865994e3359::registry::ShareCreated";

        var lastEventId = (IHasNextEntry?)_vaultContext.ShareTypes
            .FirstOrDefault(x => x.NextEventSeq.HasValue && !string.IsNullOrEmpty(x.NextTxDigest));

        var eventsBatchStream = lastEventId == null
            ? _eventApi.GetEventStream(new MoveEventEventQuery(EventType))
            : _eventApi.GetEventStream(new MoveEventEventQuery(EventType), new SuiNet.Types.EventId(
                lastEventId.NextTxDigest!,
                (long)lastEventId.NextEventSeq!.Value)
            );


        await foreach (var eventsBatch in eventsBatchStream)
        {
            var shareTypes = eventsBatch.Select((ev, eventIndex) =>
            {
                var nextId = eventIndex + 1 >= eventsBatch.Length
                    ? null
                    : eventsBatch[eventIndex + 1].Id;

                var shareCreatedEvent = ev.Event.MoveEvent!.Fields.FromObjectDictionary<ShareCreated>();

                return new ShareType
                {
                    Name = Encoding.ASCII.GetString(shareCreatedEvent.Name),
                    Symbol = Encoding.ASCII.GetString(shareCreatedEvent.Symbol),
                    EventSeq = (ulong)ev.Id.EventSeq,
                    TotalSupply = shareCreatedEvent.TotalSupply,
                    TxDigest = ev.Id.TxDigest,
                    MetadataObjectId = shareCreatedEvent.MetadataId,
                    RegistryObjectId = shareCreatedEvent.RegistryId,
                    ObjectType = Encoding.ASCII.GetString(shareCreatedEvent.Type),
                    NextEventSeq = (ulong?)nextId?.EventSeq,
                    NextTxDigest = nextId?.TxDigest
                };
            }).Where(x => _vaultContext.ShareTypes.All(s => s.ObjectType != x.ObjectType));

            // TODO Update Next Cursor for socket-indexed entities

            await _vaultContext.ShareTypes.AddRangeAsync(shareTypes.ToArray());
            await _vaultContext.SaveChangesAsync();
        }
    }
}