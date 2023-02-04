using Hangfire;

namespace Naami.Distributor.Indexer;

public class ContainerJobActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public ContainerJobActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override object ActivateJob(Type jobType) => _serviceProvider.GetService(jobType);
}