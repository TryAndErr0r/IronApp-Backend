using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using IronApp.Model;
using IronApp.UserManagement;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IronApp.Hubs
{
    public class QuizHub : Hub
    {
        IJwtAuth jwtAuth;
        public QuizHub(IServiceProvider serviceProvider, IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
            this.serviceProvider = serviceProvider;
        }
        IServiceProvider serviceProvider { get; }
        private QuizService _quizService;
        public QuizService QuizService
        {
            get
            {
                if (_quizService == null)
                {
                    _quizService = serviceProvider.GetService<QuizService>();
                }
                return _quizService;
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = this.Context.Features.Get<IHttpContextFeature>();
            var queryCollection = httpContext.HttpContext.Request.Query;
            if (queryCollection.ContainsKey("bearer"))
            {
                var brearer = httpContext.HttpContext.Request.Query["bearer"];
                var principal = jwtAuth.ValidateToken(brearer);


                var quizModel = QuizService.GetModelByGuid(principal.GameId);
                var player =await quizModel.GetPlayerById(principal.PlayerId);
                if (player != null)
                {
                    await quizModel.PlayerDisconnected(principal.PlayerId);
                }                
            }
            await base.OnDisconnectedAsync(exception);
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = this.Context.Features.Get<IHttpContextFeature>();
            var queryCollection = httpContext.HttpContext.Request.Query;

            bool success = false;

            if (queryCollection.ContainsKey("bearer"))
            {
                var brearer = httpContext.HttpContext.Request.Query["bearer"];
                var principal = jwtAuth.ValidateToken(brearer);

                var gameid = principal.GameId;
                var playerId = principal.PlayerId;

                var quizModel = QuizService.GetModelByGuid(principal.GameId);
                if (quizModel != null)
                {
                    var playerModel =await quizModel.GetPlayerById(playerId);
                    if (playerModel != null)
                    {
                        Context.Items.Add("gameId", gameid);
                        Context.Items.Add("playerId", playerId);
                                                
                        await Groups.AddToGroupAsync(Context.ConnectionId, gameid.ToString());
                        await quizModel.PlayerConnected(playerId,Context.ConnectionId);
                       
                        success = true;
                    }
                }

            }

            if (!success)
            {
                this.Context.Abort();
            }

            Debug.WriteLine("NEW CONNECTION");
            await base.OnConnectedAsync();
        }


    }
}
