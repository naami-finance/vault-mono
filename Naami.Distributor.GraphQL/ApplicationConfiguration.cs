namespace Naami.Distributor.GraphQL;

public class ApplicationConfiguration
{
    public string RpcNodeUrl { get; set; }
    public string PostgreHost { get; set; }
    public string PostgrePort { get; set; }
    public string PostgreUsername { get; set; }
    public string PostgrePassword { get; set; }
    public string Database { get; set; }
}
