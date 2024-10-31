using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Fall2024_Assignment3_npmckivergan.Models
{
    public class ActorMovieCreateViewModel
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }

        public IEnumerable<SelectListItem> Movies { get; set; }
        public IEnumerable<SelectListItem> Actors { get; set; }
    }
}
