using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using CaloriesPlan.UTL.Wrappers;
using CaloriesPlan.DAL.Dao.EF.Base;
using CaloriesPlan.DAL.DataModel;
using CaloriesPlan.DAL.Wrappers;

using Models = CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.Dao.EF
{
    public class EFUserDao : EFDaoBase, IUserDao
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EFUserDao()
        {
            this.userManager = new UserManager<User>(new UserStore<User>(this.dbContext));
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(this.dbContext));
        }

        public Models.IUser NewUserInstance()
        {
            return new User();
        }

        public async Task<IAccountRegistrationResult> CreateUserAsync(Models.IUser user, string password)
        {
            var identityUser = (User)user;
            identityUser.PasswordSalt = this.GenerateSalt(32);
            password = this.GetPasswordWithSalt(password: password, passwordSalt: identityUser.PasswordSalt);

            var identityResult = await this.userManager.CreateAsync(identityUser, password);

            return new AspNetIdentityRegistrationResult(identityResult);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(Models.IUser user, string authType)
        {
            var claimsIdentity = await this.userManager.CreateIdentityAsync((User)user, authType);
            return claimsIdentity;
        }

        public async Task<Models.IUser> GetUserByCredentialsAsync(string userName, string password)
        {
            var identityUser = this.userManager.Users.FirstOrDefault(u => u.UserName == userName);
            if (identityUser != null)
            {
                password = this.GetPasswordWithSalt(password: password, passwordSalt: identityUser.PasswordSalt);

                return await this.userManager.FindAsync(userName, password);
            }

            return null;
        }

        public async Task<IList<Models.IUser>> GetUsersAsync()
        {
            return await this.userManager.Users.ToListAsync<Models.IUser>();
        }

        public async Task<IList<Models.IUser>> GetSubscribersAsync(Models.IUser user)
        {
            return await this.dbContext.Subscribtions
                .Where(s => s.UserID == user.Id)
                .Select(s => s.Subscriber)
                .ToListAsync<Models.IUser>();
        }

        public async Task<IList<Models.IUser>> GetSubscribingUsersAsync(Models.IUser user)
        {
            return await this.dbContext.Subscribtions
                .Where(s => s.SubscriberID == user.Id)
                .Select(s => s.User)
                .ToListAsync<Models.IUser>();
        }

        public async Task<Models.IUser> GetUserByNameAsync(string userName)
        {
            return await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task UpdateAsync(Models.IUser user)
        {
            await this.userManager.UpdateAsync((User)user);
        }

        public async Task DeleteAsync(Models.IUser user)
        {
            await this.userManager.DeleteAsync((User)user);
        }

        public async Task<IList<Models.IRole>> GetUserRolesAsync(Models.IUser user)
        {            
            return await this.dbContext.Roles
                .Where(r => r.Users.Any(u => u.UserId == user.Id))
                .Select(r => new Role { Name = r.Name })
                .ToListAsync<Models.IRole>();
        }

        public async Task<IList<Models.IRole>> GetNotUserRolesAsync(Models.IUser user)
        {
            return await this.dbContext.Roles
                .Where(r => r.Users.Any(u => u.UserId == user.Id) == false)
                .Select(r => new Role { Name = r.Name })
                .ToListAsync<Models.IRole>();
        }

        public async Task<IAccountRegistrationResult> AddUserRoleAsync(Models.IUser user, string roleName)
        {
            var identityResult = await this.userManager.AddToRoleAsync(user.Id, roleName);
            return new AspNetIdentityRegistrationResult(identityResult);
        }

        public async Task DeleteUserRoleAsync(Models.IUser user, string roleName)
        {
            await this.userManager.RemoveFromRoleAsync(user.Id, roleName);
        }

        public string GetPasswordWithSalt(string password, string passwordSalt)
        {
            return password + passwordSalt;
        }

        private string GenerateSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}
