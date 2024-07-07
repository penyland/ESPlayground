namespace ESPlayground;

internal interface IEventStoreEntity
{
    void Apply(Event @event);
}