
using IronApp.Model.QuizEntityModel;
using System;
using System.Security.Claims;

namespace IronApp.UserManagement
{
    public interface IJwtAuth
    {
        UserIdent ValidateToken(string jwtToken);
        string Authentication(Guid gameid, Player player);

        UserIdent PrincipalToIdent(ClaimsIdentity identity);
    }
}
