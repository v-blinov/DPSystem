using System;
using ApiDPSystem.Extensions;
using ApiDPSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ApiDPSystem.Filters
{
    public class ModelValidationFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _errorMessage = string.Empty;

        public ModelValidationFilterAttribute() { }

        public ModelValidationFilterAttribute(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Log.Warning("Невалидная модель");
                context.Result = new ObjectResult(
                    new ApiResponse<AuthenticationResult>
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = _errorMessage,
                        Errors = context.ModelState.GetErrorList()
                    });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}