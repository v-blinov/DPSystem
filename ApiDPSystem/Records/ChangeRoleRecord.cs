using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ApiDPSystem.Records
{
    public record ChangeRoleRecord
    {
        public ChangeRoleRecord()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }

        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}