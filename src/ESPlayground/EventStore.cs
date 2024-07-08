using System.Text.Json;
using System.Text.Json.Serialization;

namespace ESPlayground;

internal class EventStore<T>
    where T : IEventStoreEntity, new()
{
    private readonly Dictionary<Guid, SortedList<DateTime, EventEntity>> eventStore = [];
    private readonly Dictionary<Guid, T> eventEntities = [];

    public void Append(Event @event)
    {
        var stream = eventStore!.GetValueOrDefault(@event.StreamId, null);
        if (stream is null)
        {
            eventStore[@event.StreamId] = [];
        }

        var eventWithData = new EventEntity
        {
            StreamId = @event.StreamId,
            CreatedAtUtc = DateTime.UtcNow,
            Data = @event,
            Type = @event.GetType()
        };

        eventStore[@event.StreamId].Add(DateTime.UtcNow, eventWithData);

        eventEntities[@event.StreamId] = GetEntity(@event.StreamId)!;
    }

    public IList<Event> GetEvents(Guid streamId)
    {
        var entity =  eventStore.GetValueOrDefault(streamId, null).Values ?? [];
        return entity.Select(e => e.Data).ToList();
    }

    public string GetEventsAsJson(Guid id)
    {
        var values = eventStore!.GetValueOrDefault(id, default);

        var json = JsonSerializer.Serialize(eventStore!.GetValueOrDefault(id, default));
        return json;
    }

    public T? GetView(Guid id)
    {
        return eventEntities!.GetValueOrDefault(id, default);
    }

    public T? GetEntity(Guid id)
    {
        if (!eventStore.TryGetValue(id, out var value))
        {
            return default;
        }

        var entity = new T();
        foreach (var @event in value.Values)
        {
            entity.Apply(@event.Data);
        }

        return entity;
    }
}

internal abstract record Event
{
    public required Guid StreamId { get; set; }
}

internal record EventEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required Guid StreamId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [property: JsonIgnore]
    public Type? Type { get; set; }

    public Event? Data { get; set; }
}
