using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Tickets.Exceptions.ExceptionBase;
using Tickets.WebAPI.Models.Error.Response;

namespace Tickets.WebAPI.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is TicketsException)
                HandleProjectException(context);
            else
                ThrowUnknowException(context);
        }

        private static void HandleProjectException(ExceptionContext context)
        {
            if(context.Exception is ErrorOnValidationException)
            {
                var exception = context.Exception as ErrorOnValidationException;

                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ErrorResponseModel(exception.Errors));
            }
            else if (context.Exception is AuthValidationException authValidationException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ErrorResponseModel(authValidationException.Errors));
            } 
            else if(context.Exception is NotFoundException notFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Result = new NotFoundObjectResult(new ErrorResponseModel(notFoundException.Errors));
            }
            else if (context.Exception is UnauthorizedException unauthorizedException)
            {

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new UnauthorizedObjectResult(new ErrorResponseModel(unauthorizedException.Errors));
            }
            else if (context.Exception is BusinessRuleException businessRuleException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new ObjectResult(new ErrorResponseModel(businessRuleException.Errors));
            }
            else if (context.Exception is ForbiddenException forbiddenException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Result = new ObjectResult(new ErrorResponseModel(forbiddenException.Errors));
            }
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult("An unknown error occurred.");
        }


    }
}
