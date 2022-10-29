using IronApp.Hubs;
using IronApp.Model.QuizEntityModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IronApp.Model
{
    public class QuizService
    {
        List<QuizModel> models = new List<QuizModel>();
        QuizHubContext quizHub;
        public QuizService(QuizHubContext quizHub)
        {
            this.quizHub = quizHub;

        }
        public Guid CreateNewQuiz()
        {
            QuizModel quizModel = new QuizModel(Guid.NewGuid(), quizHub);
            models.Add(quizModel);
            return quizModel.GameId;
        }

        public QuizModel GetModelByGuid(Guid id)
        {
            return models.Find(x => x.GameId == id);
        }

        public async Task<Player> NewPlayer(Guid nQuizId,Credentials payload)
        {
            return await GetModelByGuid(nQuizId).NewPlayer( payload);
        }
    }
}
