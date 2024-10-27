namespace Fall2024_Assignment3_npmckivergan.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; }
        public string? ReviewerName { get; set; }
    }
}
