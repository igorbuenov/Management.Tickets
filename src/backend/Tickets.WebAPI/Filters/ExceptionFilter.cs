using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tickets.WebAPI.Exceptions.Handlers;

namespace Tickets.WebAPI.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = ExceptionResponseMapper.Map(context.Exception);

            context.HttpContext.Response.StatusCode = response.StatusCode;
            context.Result = new ObjectResult(response.Body)
            {
                StatusCode = response.StatusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
