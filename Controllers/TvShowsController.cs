// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tvmaze_scraper.Data;
using tvmaze_scraper.Models;

// namespace tvmaze_scraper.Controllers
// {
//     public class TvShowController : Controller
//     {



//         // GET: TvShow
//         public async Task<IActionResult> Index()
//         {
//             return View(await _context.TvShow.ToListAsync());
//         }

//         // GET: TvShow/Details/5
//         public async Task<IActionResult> Details(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }

//             var tvShow = await _context.TvShow
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (tvShow == null)
//             {
//                 return NotFound();
//             }

//             return View(tvShow);
//         }

//         // GET: TvShow/Create
//         public IActionResult Create()
//         {
//             return View();
//         }

//         // POST: TvShow/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Id,Name")] TvShow tvShow)
//         {
//             if (ModelState.IsValid)
//             {
//                 _context.Add(tvShow);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(tvShow);
//         }

//         // GET: TvShow/Edit/5
//         public async Task<IActionResult> Edit(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }

//             var tvShow = await _context.TvShow.FindAsync(id);
//             if (tvShow == null)
//             {
//                 return NotFound();
//             }
//             return View(tvShow);
//         }

//         // POST: TvShow/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TvShow tvShow)
//         {
//             if (id != tvShow.Id)
//             {
//                 return NotFound();
//             }

//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(tvShow);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!TvShowExists(tvShow.Id))
//                     {
//                         return NotFound();
//                     }
//                     else
//                     {
//                         throw;
//                     }
//                 }
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(tvShow);
//         }

//         // GET: TvShow/Delete/5
//         public async Task<IActionResult> Delete(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }

//             var tvShow = await _context.TvShow
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (tvShow == null)
//             {
//                 return NotFound();
//             }

//             return View(tvShow);
//         }

//         // POST: TvShow/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(int id)
//         {
//             var tvShow = await _context.TvShow.FindAsync(id);
//             if (tvShow != null)
//             {
//                 _context.TvShow.Remove(tvShow);
//             }

//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }

//         private bool TvShowExists(int id)
//         {
//             return _context.TvShow.Any(e => e.Id == id);
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;

namespace tvmaze_scraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvShowsController : ControllerBase
    {
        private readonly TvMazeScraperContext _context;

        public TvShowsController(TvMazeScraperContext context)
        {
            _context = context;
        }

        // GET: api/tvshows
        [HttpGet]
        public IActionResult GetAllShows()
        {
        var shows = new List<TvShow> { new TvShow
        {
            Id = 1,
            Name = "Show1",
            Cast = new List<Actor>
            {
                new Actor { Id = 1, Name = "Actor1", Birthday = new DateOnly(1980, 1, 1) },
                new Actor { Id = 2, Name = "Actor2", Birthday = new DateOnly(1990, 2, 2) }
            }
        }
        };
        return Ok(shows); // Returns 200 OK with the list of shows
        }
    }
}
