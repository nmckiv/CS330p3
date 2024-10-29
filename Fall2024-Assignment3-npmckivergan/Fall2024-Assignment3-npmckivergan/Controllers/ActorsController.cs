using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_npmckivergan.Data;
using Fall2024_Assignment3_npmckivergan.Models;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Identity.Client;

namespace Fall2024_Assignment3_npmckivergan.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ILogger<MoviesController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly ChatClient _client;

        public ActorsController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _logger = logger;

            _context = context;

            var apiKey = "5c906e8218294152b82454d70be2277a";
            var apiEndpoint = "https://fall2024-assignment1-npmckivergan-openai.openai.azure.com/";
            AzureOpenAIClient chat = new(new Uri(apiEndpoint), new System.ClientModel.ApiKeyCredential(apiKey));

            _client = chat.GetChatClient("gpt-35-turbo");
        }

        public async Task<IActionResult> GetActorPhoto(int id)
        {
            var actor = await _context.Actor.FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            var imageData = actor.Photo;

            return File(imageData, "image/jpg");
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actor.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .Include(m => m.Tweets)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,Age,IMDB_link,Photo")] Actor actor, IFormFile? Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0)
                {
                    using var memoryStream = new MemoryStream(); // Dispose() for garbage collection 
                    await Photo.CopyToAsync(memoryStream);
                    actor.Photo = memoryStream.ToArray();
                }
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Age,IMDB_link,Photo")] Actor actor, IFormFile? Photo)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Photo != null && Photo.Length > 0)
                    {
                        using var memoryStream = new MemoryStream(); // Dispose() for garbage collection 
                        await Photo.CopyToAsync(memoryStream);
                        actor.Photo = memoryStream.ToArray();
                    }
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                _context.Actor.Remove(actor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReview(int movieId)
        {

            var movie = await _context.Actor.Include(m => m.Tweets).FirstOrDefaultAsync(m => m.Id == movieId);
            string prompt = "Context: You are a dumb football player with the IQ of a rock.  You love simple things and hate anything your pea-sized brain can't follow.  You love hot women, titties, ass, drugs, violence, explosions, and sports.\r\n\r\nInstructions: Write a Tweet about the particular actor or actress.  No more than 50 words but it can be a lot shorter.  Sound very stupid.  Be completely unhinged.  Feel free to use hashtags.  And you can say gay stuff about the guys too. Please review " + movie.Name;
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

            movie.Tweets.Add(review); // Add the review to the movie's reviews

            // Now save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = movieId });
        }
        public async Task<IActionResult> DeleteReview(int movieId, int reviewId)
        {
            // Retrieve the movie including its reviews
            var movie = await _context.Actor
                .Include(m => m.Tweets)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
                return NotFound();

            // Find the review to delete
            var review = movie.Tweets.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                return NotFound();

            // Remove the review from the collection
            movie.Tweets.Remove(review);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the movie details page
            return RedirectToAction("Details", new { id = movieId });
        }
    }
}
