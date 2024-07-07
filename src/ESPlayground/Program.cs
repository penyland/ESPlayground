using ESPlayground;

Console.WriteLine("Hello, World!");

var eventStore = new EventStore<PullRequest>();

var streamId = Guid.NewGuid(); // Pull request ID

eventStore.Append(new CreatedEvent(1) { StreamId = streamId });
eventStore.Append(new CreatedEvent(2) { StreamId = Guid.NewGuid() });
eventStore.Append(new UpdatedEvent() { StreamId = streamId });
eventStore.Append(new DeletedEvent() { StreamId = streamId });

var events = eventStore.GetEvents(streamId);

Console.WriteLine($"Events for stream {streamId}:", streamId.ToString());
Console.WriteLine($"Events {events.Count}");

var pullRequest = eventStore.GetView(streamId);


Console.WriteLine();