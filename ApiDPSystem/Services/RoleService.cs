using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ApiDPSystem.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<string> GetAllRoles()
        {
            return _roleManager.Roles.Select(p => p.Name).ToList();
        }

        public async Task<IdentityResult> AddRoleAsync(string name)
        {
            return await _roleManager.CreateAsync(new IdentityRole(name));
        }

        public async Task<IdentityRole> FindRoleAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
                return await _roleManager.DeleteAsync(role);

            return IdentityResult.Failed(new IdentityError
            {
                Description = $"Роль {roleName} не найдена."
            });
        }
    }
}