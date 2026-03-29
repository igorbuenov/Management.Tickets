using System.Net;
using Tickets.Exceptions.ExceptionBase;
using Tickets.WebAPI.Models.Error.Response;

namespace Tickets.WebAPI.Exceptions.Handlers
{
    public static class ExceptionResponseMapper
    {
        public static ExceptionResponse Map(Exception exception)
        {

            if (exception is TicketsException ticketsException)
            {
                return new ExceptionResponse
                {
                    StatusCode = (int)ticketsException.StatusCode,
                    Body = new ErrorResponseModel(ticketsException.Errors)
                };
            }

            return new ExceptionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = new ErrorResponseModel(new List<string> { "An unknown error occurred." })
            };

        }
    }
}
