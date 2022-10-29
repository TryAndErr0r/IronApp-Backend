namespace IronApp.Model.ExchangeModels.ToClient
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public int QuestionId { get; set; }
        public int PlayerId { get; set; }
    }
}
