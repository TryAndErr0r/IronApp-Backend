using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronApp.Model;
using IronApp.Model.ExchangeModels;
using IronApp.Model.ExchangeModels.FromClient;
using IronApp.Model.ExchangeModels.ToClient;
using IronApp.UserManagement;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IronApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly IJwtAuth jwtAuth;

        QuizService quizService;
        public QuizController(QuizService quizService, IJwtAuth jwtAuth)
        {
            this.quizService = quizService;
            this.jwtAuth = jwtAuth;
        }

        [HttpPost]
        [Route("getplayer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerDto[]>> GetPlayer()
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);

                var gameModel = quizService.GetModelByGuid(ident.GameId);
                var player = gameModel.GetPlayerById(ident.PlayerId);
                //Player is not null if player is part of game!
                if (player != null)
                {
                    var allPlayer = await gameModel.GetAllPlayers();
                    return new ActionResult<PlayerDto[]>(allPlayer.Select(x => x.ToDto()).ToArray());
                }
            }
            catch
            {
            }
            return NotFound();
        }

        [HttpPost]
        [Route("getquestions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<QuestionDto[]>> GetQuestion()
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);
                var gameModel = quizService.GetModelByGuid(ident.GameId);
                var player = gameModel.GetPlayerById(ident.PlayerId);
                //Player is not null if player is part of game!
                if (player != null)
                {
                    var allQuestions = await gameModel.GetAllQuestion();
                    return new ActionResult<QuestionDto[]>(allQuestions.Select(x => x.ToDto()).ToArray());
                }
            }
            catch
            {
            }
            return NotFound();
        }

        [HttpPost]
        [Route("sendquestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendQuestion([FromBody] NewQuestionMessage requestObject)
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);
                var gameModel = quizService.GetModelByGuid(ident.GameId);
                var player = await gameModel.GetPlayerById(ident.PlayerId);

                //Player is not null if player is part of game!
                if (player != null)
                {
                    await gameModel.NewQuestion(player, requestObject);
                    return Ok();
                }
            }
            catch
            {
            }
            return NotFound();
        }

        [HttpPost]
        [Route("getanswers")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AnswerDto[]>> GetAnswers()
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);
                var gameModel = quizService.GetModelByGuid(ident.GameId);

                var player = await gameModel.GetPlayerById(ident.PlayerId);
                //Player is not null if player is part of game!
                if (player != null)
                {
                    var allAnswers = await gameModel.GetAllAnswer();
                    return new ActionResult<AnswerDto[]>(allAnswers.Select(x => x.ToDto()).ToArray());
                }
            }
            catch
            {
            }
            return NotFound();
        }

        [HttpPost]
        [Route("sendanswer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendAnswer([FromBody] NewAnswerMessage requestObject)
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);
                var gameModel = quizService.GetModelByGuid(ident.GameId);
                var player = await gameModel.GetPlayerById(ident.PlayerId);

                //Player is not null if player is part of game!
                if (player != null)
                {
                    await gameModel.NewAnswer(player, requestObject);
                    return Ok();
                }
            }
            catch
            {
            }
            return NotFound();
        }

        [HttpPost]
        [Route("deletequestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteQuestion([FromBody] DeleteQuestion requestObject)
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);
                var gameModel = quizService.GetModelByGuid(ident.GameId);
                var player = await gameModel.GetPlayerById(ident.PlayerId);

                //Player is not null if player is part of game!
                if (player != null)
                {
                    await gameModel.DeleteQuestion(player, requestObject);
                    return Ok();
                }
            }
            catch
            {
            }
            return NotFound();
        }

        [HttpPost]
        [Route("editquestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditQuestion([FromBody] EditQuestion requestObject)
        {
            try
            {
                var ident = jwtAuth.PrincipalToIdent(HttpContext.User.Identity as ClaimsIdentity);
                var gameModel = quizService.GetModelByGuid(ident.GameId);
                var player = await gameModel.GetPlayerById(ident.PlayerId);

                //Player is not null if player is part of game!
                if (player != null)
                {
                    await gameModel.EditQuestion(player, requestObject);
                    return Ok();
                }
            }
            catch
            {
            }
            return NotFound();
        }
    }
}
