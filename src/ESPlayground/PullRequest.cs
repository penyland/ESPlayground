namespace ESPlayground;

internal class PullRequest : IEventStoreEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public void Apply(CreatedEvent createdEvent)
    {
        Id = createdEvent.Id;
        Title = $"Pull request {Id} created";
    }

    public void Apply(UpdatedEvent updatedEvent)
    {
        Title = $"Updated pull request {Id}";
    }

    public void Apply(DeletedEvent deletedEvent)
    {
        Title = $"Deleted pull request {Id}";
    }

    public void Apply(Event @event)
    {
        switch (@event)
        {
            case CreatedEvent createdEvent:
                Apply(createdEvent);
                break;
            case UpdatedEvent updatedEvent:
                Apply(updatedEvent);
                break;
            case DeletedEvent deletedEvent:
                Apply(deletedEvent);
                break;
        }
    }
}
internal record CreatedEvent : Event
{
    public int Id { get; set; }
}

internal record UpdatedEvent : Event
{
    public int Id { get; set; }

    public string Reason { get; set; }
}

internal record DeletedEvent : Event
{
    public int Id { get; set; }

    public DateTime DeletedAtUtc { get; set; }
}
