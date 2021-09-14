using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiDPSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ApiDPSystem.Filters
{
    public class FileSendParamsValidationFilterAttribute : Attribute, IActionFilter
    {
        private readonly List<string> _availableExtensions = new() { ".json", ".xml", ".yaml", ".csv" };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var file = (IFormFile)context.ActionArguments.SingleOrDefault(p => p.Value is IFormFile).Value;

            if (file is null || file.Length == 0)
            {
                Log.Error("Файл не отправлен, или он пустой");
                context.Result = new ObjectResult(
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Файл не отправлен, или он пустой."
                    });
                return;
            }

            if (!_availableExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                Log.Error($"Отправлен неподдерживаемый формат файла {Path.GetExtension(file.FileName)}");
                context.Result = new ObjectResult(
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = $"Неподдерживаемый формат файла {Path.GetExtension(file.FileName)}"
                    });
                return;
            }

            var dealer = (string)context.ActionArguments.SingleOrDefault(p => p.Key == "dealer").Value;

            if (string.IsNullOrEmpty(dealer))
            {
                Log.Error("Имя диллера пустое или не отправлено.");
                context.Result = new ObjectResult(
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Имя диллера пустое или не отправлено."
                    });
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}