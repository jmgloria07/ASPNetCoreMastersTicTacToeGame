using DryIoc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeGame.Api.Utilities;
using TicTacToeGame.Repositories;
using TicTacToeGame.Services;
using TicTacToeGame.Services.Interfaces.Helpers;
using TicTacToeGame.Services.Interfaces.Services;
using TicTacToeGame.Services.Utilities;

namespace TicTacToeGame.Api
{
	public class CompositionRoot
	{
		public CompositionRoot(IRegistrator registrator)
		{
			registrator.Register<IUserService, UserService>();
			registrator.Register<IGameService, GameService>();
			registrator.Register<IEmailService, EmailService>();
			registrator.Register<ISmtpHelper, SmtpHelper>();
			registrator.Register<IGameRepository, GameRepository>();
			registrator.Register<IAuthorizationHandler, IsUserInvolvedAuthorizationHandler>();
			registrator.RegisterInstance(new TicTacToeHelper());
		}
	}
}
