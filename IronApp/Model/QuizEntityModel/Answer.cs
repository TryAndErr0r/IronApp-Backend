using IronApp.Model.ExchangeModels.ToClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronApp.Model.QuizEntityModel
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AnswerText { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public AnswerDto ToDto()
        {
            return new AnswerDto()
            {
                Id = Id,
                AnswerText = AnswerText,
                QuestionId = QuestionId,
                PlayerId = PlayerId,
            };
        }
    }


}
