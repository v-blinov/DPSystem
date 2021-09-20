namespace DPSystem.Tests
{
    public class AccountTests
    {
        //private readonly Fixture _fixture = new();

        //private const string TestConnectionString = "Server=mssql,8083;Database=Identity.Tests;User=sa;Password=Qwerty123!;";
        //private readonly DbContextOptions<IdentityContext> _testIdentityContextOptions =
        //    new DbContextOptionsBuilder<IdentityContext>().UseSqlServer(TestConnectionString).Options;

        //private void CreateDatabase()
        //{
        //    using IdentityContext context = new(_testIdentityContextOptions);

        //    context.Database.EnsureDeleted();
        //    context.Database.EnsureCreated();
        //}

        //[Fact]
        //public void Standard_Login_to_user_account()
        //{
        //    const string userEmail = "testUser@mail.ru";
        //    const string userPassword = "testUserPassword!123";
        //    var testUser = new User
        //    {
        //        UserName = "testUserName",
        //        Email = userEmail,
        //        FirstName = "testFirstName",
        //        LastName = "testLastName"
        //    };

        //    //Arrange


        //    var mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
        //                                                      new Mock<IOptions<IdentityOptions>>().Object,
        //                                                      new Mock<IPasswordHasher<User>>().Object,
        //                                                      new IUserValidator<User>[0],
        //                                                      new IPasswordValidator<User>[0],
        //                                                      new Mock<ILookupNormalizer>().Object,
        //                                                      new Mock<IdentityErrorDescriber>().Object,
        //                                                      null,
        //                                                      null);

        //    //var store = new Mock<IUserStore<User>>();
        //    //store.Setup(p => p.FindByIdAsync("123", CancellationToken.None)).ReturnsAsync(testUser);
        //    //var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        //    mockUserManager.Setup(p => p.FindByEmailAsync(userEmail)).ReturnsAsync(testUser);


        //    //var mockContextAccessor = new Mock<IHttpContextAccessor>();
        //    //var mockUserPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        //    //var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, mockContextAccessor.Object, mockUserPrincipalFactory.Object, null, null, null);

        //    var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
        //                                                          new HttpContextAccessor(),
        //                                                          new Mock<IUserClaimsPrincipalFactory<User>>().Object,
        //                                                          new Mock<IOptions<IdentityOptions>>().Object,
        //                                                          null);
        //    mockSignInManager.Setup(p => p.PasswordSignInAsync(userEmail, userPassword, false, false)).ReturnsAsync(SignInResult.Success);

        //    var emailService = new Mock<EmailService>();

        //    var userService = new Mock<UserService>();
        //    var configuration = new Mock<IConfiguration>();
        //    var tokenValidationParameters = new Mock<TokenValidationParameters>();
        //    var accountRepository = new Mock<AccountRepository>();

        //    //Act
        //    var sut = new AccountService(mockUserManager.Object, 
        //                                 mockSignInManager.Object, 
        //                                 userService.Object, 
        //                                 emailService.Object,
        //                                 configuration.Object, 
        //                                 tokenValidationParameters.Object, 
        //                                 accountRepository.Object);

        //    var testLoginResult = sut.LogInAsync(new LogInRecord { Email = userEmail, Password = userPassword });

        //    //Assert
        //    Assert.Equal(IdentityResult.Success, testLoginResult.Result);
        //}
    }
}