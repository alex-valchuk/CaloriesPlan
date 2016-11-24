using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Services.Impl;
using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DTO.In;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.UTL;
using CaloriesPlan.UTL.Wrappers;

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
        public void RegisterUser_EmptyRegisterDto_ArgumentNullExceptionThrown()
        {
            //act
            this.accountService.SignUp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Register data not specified correctly")]
        public void RegisterUser_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            registerDto.UserName = null;

            //act
            this.accountService.SignUp(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Register data not specified correctly")]
        public void RegisterUser_EmptyPassword_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            registerDto.Password = null;

            //act
            this.accountService.SignUp(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyInconsistencyException), "Password does not match password confirmation")]
        public void RegisterUser_PasswordDoesNotEqualToConfirmPassword_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            registerDto.Password += "2";

            //act
            this.accountService.SignUp(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(RegistrationException), "Problem while creating the user")]
        public void RegisterUser_NotSuccededUserRegistration_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            var result = this.GetNotSuccededRegistrationResult();

            this.userDaoMock.Setup(d => d.NewUserInstance()).Returns(Mock.Of<IUser>());
            this.userDaoMock.Setup(d => d.CreateUser(It.IsAny<IUser>(), registerDto.Password)).Returns(result);

            //act
            this.accountService.SignUp(registerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(RegistrationException), "Problem while assigning user to role")]
        public void RegisterUser_NotSuccededRoleRegistration_ArgumentNullExceptionThrown()
        {
            //arrange
            var registerDto = this.GetValidRegisterDto();
            var userResult = this.GetSuccededRegistrationResult();
            var roleResult = this.GetNotSuccededRegistrationResult();

            this.userDaoMock.Setup(d => d.NewUserInstance()).Returns(Mock.Of<IUser>());
            this.userDaoMock.Setup(d => d.CreateUser(It.IsAny<IUser>(), registerDto.Password)).Returns(userResult);
            this.userDaoMock.Setup(d => d.AddUserRole(It.IsAny<IUser>(), "User")).Returns(roleResult);

            //act
            this.accountService.SignUp(registerDto);
        }

        [TestMethod]
        public void GetAccounts_AccountsExist_DtoAccountsReturned()
        {
            //arrange
            var dbUsers = new List<IUser>
            {
                Mock.Of<IUser>()
            };
            this.userDaoMock.Setup(d => d.GetUsers()).Returns(dbUsers);

            //act
            var accounts = this.accountService.GetAccounts();

            //assert
            Assert.IsNotNull(accounts);
        }

        [TestMethod]
        public void GetAccounts_AccountsNotExist_NullReturned()
        {
            //act
            var accounts = this.accountService.GetAccounts();

            //assert
            Assert.IsNull(accounts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public void GetAccount_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //act
            this.accountService.GetAccount(null);
        }

        [TestMethod]
        public void GetAccount_AccountDoesNotExist_NullReturned()
        {
            //act
            var account = this.accountService.GetAccount("Alex");

            //assert
            Assert.IsNull(account);
        }

        [TestMethod]
        public void GetAccount_AccountExistsWithoutRoles_AccountReturned()
        {
            //arrange
            this.userDaoMock.Setup(d => d.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());

            //act
            var account = this.accountService.GetAccount("Alex");

            //assert
            Assert.IsNotNull(account);
        }

        [TestMethod]
        public void GetAccount_AccountExistsWithRoles_AccountWithRolesReturned()
        {
            //arrange
            var dbRoles = new List<IRole>
            {
                Mock.Of<IRole>()
            };

            this.userDaoMock.Setup(d => d.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());
            this.userDaoMock.Setup(d => d.GetUserRoles(It.IsAny<IUser>())).Returns(dbRoles);

            //act
            var account = this.accountService.GetAccount("Alex");

            //assert
            Assert.IsNotNull(account.UserRoles);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public void UpdateAccount_UserNameNotSpecified_ArgumentNullExceptionThrown()
        {
            //act
            this.accountService.UpdateAccount(null, It.IsAny<InAccountDto>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Account not specified")]
        public void UpdateAccount_AccountDtoNotSpecified_ArgumentNullExceptionThrown()
        {
            //act
            this.accountService.UpdateAccount("Alex", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "DailyCaloriesLimit not specified")]
        public void UpdateAccount_DailyCaloriesLimitNotSpecified_ArgumentNullExceptionThrown()
        {
            //act
            this.accountService.UpdateAccount("Alex", Mock.Of<InAccountDto>());
        }

        [TestMethod]
        [ExpectedException(typeof(AccountDoesNotExistException), "Account does not exist")]
        public void UpdateAccount_AccountDoesNotExist_ArgumentNullExceptionThrown()
        {
            //arrange
            var accountDto = Mock.Of<InAccountDto>();
            accountDto.DailyCaloriesLimit = 20;

            //act
            this.accountService.UpdateAccount("Alex", accountDto);
        }

        [TestMethod]
        public void UpdateAccount_CorrectInput_AccountUpdated()
        {
            //arrange
            var accountDto = Mock.Of<InAccountDto>();
            accountDto.DailyCaloriesLimit = 20;

            var dbUser = Mock.Of<IUser>();
            this.userDaoMock.Setup(d => d.GetUserByName(It.IsAny<string>())).Returns(dbUser);

            //act
            this.accountService.UpdateAccount("Alex", accountDto);

            //assert
            this.userDaoMock.Verify(d => d.Update(dbUser));
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

        private IAccountRegistrationResult GetNotSuccededRegistrationResult()
        {
            var resultMock = new Mock<IAccountRegistrationResult>();
            resultMock.Setup(r => r.Succeeded).Returns(false);

            return resultMock.Object;
        }

        private IAccountRegistrationResult GetSuccededRegistrationResult()
        {
            var resultMock = new Mock<IAccountRegistrationResult>();
            resultMock.Setup(r => r.Succeeded).Returns(true);

            return resultMock.Object;
        }
    }
}
