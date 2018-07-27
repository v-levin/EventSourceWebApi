using EventSourceWebApi.Contracts.Responses;
using FluentValidation.Results;
using System.Linq;

namespace EventSourceWebApi.Contracts.Extensions
{
    public static class ResponseExtensions
    {
        public static Response ToResponse(this ValidationResult result)
        {
            return new Response()
            {
                Result = result.IsValid,
                Errors = result.Errors
                               .Select(x => new ResponseError { Name = x.PropertyName, Error = x.ErrorMessage })
                               .ToList()
            };
        }
    }
}
