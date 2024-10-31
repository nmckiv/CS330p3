using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Fall2024_Assignment3_npmckivergan.Models
{
    public class ActorMovieCreateViewModel
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ActorId { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Movies { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Actors { get; set; }
    }
}
