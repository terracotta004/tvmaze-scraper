namespace tvmaze_scraper.Models;

public class TvShow
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Actor>? Cast { get; set; }
}