using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

using Newtonsoft.Json;

using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DAL.DataModel;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.UTL;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.BLL.Entities;
using CaloriesPlan.BLL.Entities.AspNetIdentity;
using CaloriesPlan.BLL.Services.Impl.Base;
using CaloriesPlan.DTO.In;

using Models = CaloriesPlan.DAL.DataModel.Abstractions;

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

        public IRegistrationResult RegisterUser(InRegisterDto registerDto)
        {
            var defaultCaloriesLimit = this.configProvider.GetDefaultCaloriesLimit();

            var user = new User
            {
                DailyCaloriesLimit = defaultCaloriesLimit > 0 ? defaultCaloriesLimit : 50,
                UserName = registerDto.UserName
            };

            AspNetIdentityRegistrationResut registrationResult = null;

            var identityResult = this.userDao.CreateUser(user, registerDto.Password);
            if (identityResult.Succeeded)
            {
                identityResult = this.userDao.AddUserRole(user, "User");

                registrationResult = new AspNetIdentityRegistrationResut(identityResult);
            }

            return registrationResult;
        }

        public IList<OutAccountDto> GetAccounts()
        {
            var dbUsers = this.userDao.GetUsers();
            var dtoUsers = this.ConvertToOutAccountDtoList(dbUsers);

            return dtoUsers;
        }

        public OutAccountDto GetAccount(string userName)
        {
            var user = this.userDao.GetUserByName(userName);
            var dto = this.ConvertToOutAccountDto(user);

            var dbUserRoles = this.userDao.GetUserRoles(user);
            dto.UserRoles = this.ConvertToOutUserRoleDtoList(dbUserRoles);

            return dto;
        }

        public void UpdateAccount(string userName, InAccountDto accountDto)
        {
            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            user.DailyCaloriesLimit = accountDto.DailyCaloriesLimit.Value;

            this.userDao.Update(user);
        }

        public void DeleteAccount(string userName)
        {
            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            this.userDao.Delete(user);
        }

        public IList<OutUserRoleDto> GetUserRoles(string userName)
        {
            var user = this.userDao.GetUserByName(userName);
            var dbRoles = this.userDao.GetUserRoles(user);

            var dtoRoles = this.ConvertToOutUserRoleDtoList(dbRoles);
            return dtoRoles;
        }

        public IList<OutUserRoleDto> GetNotUserRoles(string userName)
        {
            var user = this.userDao.GetUserByName(userName);
            var dbRoles = this.userDao.GetNotUserRoles(user);

            var dtoRoles = this.ConvertToOutUserRoleDtoList(dbRoles);
            return dtoRoles;
        }

        public void AddUserRole(string userName, string roleName)
        {
            var user = this.userDao.GetUserByName(userName);
            this.userDao.AddUserRole(user, roleName);
        }

        public void DeleteUserRole(string userName, string roleName)
        {
            var user = this.userDao.GetUserByName(userName);
            this.userDao.DeleteUserRole(user, roleName);
        }

        public async Task<AuthenticationTicket> GetAuthenticationTicket(string userName, string password, string authType)
        {
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

        private AuthenticationProperties CreateAuthProperties(string userName, string roleNames)
        {
            var data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "roles", roleNames }
            };

            return new AuthenticationProperties(data);
        }

        private IList<OutAccountDto> ConvertToOutAccountDtoList(IList<Models.IUser> dbUsers)
        {
            var dtoUsers = new List<OutAccountDto>();

            foreach (var dbUser in dbUsers)
            {
                var dtoUser = this.ConvertToOutAccountDto(dbUser);
                dtoUsers.Add(dtoUser);
            }

            return dtoUsers;
        }

        private OutAccountDto ConvertToOutAccountDto(Models.IUser dbUser)
        {
            var dtoUser = new OutAccountDto
            {
                UserName = dbUser.UserName,
                DailyCaloriesLimit = dbUser.DailyCaloriesLimit
            };

            return dtoUser;
        }

        private IList<OutUserRoleDto> ConvertToOutUserRoleDtoList(IList<IdentityRole> dbRoles)
        {
            var dtoRoles = new List<OutUserRoleDto>();

            foreach (var dbRole in dbRoles)
            {
                var dtoRole = this.ConvertToOutUserRoleDto(dbRole);
                dtoRoles.Add(dtoRole);
            }

            return dtoRoles;
        }

        private OutUserRoleDto ConvertToOutUserRoleDto(IdentityRole dbRole)
        {
            var dtoRole = new OutUserRoleDto
            {
                RoleName = dbRole.Name
            };

            return dtoRole;
        }
    }
}