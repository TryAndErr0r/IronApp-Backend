using IronApp.Model.ExchangeModels.ToClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronApp.Model.QuizEntityModel
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string QuestionText { get; set; }
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public QuestionDto ToDto()
        {
            return new QuestionDto()
            {
                Id = Id,
                QuestionText=QuestionText,
                PlayerId = PlayerId
            };
        }
    }

  
}
