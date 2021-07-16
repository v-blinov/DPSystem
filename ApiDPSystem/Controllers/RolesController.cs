using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public class RolesController : Controller
    {
        private readonly RoleService _roleService;
        private readonly UserService _userService;

        public RolesController(RoleService roleService, UserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [HttpGet]
        public ApiResponse GetAllRoles()
        {
            try
            {
                var roles = _roleService.GetAllRoles();

                return new ApiResponse<List<string>>()
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Content = roles
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse> CreateRoleAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.Error("Попытка добавить роль с пустым именем");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при добавлении роли: имя роли пустое или не передано."
                };
            }

            try
            {
                var result = await _roleService.AddRoleAsync(name);

                if (result.Succeeded)
                    return new ApiResponse<List<string>>()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                    };

                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при добавлении роли.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpDelete]
        public async Task<ApiResponse> DeleteRoleAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Log.Error("Переданный парметр id пустой или равен null.");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при добавлении роли: имя роли пустое или не передано."
                };
            }

            try
            {
                var result = await _roleService.DeleteRoleAsync(id);

                if (result.Succeeded)
                    return new ApiResponse<List<string>>()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                    };
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Ошибка при удалении роли.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse> AddRoleForUserAsync(string userId, string role)
        {
            if (String.IsNullOrEmpty(userId))
            {
                Log.Error("Переданный парметр userId пустой или равен null.");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при добавлении роли для пользователя: Идентификатор пользователя пустой или не указан."
                };
            }
            if (String.IsNullOrEmpty(role))
            {
                Log.Error("Попытка добавить пользователю роль с пустым именем");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при добавлении роли для пользователя: имя роли пустое или не передано."
                };
            }

            try
            {
                var user = await _userService.GetUserById(userId);
                if (user == null)
                {
                    Log.Error($"Пользователь с id = {userId} не найден.");
                    return new ApiResponse<AuthenticationResult>()
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Пользователь не найден."
                    };
                }

                var result = await _userService.AddRoleToUser(user, role);
                if (result.Succeeded)
                    return new ApiResponse<List<string>>()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                    };

                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при добавлении пользователю роли.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }
    }
}
