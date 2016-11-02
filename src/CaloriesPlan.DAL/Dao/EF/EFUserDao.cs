using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using CaloriesPlan.DAL.Dao.EF.Base;
using CaloriesPlan.DAL.DataModel;
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

        public IdentityResult CreateUser(User user, string password)
        {
            var result = this.userManager.Create(user, password);
            return result;
        }

        public async Task<ClaimsIdentity> CreateIdentity(User user, string authType)
        {
            var claimsIdentity = await this.userManager.CreateIdentityAsync(user, authType);
            return claimsIdentity;
        }

        public async Task<User> GetUserByCredentials(string userName, string password)
        {
            return await this.userManager.FindAsync(userName, password);
        }

        public IList<Models.IUser> GetUsers()
        {
            return this.userManager.Users.ToList<Models.IUser>();
        }

        public Models.IUser GetUserByName(string userName)
        {
            return this.userManager.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public void Update(Models.IUser user)
        {
            this.userManager.Update((User)user);
        }

        public void Delete(Models.IUser user)
        {
            this.userManager.Delete((User)user);
        }

        public IList<IdentityRole> GetUserRoles(Models.IUser user)
        {            
            return this.dbContext.Roles
                .Where(r => r.Users.Any(u => u.UserId == user.Id))
                .ToList();
        }

        public IList<IdentityRole> GetNotUserRoles(Models.IUser user)
        {
            return this.dbContext.Roles
                .Where(r => r.Users.Any(u => u.UserId == user.Id) == false)
                .ToList();
        }

        public IdentityResult AddUserRole(Models.IUser user, string roleName)
        {
            var result = this.userManager.AddToRole(user.Id, roleName);
            return result;
        }

        public void DeleteUserRole(Models.IUser user, string roleName)
        {
            this.userManager.RemoveFromRole(user.Id, roleName);
        }
    }
}
