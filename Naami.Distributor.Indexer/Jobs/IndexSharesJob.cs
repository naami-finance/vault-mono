using System.Text;
using Naami.Distributor.Data;
using Naami.Distributor.SDK;
using Naami.Distributor.SDK.Models.Share.Events;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Event.Query;
using Naami.SuiNet.Extensions.ApiStreams;
using ServiceStack;
using EventId = Naami.SuiNet.Types.EventId;

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
        var eventSnapshot = _vaultContext.EventSourcingSnapshots.FirstOrDefault() ?? new EventSourcingSnapshot();
        var lastEventId = eventSnapshot.ShareTypeEventSeq.HasValue &&
                          !string.IsNullOrEmpty(eventSnapshot.ShareTypeTxDigest)
            ? new EventId(eventSnapshot.ShareTypeTxDigest, (long)eventSnapshot.ShareTypeEventSeq + 1) : null;
        
        
        var eventType = $"{_applicationConfiguration.SharesPackageId}::registry::ShareCreated";
        var eventsBatchStream = lastEventId == null
            ? _eventApi.GetEventStream(new MoveEventEventQuery(eventType))
            : _eventApi.GetEventStream(new MoveEventEventQuery(eventType),
                lastEventId
            );

       
        await foreach (var eventsBatch in eventsBatchStream)
        {
            var shareTypes = eventsBatch.Select((ev, eventIndex) =>
            {
                var shareCreatedEvent = ev.Event.MoveEvent!.Fields.FromObjectDictionary<ShareCreated>();

                return new ShareType
                {
                    Name = Encoding.ASCII.GetString(shareCreatedEvent.Name),
                    Symbol = Encoding.ASCII.GetString(shareCreatedEvent.Symbol),
                    TotalSupply = shareCreatedEvent.TotalSupply,
                    MetadataObjectId = shareCreatedEvent.MetadataId,
                    RegistryObjectId = shareCreatedEvent.RegistryId,
                    ObjectType = Encoding.ASCII.GetString(shareCreatedEvent.Type).AsFormattedAddress(),
                };
            }).Where(x => _vaultContext.ShareTypes.All(s => s.ObjectType != x.ObjectType));


            await _vaultContext.ShareTypes.AddRangeAsync(shareTypes.ToArray());
            
            if (eventsBatch.Any())
            {
                var lastEvent = eventsBatch.Last();
                if (_vaultContext.EventSourcingSnapshots.Any())
                {
                    var snapshot = _vaultContext.EventSourcingSnapshots.Single();
                    snapshot.ShareTypeEventSeq = (ulong)lastEvent.Id.EventSeq;
                    snapshot.ShareTypeTxDigest = lastEvent.Id.TxDigest;
                    _vaultContext.Update(snapshot);
                }
                else
                {
                    var snapshot = new EventSourcingSnapshot
                    {
                        ShareTypeEventSeq = (ulong)lastEvent.Id.EventSeq,
                        ShareTypeTxDigest = lastEvent.Id.TxDigest
                    };
                    await _vaultContext.EventSourcingSnapshots.AddAsync(snapshot);
                }
            }
            
            await _vaultContext.SaveChangesAsync();
        }
    }
}