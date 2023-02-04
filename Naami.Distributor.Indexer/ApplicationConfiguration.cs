namespace Naami.Distributor.Indexer;

public class ApplicationConfiguration
{
    public string PostgreHost { get; set; }
    public string PostgrePort { get; set; }
    public string PostgreUsername { get; set; }
    public string PostgrePassword { get; set; }
    public string HangfireDatabase { get; set; }
    public string IndexingDatabase { get; set; }
    public string CreatedShareEventType { get; set; }
    public string FullNodeRpcUrl { get; set; }
}