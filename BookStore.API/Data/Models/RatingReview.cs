namespace BookStore.API.Data.Models
{
    public class RatingReview
    {
        public int ProductId { get; set; }
        public string? Review { get; set; } 
       public decimal? Rating { get; set; }
        public string? Title { get; set; } 
        public string? CustomerName { get; set; } 
       public string? UserName { get; set; }
       public string? Image { get; set; } 
        public string? errormessage { get; set; }
    }
}
