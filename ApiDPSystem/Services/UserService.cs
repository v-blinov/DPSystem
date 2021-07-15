﻿using ApiDPSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleService _roleService;

        public UserService(UserManager<User> userManager, RoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task<User> GetUser(string userId) =>
            await _userManager.FindByIdAsync(userId);

        public async Task<IdentityResult> AddRoleToUser(User user, string roleName)
        {
            var role = await _roleService.FindRoleAsync(roleName);

            if (role == null)
                await _roleService.AddRoleAsync(roleName);

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return await _userManager.DeleteAsync(user);
        }

        public async Task<string> GetRole(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.FirstOrDefault();
        }
    }
}
