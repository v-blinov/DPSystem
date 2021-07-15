using NUnit.Framework;
using ApiDPSystem;
using ApiDPSystem.Controllers;
using ApiDPSystem.Services;
using Moq;
using ApiDPSystem.Records;
using ApiDPSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Tests.Fakes;
using Castle.Core.Configuration;

namespace Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private Mock<FakeUserManager> mockUsrMng;
        private Mock<FakeSignInManager> mockSignInMng;
        private AccountService _accountSrv;

        //[SetUp]
        //public void Setup()
        //{
        //    mockUsrMng = new Mock<FakeUserManager>();
        //    mockSignInMng = new Mock<FakeSignInManager>();
        //    _accountSrv = new AccountService(mockUsrMng.Object, mockSignInMng.Object);

        //    //    mockAccServ = new Mock<AccountService>(mockUsrMng.Object, mockSignInMng.Object);
        //    //    mockEmailServ = new Mock<EmailService>();
        //    //    _accountController = new AccountController(mockAccServ.Object, mockEmailServ.Object);
        //}


        //[Test]
        //public async Task AccountService_CheckIfEmailConfirmedAsync_IfConfirmedTrue_Test()
        //{
        //    //Arrange
        //    mockUsrMng.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
        //    mockUsrMng.Setup(p => p.IsEmailConfirmedAsync(It.IsAny<User>())).ReturnsAsync(true);
        //    LogInRecord logInRec = new LogInRecord() { Email = "", Password = "" };

        //    //Act
        //    var result = await _accountSrv.CheckIfEmailConfirmedAsync(logInRec.Email);

        //    //Assert
        //    Assert.IsInstanceOf<IdentityResult>(result);
        //    Assert.AreEqual(true, result.Succeeded);
        //}
        //[Test]
        //public async Task AccountService_CheckIfEmailConfirmedAsync_IfConfirmedFalse_Test()
        //{
        //    //Arrange
        //    mockUsrMng.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
        //    mockUsrMng.Setup(p => p.IsEmailConfirmedAsync(It.IsAny<User>())).ReturnsAsync(false);
        //    LogInRecord logInRec = new LogInRecord() { Email = "", Password = "" };

        //    //Act
        //    var result = await _accountSrv.CheckIfEmailConfirmedAsync(logInRec.Email);

        //    //Assert
        //    //Assert.IsInstanceOf<IdentityResult>(result);
        //    Assert.AreEqual(false, result.Succeeded);
        //}
        //[Test]
        //public async Task AccountService_CheckIfEmailConfirmedAsync_IfEmailIsNotFound_Test()
        //{
        //    //Arrange
        //    mockUsrMng.Setup(p => p.FindByEmailAsync(It.IsAny<string>()));
        //    LogInRecord logInRec = new LogInRecord() { Email = "", Password = "" };

        //    //Act
        //    var result = await _accountSrv.CheckIfEmailConfirmedAsync(logInRec.Email);

        //    //Assert
        //    Assert.IsInstanceOf<IdentityResult>(result);
        //    Assert.AreEqual(false, result.Succeeded);
        //}


        //[Test]
        //public void AccountService_ConfirmEmail_IfEmailAndTokenCorrect_Test()
        //{
        //    //Arrange
        //    mockUsrMng.Setup(p => p.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
        //    mockUsrMng.Setup(p => p.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        //    //Act
        //    var result = _accountSrv.ConfirmEmail("", "");

        //    //Assert
        //    //Assert.IsInstanceOf<bool>(result);
        //    Assert.IsTrue(result.Result);
        //}
        //[Test]
        //public void AccountService_ConfirmEmail_IfEmailOrTokenIncorrect_Test()
        //{
        //    //Arrange
        //    mockUsrMng.Setup(p => p.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
        //    mockUsrMng.Setup(p => p.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

        //    //Act
        //    var result = _accountSrv.ConfirmEmail("", "");

        //    //Assert
        //    //Assert.IsInstanceOf<bool>(result);
        //    Assert.IsFalse(result.Result);
        //}
    }
}