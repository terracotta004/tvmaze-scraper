using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;

public sealed class TvMazeScraperService : BackgroundService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly ILogger<TvMazeScraperService> _logger;
    private readonly ScraperOptions _options;
    private readonly IShowRepository _repo;

    // Simple in-memory ETag cache for one endpoint
    private string? _lastEtag;
    private DateTimeOffset? _lastModified;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public TvMazeScraperService(
        IHttpClientFactory httpFactory,
        IOptions<ScraperOptions> options,
        IShowRepository repo,
        ILogger<TvMazeScraperService> logger)
    {
        _httpFactory = httpFactory;
        _logger = logger;
        _repo = repo;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TVMaze scraper started. Every {Interval}s", _options.IntervalSeconds);

        var timer = new PeriodicTimer(TimeSpan.FromSeconds(_options.IntervalSeconds));

        try
        {
            await RunOnceAsync(stoppingToken); // initial tick

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunOnceAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Scraper crashed");
        }
        finally
        {
            _logger.LogInformation("TVMaze scraper stopped.");
        }
    }

    private async Task RunOnceAsync(CancellationToken ct)
    {
        var http = _httpFactory.CreateClient("tvmaze");
        using var req = new HttpRequestMessage(HttpMethod.Get, _options.Url);

        if (!string.IsNullOrEmpty(_lastEtag))
            req.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(_lastEtag));

        if (_lastModified is { } lm)
            req.Headers.IfModifiedSince = lm;

        try
        {
            using var res = await http.SendAsync(req, ct);

            if (res.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                _logger.LogInformation("No changes (304 Not Modified).");
                return;
            }

            res.EnsureSuccessStatusCode();

            // Track ETag/Last-Modified for next time
            _lastEtag = res.Headers.ETag?.Tag;
            if (res.Content.Headers.LastModified is { } lm2)
                _lastModified = lm2;

            var stream = await res.Content.ReadAsStreamAsync(ct);
            var show = await JsonSerializer.DeserializeAsync<TvMazeShow>(stream, JsonOpts, ct);
            if (show is null)
            {
                _logger.LogWarning("Deserialized show was null.");
                return;
            }

            var castNames = (show.Embedded?.Cast ?? new())
                .Select(c => c.Person?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct()
                .OrderBy(n => n)
                .ToList()!;

            var entity = new ShowEntity
            {
                Id = show.Id,
                Name = show.Name,
                FetchedAt = DateTimeOffset.UtcNow,
                CastNames = castNames
            };

            await _repo.UpsertAsync(entity, ct);
            _logger.LogInformation("Upserted Show {Id} '{Name}' with {CastCount} cast members.",
                entity.Id, entity.Name, entity.CastNames.Count);
        }
        catch (OperationCanceledException) { }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "HTTP error contacting TVMaze");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
        }
    }
}
