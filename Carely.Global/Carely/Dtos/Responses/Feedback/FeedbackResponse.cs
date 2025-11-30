namespace Carely.Dtos.Responses.Feedback
{
    public class FeedbackResponse
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public int MotherId { get; set; }

        public static FeedbackResponse FromEntity(Models.Feedback f) =>
            new FeedbackResponse
            {
                Id = f.Id,
                Stars = f.Stars,
                Comment = f.Comment,
                MotherId = f.MotherId
            };
    }
}
