using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_npmckivergan.Data;
using Fall2024_Assignment3_npmckivergan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fall2024_Assignment3_npmckivergan.Controllers
{
    public class ActorMoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorMoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActorMovies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ActorMovie.Include(a => a.Actor).Include(a => a.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActorMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorMovie = await _context.ActorMovie
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorMovie == null)
            {
                return NotFound();
            }

            return View(actorMovie);
        }

        // GET: ActorMovies/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ActorMovieCreateViewModel
            {
                Movies = await _context.Movie.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title
                }).ToListAsync(),
                Actors = await _context.Actor.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: ActorMovies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActorMovieCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool alreadyExists = await _context.ActorMovie
                    .AnyAsync(m => m.MovieId == viewModel.MovieId && m.ActorId == viewModel.ActorId);

                if (!alreadyExists)
                {
                    var actorMovie = new ActorMovie
                    {
                        MovieId = viewModel.MovieId,
                        ActorId = viewModel.ActorId
                    };

                    _context.Add(actorMovie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Cannot add the same entry multiple times");
            }

            viewModel.Movies = await _context.Movie.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Title
            }).ToListAsync();

            viewModel.Actors = await _context.Actor.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToListAsync();

            return View(viewModel);
        }

        // GET: ActorMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorMovie = await _context.ActorMovie.FindAsync(id);
            if (actorMovie == null)
            {
                return NotFound();
            }

            var viewModel = new ActorMovieCreateViewModel
            {
                MovieId = actorMovie.MovieId,
                ActorId = actorMovie.ActorId,
                Movies = await _context.Movie.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title
                }).ToListAsync(),
                Actors = await _context.Actor.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: ActorMovies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActorMovieCreateViewModel viewModel)
        {
            if (id != viewModel.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var actorMovie = await _context.ActorMovie.FindAsync(id);
                    if (actorMovie == null)
                    {
                        return NotFound();
                    }

                    actorMovie.MovieId = viewModel.MovieId;
                    actorMovie.ActorId = viewModel.ActorId;

                    _context.Update(actorMovie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorMovieExists(viewModel.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Repopulate the select lists if model state is invalid
            viewModel.Movies = await _context.Movie.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Title
            }).ToListAsync();

            viewModel.Actors = await _context.Actor.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToListAsync();

            return View(viewModel);
        }

        // GET: ActorMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorMovie = await _context.ActorMovie
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorMovie == null)
            {
                return NotFound();
            }

            return View(actorMovie);
        }

        // POST: ActorMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorMovie = await _context.ActorMovie.FindAsync(id);
            if (actorMovie != null)
            {
                _context.ActorMovie.Remove(actorMovie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorMovieExists(int id)
        {
            return _context.ActorMovie.Any(e => e.Id == id);
        }
    }
}
