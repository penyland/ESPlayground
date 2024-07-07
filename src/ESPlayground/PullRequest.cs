namespace ESPlayground;

internal class PullRequest : IEventStoreEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public void Apply(CreatedEvent createdEvent)
    {
        Id = createdEvent.Id;
        Title = $"Pull request {Id}";
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
    public CreatedEvent(int id) : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        Id = id;
    }

    public int Id { get; set; }
}

internal record UpdatedEvent : Event
{
    public UpdatedEvent() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}

internal record DeletedEvent : Event
{
    public DeletedEvent() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}