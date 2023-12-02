namespace BookStore.API.Data.Models
{
    public class Questionair
    {
        public int ProductId { get; set; }
        public string? Questions { get; set; }
        public string? Answers { get; set; }
        public DateTime QuestioneDate { get; set; }
        public DateTime AnswerDate { get; set; }
        public string? errormessage { get; set; }
    }
}
