using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToeGame.Services.Dto;

namespace TicTacToeGame.Api.Infrastructures
{
    public class IsUserInvolvedAuthorizationHandler : AuthorizationHandler<IsUserInvolved, GameDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsUserInvolved requirement, GameDTO resource)
        {
            ClaimsPrincipal appUser = context.User;
            if (!appUser.HasClaim(c => c.Type == "Id")) return Task.CompletedTask;

            string loggedInUserId = appUser.FindFirst("Id").Value;
            bool isLoggedInUserInvolvedInGame = resource.OwnerId == loggedInUserId
                || resource.PlayerCross == loggedInUserId
                || resource.PlayerNaught == loggedInUserId;

            if (isLoggedInUserInvolvedInGame)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
