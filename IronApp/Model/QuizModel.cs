using Microsoft.EntityFrameworkCore;
using IronApp.Hubs;
using IronApp.Model.ExchangeModels;
using IronApp.Model.ExchangeModels.FromClient;
using IronApp.Model.QuizEntityModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IronApp.Model
{
    public class QuizModel
    {
        public Guid GameId { get; }
        private QuizHubContext QuizHub { get; }

        private QuizContext createContext()
        {
            var options = new DbContextOptionsBuilder<QuizContext>()
              .UseInMemoryDatabase(databaseName: GameId.ToString())
              .Options;
            return new QuizContext(options);
        }

        public QuizModel(Guid guid, QuizHubContext quizHub)
        {
            GameId = guid;
            QuizHub = quizHub;
        }

        #region Player CRUD

        internal async Task PlayerConnected(int Id, string rConnection)
        {
            using (var context = createContext())
            {
                if (context.Players.Any(x => x.Id != Id))
                {
                    var player = context.Players.FirstOrDefault(x => x.Id == Id);
                    player.IsConnected = true;
                    player.RConnection = rConnection;
                    await context.SaveChangesAsync();
                    await QuizHub.NewPlayer(GameId, player.ToDto());
                }
            }
        }
        internal async Task<Player> NewPlayer(Credentials payload)
        {
            //Wenn spieler bereits eingeloggt dann mach nichts
            using (var context = createContext())
            {
                if (!context.Players.Any(x => x.Name == payload.TeamName))
                {
                    var player = new Player()
                    {
                        Name = payload.TeamName,
                        IsConnected = true
                    };
                    context.Players.Add(player);
                    await context.SaveChangesAsync();
                    await QuizHub.NewPlayer(GameId, player.ToDto());
                    return player;
                }
                else
                {
                    var player = await context.Players.FirstOrDefaultAsync(x => x.Name == payload.TeamName);
                    player.IsConnected = true;
                    await QuizHub.NewPlayer(GameId, player.ToDto());
                    return player;
                }
            }
        }

        internal async Task<Player[]> GetAllPlayers()
        {
            using (var context = createContext())
            {
                return await context.Players.ToArrayAsync();
            }
        }

        internal async Task<Player> GetPlayerById(int Id)
        {
            using (var context = createContext())
            {
                return await context.Players.FirstOrDefaultAsync(x => x.Id == Id);
            }
        }


        internal async Task PlayerDisconnected(int Id)
        {
            using (var context = createContext()) {
                if (context.Players.Any(x => x.Id != Id))
                {
                    var player = context.Players.FirstOrDefault(x => x.Id == Id);
                    player.IsConnected = false;
                    await context.SaveChangesAsync();
                    await QuizHub.NewPlayer(GameId,player.ToDto());
                }
            }
        }

        #endregion

        #region Question CRUD

        public async Task NewQuestion(Player player, NewQuestionMessage newMessage)
        {
            using(var context = createContext())
            {
                var nQuestion = new Question() { PlayerId = player.Id, QuestionText = newMessage.Question };
                context.Questions.Add(nQuestion);
                await context.SaveChangesAsync();
                await QuizHub.BroadcastQuestion(GameId, nQuestion.ToDto());
            }
        }

        public async Task<Question[]> GetAllQuestion()
        {
            using(var context = createContext())
            {
                return await context.Questions.ToArrayAsync();
            }
        }

        public async Task DeleteQuestion(Player player, DeleteQuestion requestObject)
        {
            using (var context = createContext())
            {
                var question = context.Questions.FirstOrDefault(x=> x.Id == requestObject.questionId);
                if(question != null && question.PlayerId == player.Id)
                {
                    context.Questions.Remove(question);
                    await context.SaveChangesAsync();
                    await QuizHub.BroadcastDeleteQuestion(GameId, question.ToDto());
                }
            }
        }
        public async Task EditQuestion(Player player, EditQuestion requestObject)
        {
            using (var context = createContext())
            {
                var question = context.Questions.FirstOrDefault(x => x.Id == requestObject.questionId);
                if (question != null && question.PlayerId == player.Id)
                {
                    question.QuestionText = requestObject.question;
                    await context.SaveChangesAsync();
                    await QuizHub.BroadcastQuestion(GameId, question.ToDto());
                }
            }
        }

        #endregion

        #region Answers CRUD
        public async Task NewAnswer(Player player, NewAnswerMessage newMessage)
        {
            using (var context = createContext())
            {
                var oldAnswer = await context.Answers.FirstOrDefaultAsync(x => x.PlayerId == player.Id && x.QuestionId == newMessage.QuestionId);
                if (oldAnswer != null)
                {
                    oldAnswer.AnswerText = newMessage.AnswerText;
                }
                else
                {
                    oldAnswer = new Answer() { PlayerId = player.Id, AnswerText = newMessage.AnswerText, QuestionId = newMessage.QuestionId};
                    context.Answers.Add(oldAnswer);
                }
                await context.SaveChangesAsync();
                await QuizHub.BroadcastAnswer(GameId, oldAnswer.ToDto());
            }
        }

        public async Task<Answer[]> GetAllAnswer()
        {
            using (var context = createContext())
            {
                return await context.Answers.ToArrayAsync();
            }
        }

        #endregion
    }
}
