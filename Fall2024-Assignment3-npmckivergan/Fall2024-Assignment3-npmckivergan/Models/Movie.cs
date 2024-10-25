namespace Fall2024_Assignment3_npmckivergan.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string IMDB_link { get; set; }
        public byte[]? Media { get; set; }
    }
}
