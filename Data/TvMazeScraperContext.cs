using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tvmaze_scraper.Models;

namespace tvmaze_scraper.Data
{
    public class TvMazeScraperContext : DbContext
    {
        public TvMazeScraperContext (DbContextOptions<TvMazeScraperContext> options)
            : base(options)
        {
        }

        public DbSet<tvmaze_scraper.Models.TvShow> TvShow { get; set; } = default!;
    }
}
