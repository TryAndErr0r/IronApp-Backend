using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronApp.Model;
using IronApp.Model.ExchangeModels;
using IronApp.UserManagement;
using System.Threading.Tasks;

namespace IronApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizRegistrationController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;
        QuizService quizService;
        public QuizRegistrationController(QuizService quizService, IJwtAuth jwtAuth)
        {
            this.quizService = quizService; 
            this.jwtAuth = jwtAuth;

        }

        [HttpPost]
        [Route("newgame")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<GameCreated> CreateNewQuiz()
        {
            try
            {
                var nQuizId = quizService.CreateNewQuiz();

                return new ActionResult<GameCreated>(new GameCreated() { GameId = nQuizId });
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("joingame")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NewGameMessage>> JoinQuiz([FromBody] JoinGameMessage newgamejoin)
        {
            try
            {                
                var Player = await quizService.NewPlayer(newgamejoin.gameId, newgamejoin.credentials);

                var Token = jwtAuth.Authentication(newgamejoin.gameId, Player);
                return new ActionResult<NewGameMessage>(new NewGameMessage() { GameId = newgamejoin.gameId, UserName = Player.Name, PlayerId = Player.Id, BearerToken = Token });
            }
            catch
            {
                return NotFound();
            }
        }

      
    }
}
