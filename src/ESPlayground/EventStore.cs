namespace ESPlayground;

internal class EventStore<T>
    where T : IEventStoreEntity, new()
{
    private readonly Dictionary<Guid, SortedList<DateTime, Event>> eventStore = [];
    private readonly Dictionary<Guid, T> eventEntities = [];

    public void Append(Event @event)
    {
        var stream = eventStore!.GetValueOrDefault(@event.StreamId, null);
        if (stream is null)
        {
            eventStore[@event.StreamId] = [];
        }

        eventStore[@event.StreamId].Add(DateTime.UtcNow, @event);

        eventEntities[@event.StreamId] = GetEntity(@event.StreamId)!;
    }

    public IList<Event> GetEvents(Guid streamId)
    {
        return eventStore.GetValueOrDefault(streamId, null).Values ?? [];
    }

    public T? GetView(Guid id)
    {
        return eventEntities!.GetValueOrDefault(id, default);
    }

    public T? GetEntity(Guid id)
    {
        if (!eventStore.TryGetValue(id, out SortedList<DateTime, Event>? value))
        {
            return default;
        }

        var entity = new T();
        foreach (var @event in value.Values)
        {
            entity.Apply(@event);
        }

        return entity;
    }
}

internal abstract record Event(Guid StreamId, DateTime CreatedAtUtc);