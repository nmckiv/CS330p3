namespace Fall2024_Assignment3_npmckivergan.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string IMDB_link { get; set; }
        public byte[]? Photo { get; set; }
        public float OverallSentiment { get; set; }
        public List<Review> Tweets { get; set; } = new List<Review>();
    }
}
