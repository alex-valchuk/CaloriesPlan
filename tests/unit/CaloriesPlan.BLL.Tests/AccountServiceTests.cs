using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using CaloriesPlan.UTL.Config.Abstractions;
using CaloriesPlan.UTL.Wrappers;
using CaloriesPlan.DTO.In;
using CaloriesPlan.DAL.Dao.Abstractions;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.BLL.Services.Abstractions;
using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Exceptions;

namespace CaloriesPlan.BLL.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        private Mock<IConfigProvider> configProviderMock;
        private Mock<IUserDao> userDaoMock;

        private IAccountService accountService;

        [TestInitialize]
        public void Setup()
        {
            this.configProviderMock = new Mock<IConfigProvider>();
            this.userDaoMock = new Mock<IUserDao>();
            this.accountService = new AccountService(configProviderMock.Object, userDaoMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Register data not specified correctly")]
        public async Task SignUpAsync_EmptyRegisterDto_ArgumentNullExceptionThrown()
        {
            //act
            await this.accountService.SignUpAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Register data not specified correctly")]
        public async Task SignUpAsync_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            registerDto.UserName = null;

            //act
            await this.accountService.SignUpAsync(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Register data not specified correctly")]
        public async Task SignUpAsync_EmptyPassword_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            registerDto.Password = null;

            //act
            await this.accountService.SignUpAsync(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPasswordConfirmationException), "Password does not match password confirmation")]
        public async Task SignUpAsync_PasswordDoesNotEqualToConfirmPassword_InvalidPasswordConfirmationExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            registerDto.Password += "2";

            //act
            await this.accountService.SignUpAsync(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(RegistrationException), "Problem while creating the user")]
        public async Task SignUpAsync_NotSuccededUserRegistration_RegistrationExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            var resultTask = this.GetNotSuccededRegistrationResultTask();

            this.userDaoMock.Setup(d => d.NewUserInstance()).Returns(Mock.Of<IUser>());
            this.userDaoMock.Setup(d => d.CreateUserAsync(It.IsAny<IUser>(), registerDto.Password)).Returns(resultTask);

            //act
            await this.accountService.SignUpAsync(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(RegistrationException), "Problem while assigning user to role")]
        public async Task SignUpAsync_NotSuccededRoleRegistration_RegistrationExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            var userResultTask = this.GetSuccededRegistrationResultTask();
            var roleResultTask = this.GetNotSuccededRegistrationResultTask();

            this.userDaoMock.Setup(d => d.NewUserInstance()).Returns(Mock.Of<IUser>());
            this.userDaoMock.Setup(d => d.CreateUserAsync(It.IsAny<IUser>(), registerDto.Password)).Returns(userResultTask);
            this.userDaoMock.Setup(d => d.AddUserRoleAsync(It.IsAny<IUser>(), "User")).Returns(roleResultTask);

            //act
            await this.accountService.SignUpAsync(registerDto);
        }

        [TestMethod]
        public async Task GetAccountsAsync_AccountsExist_DtoAccountsReturned()
        {
            //arrange
            IList<IUser> dbUsers = new List<IUser>
            {
                Mock.Of<IUser>()
            };
                
            var dbUsersTask = Task.FromResult(dbUsers);

            this.userDaoMock.Setup(d => d.GetUsersAsync()).Returns(dbUsersTask);

            //act
            var accounts = await this.accountService.GetAccountsAsync();

            //assert
            Assert.IsNotNull(accounts);
        }

        [TestMethod]
        public async Task GetAccountsAsync_AccountsNotExist_NullReturned()
        {
            //act
            var accounts = await this.accountService.GetAccountsAsync();

            //assert
            Assert.IsNull(accounts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public async Task GetAccountAsync_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //act
            await this.accountService.GetAccountAsync(null);
        }

        [TestMethod]
        public async Task GetAccountAsync_AccountDoesNotExist_NullReturned()
        {
            //act
            var account = await this.accountService.GetAccountAsync("Alex");

            //assert
            Assert.IsNull(account);
        }

        [TestMethod]
        public async Task GetAccountAsync_AccountExistsWithoutRoles_AccountReturned()
        {
            //arrange
            var task = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(d => d.GetUserByNameAsync(It.IsAny<string>())).Returns(task);

            //act
            var account = await this.accountService.GetAccountAsync("Alex");

            //assert
            Assert.IsNotNull(account);
        }

        [TestMethod]
        public async Task GetAccountAsync_AccountExistsWithRoles_AccountWithRolesReturned()
        {
            //arrange
            IList<IRole> dbRoles = new List<IRole>
            {
                Mock.Of<IRole>()
            };

            var userTask = Task.FromResult(Mock.Of<IUser>());
            var rolesTask = Task.FromResult(dbRoles);

            this.userDaoMock.Setup(d => d.GetUserByNameAsync(It.IsAny<string>())).Returns(userTask);
            this.userDaoMock.Setup(d => d.GetUserRolesAsync(It.IsAny<IUser>())).Returns(rolesTask);

            //act
            var account = await this.accountService.GetAccountAsync("Alex");

            //assert
            Assert.IsNotNull(account.UserRoles);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public async Task UpdateAccountAsync_UserNameNotSpecified_ArgumentNullExceptionThrown()
        {
            //act
            await this.accountService.UpdateAccountAsync(null, It.IsAny<InAccountDto>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Account not specified")]
        public async Task UpdateAccountAsync_AccountDtoNotSpecified_ArgumentNullExceptionThrown()
        {
            //act
            await this.accountService.UpdateAccountAsync("Alex", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "DailyCaloriesLimit not specified")]
        public async Task UpdateAccountAsync_DailyCaloriesLimitNotSpecified_ArgumentNullExceptionThrown()
        {
            //act
            await this.accountService.UpdateAccountAsync("Alex", Mock.Of<InAccountDto>());
        }

        [TestMethod]
        [ExpectedException(typeof(AccountDoesNotExistException), "Account does not exist")]
        public async Task UpdateAccountAsync_AccountDoesNotExist_AccountDoesNotExistExceptionThrown()
        {
            //arrange
            var accountDto = Mock.Of<InAccountDto>();
            accountDto.DailyCaloriesLimit = 20;

            //act
            await this.accountService.UpdateAccountAsync("Alex", accountDto);
        }

        [TestMethod]
        public async Task UpdateAccountAsync_CorrectInput_AccountUpdated()
        {
            //arrange
            var accountDto = Mock.Of<InAccountDto>();
            accountDto.DailyCaloriesLimit = 20;

            var dbUser = Mock.Of<IUser>();
            var accountTask = Task.FromResult(dbUser);

            this.userDaoMock.Setup(d => d.GetUserByNameAsync(It.IsAny<string>())).Returns(accountTask);

            //act
            await this.accountService.UpdateAccountAsync("Alex", accountDto);

            //assert
            this.userDaoMock.Verify(d => d.UpdateAsync(dbUser));
        }

        private InSignUpDto GetValidRegisterDto()
        {
            return new InSignUpDto
            {
                UserName = "Alex",
                Password = "123456",
                ConfirmPassword = "123456"
            };
        }

        private Task<IAccountRegistrationResult> GetNotSuccededRegistrationResultTask()
        {
            var resultMock = new Mock<IAccountRegistrationResult>();
            resultMock.Setup(r => r.Succeeded).Returns(false);

            return Task.FromResult(resultMock.Object);
        }

        private Task<IAccountRegistrationResult> GetSuccededRegistrationResultTask()
        {
            var resultMock = new Mock<IAccountRegistrationResult>();
            resultMock.Setup(r => r.Succeeded).Returns(true);

            return Task.FromResult(resultMock.Object);
        }
    }
}
