using Dumpify;
using ESPlayground;

Console.WriteLine("Hello, World!");

var eventStore = new EventStore<PullRequest>();

var streamId = Guid.NewGuid(); // Pull request ID

eventStore.Append(new CreatedEvent { StreamId = streamId, Id = 1 });
eventStore.Append(new CreatedEvent { StreamId = Guid.NewGuid(), Id = 2 });
eventStore.Append(new UpdatedEvent() { StreamId = streamId, Reason = "Reviewer added"});
eventStore.Append(new UpdatedEvent() { StreamId = streamId, Reason = "Approved"});
eventStore.Append(new DeletedEvent() { StreamId = streamId , DeletedAtUtc = DateTime.UtcNow});

var events = eventStore.GetEvents(streamId);

Console.WriteLine($"Events for stream {streamId}:", streamId.ToString());
Console.WriteLine($"Events {events.Count}");

var json = eventStore.GetEventsAsJson(streamId);

var pullRequest = eventStore.GetView(streamId);
pullRequest.Dump();

Console.WriteLine();
