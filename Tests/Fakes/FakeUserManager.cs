using ApiDPSystem.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Fakes
{
    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null)
        { }
    }
}
