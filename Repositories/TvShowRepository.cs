public record ShowEntity
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public DateTimeOffset FetchedAt { get; init; }
    public List<string> CastNames { get; init; } = new();
}

public interface IShowRepository
{
    Task UpsertAsync(ShowEntity show, CancellationToken ct);
}

// Demo: in-memory store (swap for EF Core)
public sealed class InMemoryShowRepository : IShowRepository
{
    private readonly Dictionary<int, ShowEntity> _db = new();

    public Task UpsertAsync(ShowEntity show, CancellationToken ct)
    {
        _db[show.Id] = show;
        return Task.CompletedTask;
    }

    // For testing/inspection
    public ShowEntity? Get(int id) => _db.TryGetValue(id, out var v) ? v : null;
}
