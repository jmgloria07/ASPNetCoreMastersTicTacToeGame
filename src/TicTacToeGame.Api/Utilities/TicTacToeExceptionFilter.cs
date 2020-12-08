using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeGame.Api.Models;
using TicTacToeGame.Services.Exceptions;
using TicTacToeGame.Services.Utilities;

namespace TicTacToeGame.Api.Utilities
{
    public class TicTacToeExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<TicTacToeExceptionFilter> _logger;
        public TicTacToeExceptionFilter(ILogger<TicTacToeExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            int code = TicTacToeConstants.UNHANDLED_ERROR_CODE;

            if (context.Exception is TicTacToeServiceException appException)
            {
                _logger.LogInformation(appException.Message);
                code = appException.TicTacToeErrorCode;
            }                

            ErrorResponseApiModel error = new ErrorResponseApiModel
            {
                Message = "An error occurred.",
                ResultInfo = new ErrorResultInfo
                {
                    Code = code,
                    Message = context.Exception.Message,
                    TimeStamp = DateTime.UtcNow
                }
            };

            if (context.Exception is TicTacToeIdentityException)
                context.Result = new UnauthorizedObjectResult(error);
            else if (context.Exception is TicTacToeNotFoundException)
                context.Result = new NotFoundObjectResult(error);
            else if (context.Exception is TicTacToeLogicException)
                context.Result = new BadRequestObjectResult(error);
            else
            {
                _logger.LogError(null, context.Exception);
                context.Result = new ObjectResult(error)
                {
                    StatusCode = 500
                };
            }                
        }
    }
}
