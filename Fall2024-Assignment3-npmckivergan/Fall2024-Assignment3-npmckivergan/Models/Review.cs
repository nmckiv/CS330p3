namespace Fall2024_Assignment3_npmckivergan.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public int? ActorId { get; set; }
        public string Content { get; set; }
        public float Rating { get; set; }
        public string ReviewerName { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
