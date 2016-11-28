using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Security;

using Newtonsoft.Json;

using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.UTL;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.BLL.Services.Impl.Base;
using CaloriesPlan.DTO.In;

using Models = CaloriesPlan.DAL.DataModel.Abstractions;
using System.Net;

namespace CaloriesPlan.BLL.Services.Impl
{
    public class AccountService : ServiceBase, IAccountService, IOAuthService
    {
        private readonly IConfigProvider configProvider;
        private readonly IUserDao userDao;

        public AccountService(IConfigProvider configProvider, IUserDao userDao)
        {
            this.configProvider = configProvider;
            this.userDao = userDao;
        }

        public void SignUp(InSignUpDto signUpDto)
        {
            if (signUpDto == null ||
                string.IsNullOrEmpty(signUpDto.UserName) ||
                string.IsNullOrEmpty(signUpDto.Password))
                throw new ArgumentNullException("Register data");

            if (signUpDto.Password != signUpDto.ConfirmPassword)
                throw new PropertyInconsistencyException("Password", "Password does not match password confirmation");


            var defaultCaloriesLimit = this.configProvider.GetDefaultCaloriesLimit();

            var user = this.userDao.NewUserInstance();
            user.UserName = signUpDto.UserName;
            user.DailyCaloriesLimit =
                defaultCaloriesLimit > 0
                    ? defaultCaloriesLimit
                    : 50;//if not configured


            var userRegistrationResult = this.userDao.CreateUser(user, signUpDto.Password);
            if (userRegistrationResult.Succeeded == false)
                throw new RegistrationException(userRegistrationResult);

            var roleRegistrationResult = this.userDao.AddUserRole(user, "User");
            if (roleRegistrationResult.Succeeded == false)
                throw new RegistrationException(roleRegistrationResult);
        }

        public async Task<AuthenticationTicket> SignIn(string userName, string password, string authType)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Password");

            if (string.IsNullOrEmpty(authType))
                throw new ArgumentNullException("Authentication Type");

            var user = await this.userDao.GetUserByCredentials(userName, password);
            if (user == null)
                return null;

            var claimsIdentity = await this.userDao.CreateIdentity(user, authType);

            var roles = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            var roleNames = JsonConvert.SerializeObject(roles.Select(x => x.Value));

            var authProperties = this.CreateAuthProperties(user.UserName, roleNames);
            var authTicket = new AuthenticationTicket(claimsIdentity, authProperties);

            return authTicket;
        }

        public IList<OutAccountDto> GetAccounts()
        {
            var dbUsers = this.userDao.GetUsers();
            var dtoUsers = this.ConvertToOutAccountDtoList(dbUsers);

            return dtoUsers;
        }

        public OutAccountDto GetAccount(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                return null;


            var dto = this.ConvertToOutAccountDto(user);

            var dbUserRoles = this.userDao.GetUserRoles(user);
            if (dbUserRoles != null)
            {
                dto.UserRoles = this.ConvertToOutUserRoleDtoList(dbUserRoles);
            }

            return dto;
        }

        public void UpdateAccount(string userName, InAccountDto accountDto)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (accountDto == null)
                throw new ArgumentNullException("Account");

            if (accountDto.DailyCaloriesLimit == null)
                throw new ArgumentNullException("DailyCaloriesLimit");


            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            user.DailyCaloriesLimit = accountDto.DailyCaloriesLimit.Value;

            this.userDao.Update(user);
        }

        public void DeleteAccount(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();


            this.userDao.Delete(user);
        }

        public IList<OutUserRoleDto> GetUserRoles(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var dbRoles = this.userDao.GetUserRoles(user);
            if (dbRoles == null)
                return null;


            var dtoRoles = this.ConvertToOutUserRoleDtoList(dbRoles);
            return dtoRoles;
        }

        public IList<OutUserRoleDto> GetNotUserRoles(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var dbRoles = this.userDao.GetNotUserRoles(user);
            if (dbRoles == null)
                return null;


            var dtoRoles = this.ConvertToOutUserRoleDtoList(dbRoles);
            return dtoRoles;
        }

        public void AddUserRole(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName");

            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();


            this.userDao.AddUserRole(user, roleName);
        }

        public void DeleteUserRole(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName");

            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();


            this.userDao.DeleteUserRole(user, roleName);
        }

        public ICollection<OutShortUserInfoDto> GetSubscribers(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = this.userDao.GetUserByName(userName);
            var users = this.userDao.GetSubscribers(user);
            var subscribers = this.ConvertToOutShortUserInfoDtoList(users);

            return subscribers;
        }

        private AuthenticationProperties CreateAuthProperties(string userName, string roleNames)
        {
            var data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "roles", roleNames }
            };

            return new AuthenticationProperties(data);
        }

        private IList<OutAccountDto> ConvertToOutAccountDtoList(IList<Models.IUser> models)
        {
            if (models == null ||
                models.Count == 0)
                return null;

            var dtoList = new List<OutAccountDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToOutAccountDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        private OutAccountDto ConvertToOutAccountDto(Models.IUser model)
        {
            if (model == null)
                return null;

            var dto = new OutAccountDto
            {
                UserName = model.UserName,
                DailyCaloriesLimit = model.DailyCaloriesLimit
            };

            return dto;
        }

        private IList<OutUserRoleDto> ConvertToOutUserRoleDtoList(IList<Models.IRole> models)
        {
            if (models == null)
                return null;


            var dtoList = new List<OutUserRoleDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToOutUserRoleDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        private OutUserRoleDto ConvertToOutUserRoleDto(Models.IRole model)
        {
            if (model == null)
                return null;

            var dto = new OutUserRoleDto
            {
                RoleName = model.Name
            };

            return dto;
        }

        private IList<OutShortUserInfoDto> ConvertToOutShortUserInfoDtoList(IList<Models.IUser> models)
        {
            if (models == null)
                return null;


            var dtoList = new List<OutShortUserInfoDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToOutShortUserInfoDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        private OutShortUserInfoDto ConvertToOutShortUserInfoDto(Models.IUser model)
        {
            if (model == null)
                return null;

            var dto = new OutShortUserInfoDto
            {
                UserName = model.UserName
            };

            return dto;
        }
    }
}