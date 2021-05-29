using Kledex.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.Api.Extensions
{
    public static class ApiResultExceptionExtensions
    {
        public static ProblemDetails ToProblemDetails(this Exception ex)
        {
            if (ex.GetType() == typeof(ValidationException)) 
            {
                var e = (ValidationException)ex;
                return e.ToValidationProblemDetails();
            }
            var problemDetails = new ProblemDetails()
            {
                Type = "https://httpstatuses.com/500",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = ex.Message
            };
            return problemDetails;
        }

        public static ProblemDetails ToProblemDetails(this System.ApplicationException ex, string title, int statusCode) 
        {
            var problemDetails = new ProblemDetails()
            {
                Type = $"https://httpstatuses.com/{statusCode}",
                Status = statusCode,
                Title = title,
                Detail = ex.Message
            };
            return problemDetails;
        }

        public static ValidationProblemDetails ToValidationProblemDetails(this ValidationException ex)
        {
            var formatedErrors = new Dictionary<string, string[]>();
            string originalMessage = ex.Message.Replace("Validation failed: ", "").Replace("\r\n ", "");
            string[] errors = originalMessage.Split("- ");
            for (int i = 0; i < errors.Length; i++)
            {
                if (!string.IsNullOrEmpty(errors[i]))
                {
                    var keyValue = errors[i].Split("-");
                    formatedErrors.Add(keyValue[0], new string[] { keyValue[1] });
                }
            }
            var problemBackendValidations = new ValidationProblemDetails(formatedErrors)
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = "https://httpstatuses.com/422"
            };
            return problemBackendValidations;
        }
    }
}
