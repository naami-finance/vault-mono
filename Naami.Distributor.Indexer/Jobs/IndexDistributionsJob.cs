using System.Text;
using Naami.Distributor.Data;
using Naami.Distributor.SDK;
using Naami.Distributor.SDK.Models.Distributor.Events;
using Naami.Distributor.SDK.Models.Share.Events;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Event.Query;
using Naami.SuiNet.Extensions.ApiStreams;
using ServiceStack;
using EventId = Naami.SuiNet.Types.EventId;

namespace Naami.Distributor.Indexer.Jobs;

public class IndexDistributionsJob
{
    private readonly IEventApi _eventApi;
    private readonly VaultContext _vaultContext;
    private readonly ApplicationConfiguration _applicationConfiguration;

    public IndexDistributionsJob(IEventApi eventApi, VaultContext vaultContext,
        ApplicationConfiguration applicationConfiguration)
    {
        _eventApi = eventApi;
        _vaultContext = vaultContext;
        _applicationConfiguration = applicationConfiguration;
    }

    public async Task RunAsync()
    {
        var eventSnapshot = _vaultContext.EventSourcingSnapshots.FirstOrDefault() ?? new EventSourcingSnapshot();
        var lastEventId = eventSnapshot.DistributionEventSeq.HasValue &&
                          !string.IsNullOrEmpty(eventSnapshot.DistributionTxDigest)
            ? new EventId(eventSnapshot.DistributionTxDigest, (long)eventSnapshot.DistributionEventSeq + 1)
            : null;

        var eventType = $"{_applicationConfiguration.VaultPackageId}::distributor_events::DistributionCreated";
        var eventsBatchStream = lastEventId == null
            ? _eventApi.GetEventStream(new MoveEventEventQuery(eventType))
            : _eventApi.GetEventStream(new MoveEventEventQuery(eventType),
                lastEventId
            );


        await foreach (var eventsBatch in eventsBatchStream)
        {
            var distributions = eventsBatch.Select((ev, eventIndex) =>
            {
                var distributionCreatedEvent = ev.Event.MoveEvent!.Fields.FromObjectDictionary<DistributionCreated>();

                return new Distribution
                {
                    CoinType = Encoding.ASCII.GetString(distributionCreatedEvent.CoinType).AsFormattedAddress(),
                    ShareType = Encoding.ASCII.GetString(distributionCreatedEvent.ShareType).AsFormattedAddress(),
                    Id = distributionCreatedEvent.DistributionId,
                    InitialAmount = distributionCreatedEvent.Amount,
                    RemainingAmount = distributionCreatedEvent.Amount,
                    CreatedAt = distributionCreatedEvent.Timestamp,
                };
            }).Where(x => _vaultContext.Distributions.All(s => s.Id != x.Id));


            await _vaultContext.Distributions.AddRangeAsync(distributions.ToArray());

            if (eventsBatch.Any())
            {
                var lastEvent = eventsBatch.Last();
                if (_vaultContext.EventSourcingSnapshots.Any())
                {
                    var snapshot = _vaultContext.EventSourcingSnapshots.Single();
                    snapshot.DistributionEventSeq = (ulong)lastEvent.Id.EventSeq;
                    snapshot.DistributionTxDigest = lastEvent.Id.TxDigest;
                    _vaultContext.Update(snapshot);
                }
                else
                {
                    var snapshot = new EventSourcingSnapshot
                    {
                        DistributionEventSeq = (ulong)lastEvent.Id.EventSeq,
                        DistributionTxDigest = lastEvent.Id.TxDigest
                    };
                    await _vaultContext.EventSourcingSnapshots.AddAsync(snapshot);
                }
            }

            await _vaultContext.SaveChangesAsync();
        }
    }
}