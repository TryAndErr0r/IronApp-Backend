using Microsoft.AspNetCore.SignalR;
using IronApp.Model.ExchangeModels.ToClient;
using System;
using System.Threading.Tasks;

namespace IronApp.Hubs
{
    public class QuizHubContext
    {

        IHubContext<QuizHub> quizhubContext;
        public QuizHubContext(IHubContext<QuizHub> quizhubContext)
        {
            this.quizhubContext = quizhubContext;
        }
        public async Task NewPlayer(Guid gameid, PlayerDto Playername)
        {
            await quizhubContext.Clients.Group(gameid.ToString()).SendAsync("NewPlayer", Playername);
        }
        public async Task BroadcastQuestion(Guid gameid, QuestionDto question)
        {
            await quizhubContext.Clients.Group(gameid.ToString()).SendAsync("NewQuestion", question);
        }

        public async Task BroadcastDeleteQuestion(Guid gameid, QuestionDto question)
        {
            await quizhubContext.Clients.Group(gameid.ToString()).SendAsync("DeleteQuestion", question);
        }

        public async Task BroadcastAnswer(Guid gameid, AnswerDto answer)
        {
            await quizhubContext.Clients.Group(gameid.ToString()).SendAsync("NewAnswer", answer);
        }
    }
}
