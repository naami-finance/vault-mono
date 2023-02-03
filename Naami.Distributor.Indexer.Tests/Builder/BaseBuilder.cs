using Naami.SuiNet.Types;
using Naami.SuiNet.Types.Events;

namespace Naami.Distributor.Indexer.Tests.Builder;

public abstract class BaseBuilder<T> where T : new()
{
    protected readonly T Entity;

    protected BaseBuilder()
    {
        Entity = new T();
    }

    public T Build() => Entity;
}

public class SuiEventBuilder : BaseBuilder<SuiEvent>
{
    public SuiEventBuilder WithMoveEvent(MoveEvent moveEvent)
    {
        Entity.MoveEvent = moveEvent;
        return this;
    }
}