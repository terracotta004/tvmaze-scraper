public sealed class ScraperOptions
{
    public string Url { get; set; } = "";
    public int IntervalSeconds { get; set; } = 300;
    public string UserAgent { get; set; } = "TvMazeScraperBot/1.0";
}