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
    private readonly ApplicationConfiguration _applicationConfiguration;

    public IndexSharesJob(IEventApi eventApi, VaultContext vaultContext,
        ApplicationConfiguration applicationConfiguration)
    {
        _eventApi = eventApi;
        _vaultContext = vaultContext;
        _applicationConfiguration = applicationConfiguration;
    }

    public async Task RunAsync()
    {
        var lastIndexedShareType = _vaultContext.ShareTypes
            .FirstOrDefault(x => x.NextEventSeq.HasValue && !string.IsNullOrEmpty(x.NextTxDigest));

        var eventsBatchStream = lastIndexedShareType == null
            ? _eventApi.GetEventStream(new MoveEventEventQuery(_applicationConfiguration.CreatedShareEventType))
            : _eventApi.GetEventStream(new MoveEventEventQuery(_applicationConfiguration.CreatedShareEventType),
                new SuiNet.Types.EventId(
                    lastIndexedShareType.NextTxDigest!,
                    (long)lastIndexedShareType.NextEventSeq!.Value + 1)
            );

        var run = 0;
        await foreach (var eventsBatch in eventsBatchStream)
        {
            var firstElement = eventsBatch.First();
            
            if (run == 0 && lastIndexedShareType != null)
            {
                // update lastEventId
                lastIndexedShareType.NextTxDigest = firstElement.Id.TxDigest;
                lastIndexedShareType.NextEventSeq = (ulong)firstElement.Id.EventSeq;
                _vaultContext.ShareTypes.Update(lastIndexedShareType);
            }


            // second page, first item -> update previous page last items next cursor
            if (run != 0)
            {
                var previousShareType =
                    _vaultContext.ShareTypes.FirstOrDefault(x => string.IsNullOrEmpty(x.NextTxDigest));
                if (previousShareType != null)
                {
                    previousShareType.NextTxDigest = firstElement.Id.TxDigest;
                    previousShareType.NextEventSeq = (ulong)firstElement.Id.EventSeq;
                    _vaultContext.ShareTypes.Update(previousShareType);
                }
            }

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

            run++;
        }
    }
}