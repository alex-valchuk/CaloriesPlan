using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Security;

using Newtonsoft.Json;

using CaloriesPlan.UTL.Config.Abstractions;
using CaloriesPlan.DAL.Dao.Abstractions;
using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.BLL.Services.Abstractions;
using CaloriesPlan.BLL.Exceptions;

using Models = CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.BLL.Services
{
    public class AccountService : IAccountService, IOAuthService
    {
        private readonly IConfigProvider configProvider;
        private readonly IUserDao userDao;

        public AccountService(IConfigProvider configProvider, IUserDao userDao)
        {
            this.configProvider = configProvider;
            this.userDao = userDao;
        }

        public async Task SignUpAsync(InSignUpDto signUpDto)
        {
            if (signUpDto == null ||
                string.IsNullOrEmpty(signUpDto.UserName) ||
                string.IsNullOrEmpty(signUpDto.Password))
                throw new ArgumentNullException("Register data");

            if (signUpDto.Password != signUpDto.ConfirmPassword)
                throw new InvalidPasswordConfirmationException("Password", "Password does not match password confirmation");


            var defaultCaloriesLimit = this.configProvider.GetDefaultCaloriesLimit();

            var user = this.userDao.NewUserInstance();
            user.UserName = signUpDto.UserName;
            user.DailyCaloriesLimit =
                defaultCaloriesLimit > 0
                    ? defaultCaloriesLimit
                    : 50;//if not configured


            var userRegistrationResult = await this.userDao.CreateUserAsync(user, signUpDto.Password);
            if (userRegistrationResult.Succeeded == false)
                throw new RegistrationException(userRegistrationResult);

            var roleRegistrationResult = await this.userDao.AddUserRoleAsync(user, "User");
            if (roleRegistrationResult.Succeeded == false)
                throw new RegistrationException(roleRegistrationResult);
        }

        public async Task<AuthenticationTicket> SignInAsync(string userName, string password, string authType)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Password");

            if (string.IsNullOrEmpty(authType))
                throw new ArgumentNullException("Authentication Type");

            var user = await this.userDao.GetUserByCredentialsAsync(userName, password);
            if (user == null)
                return null;

            var claimsIdentity = await this.userDao.CreateIdentityAsync(user, authType);

            var roles = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            var roleNames = JsonConvert.SerializeObject(roles.Select(x => x.Value));

            var authProperties = this.CreateAuthProperties(user.UserName, roleNames);
            var authTicket = new AuthenticationTicket(claimsIdentity, authProperties);

            return authTicket;
        }

        public async Task<IList<OutAccountDto>> GetAccountsAsync()
        {
            var dbUsers = await this.userDao.GetUsersAsync();
            var dtoUsers = this.ConvertToOutAccountDtoList(dbUsers);

            return dtoUsers;
        }

        public async Task<OutAccountDto> GetAccountAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                return null;


            var dto = this.ConvertToOutAccountDto(user);

            var dbUserRoles = await this.userDao.GetUserRolesAsync(user);
            if (dbUserRoles != null)
            {
                dto.UserRoles = this.ConvertToOutUserRoleDtoList(dbUserRoles);
            }

            return dto;
        }

        public async Task UpdateAccountAsync(string userName, InAccountDto accountDto)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (accountDto == null)
                throw new ArgumentNullException("Account");

            if (accountDto.DailyCaloriesLimit == null)
                throw new ArgumentNullException("DailyCaloriesLimit");


            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            user.DailyCaloriesLimit = accountDto.DailyCaloriesLimit.Value;

            await this.userDao.UpdateAsync(user);
        }

        public async Task DeleteAccountAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();


            await this.userDao.DeleteAsync(user);
        }

        public async Task<IList<OutUserRoleDto>> GetUserRolesAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var dbRoles = await this.userDao.GetUserRolesAsync(user);
            if (dbRoles == null)
                return null;


            var dtoRoles = this.ConvertToOutUserRoleDtoList(dbRoles);
            return dtoRoles;
        }

        public async Task<IList<OutUserRoleDto>> GetNotUserRolesAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var dbRoles = await this.userDao.GetNotUserRolesAsync(user);
            if (dbRoles == null)
                return null;


            var dtoRoles = this.ConvertToOutUserRoleDtoList(dbRoles);
            return dtoRoles;
        }

        public async Task AddUserRoleAsync(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();


            await this.userDao.AddUserRoleAsync(user, roleName);
        }

        public async Task DeleteUserRoleAsync(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();


            await this.userDao.DeleteUserRoleAsync(user, roleName);
        }

        public async Task<ICollection<OutShortUserInfoDto>> GetSubscribersAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("User Name");

            var user = await this.userDao.GetUserByNameAsync(userName);
            var users = await this.userDao.GetSubscribersAsync(user);

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