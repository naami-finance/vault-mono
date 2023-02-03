using Naami.SuiNet.Types;

namespace Naami.Distributor.SDK.Models.Share.Events;

public record ShareMetadataUpdated(
    ObjectId Id,
    string Name,
    string Symbol
);