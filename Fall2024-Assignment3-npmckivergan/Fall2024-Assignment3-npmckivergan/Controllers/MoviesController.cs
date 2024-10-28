using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_npmckivergan.Data;
using Fall2024_Assignment3_npmckivergan.Models;
using System.Numerics;
using OpenAI.Chat;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.AspNetCore.OutputCaching;

namespace Fall2024_Assignment3_npmckivergan.Controllers
{
    public class MoviesController : Controller
    {

        private readonly ILogger<MoviesController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ChatClient _client;

        public MoviesController(ApplicationDbContext context, IConfiguration config, ILogger<MoviesController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;

            var apiKey = "5c906e8218294152b82454d70be2277a";
            var apiEndpoint = "https://fall2024-assignment1-npmckivergan-openai.openai.azure.com/";
            AzureOpenAIClient chat = new(new Uri(apiEndpoint), new System.ClientModel.ApiKeyCredential(apiKey));

            _client = chat.GetChatClient("gpt-35-turbo");
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Year,IMDB_link,Media")] Movie movie, IFormFile? Media)
        {
            if (ModelState.IsValid)
            {
                if (Media != null && Media.Length > 0)
                {
                    using var memoryStream = new MemoryStream(); // Dispose() for garbage collection 
                    await Media.CopyToAsync(memoryStream);
                    movie.Media = memoryStream.ToArray();
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,Year,IMDB_link,Media")] Movie movie, IFormFile? Media)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Media != null && Media.Length > 0)
                    {
                        using var memoryStream = new MemoryStream(); // Dispose() for garbage collection 
                        await Media.CopyToAsync(memoryStream);
                        movie.Media = memoryStream.ToArray();
                    }
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReview(int movieId)
        {

            var movie = await _context.Movie.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == movieId);

            string prompt = "Context: You are a dumb football player with the IQ of a rock.  You love simple things and hate anything your pea-sized brain can't follow.  You love hot women, titties, ass, drugs, violence, explosions, and sports.  You think romance is gay.\\n\\nInstructions: Review the given movie from this point of view.  Sound very stupid.  Be completely unhinged.  Make the review between 100 and 150 words.  Please review " + movie.Title;

            ChatCompletion completion = await _client.CompleteChatAsync(prompt);
            // Generate a hardcoded dummy review
            var review = new Review
            {
                MovieId = movieId,
                Content = completion.Content[0].Text,
                Rating = new Random().Next(0, 101),
                ReviewerName = "John Doe"
            };

            // Retrieve the movie and add the review
            
            if (movie == null) return NotFound();

            movie.Reviews.Add(review); // Add the review to the movie's reviews

            // Now save changes to the database
            await _context.SaveChangesAsync();

            var updatedMovie = await _context.Movie.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == movieId);
            _logger.LogInformation($"Number of Reviews After Save: {updatedMovie.Reviews.Count}");
            foreach (var rev in updatedMovie.Reviews)
            {
                _logger.LogInformation($"Review Content: {rev.Content}");
            }

            return RedirectToAction("Details", new { id = movieId });
        }
        public async Task<IActionResult> DeleteReview(int movieId, int reviewId)
        {
            // Retrieve the movie including its reviews
            var movie = await _context.Movie
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
                return NotFound();

            // Find the review to delete
            var review = movie.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                return NotFound();

            // Remove the review from the collection
            movie.Reviews.Remove(review);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the movie details page
            return RedirectToAction("Details", new { id = movieId });
        }
    }
}
