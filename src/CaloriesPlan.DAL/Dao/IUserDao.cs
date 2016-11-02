using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using CaloriesPlan.DAL.DataModel;
using Models = CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.Dao
{
    public interface IUserDao
    {
        IdentityResult CreateUser(User user, string password);
        Task<ClaimsIdentity> CreateIdentity(User user, string authType);
        Task<User> GetUserByCredentials(string userName, string password);

        IList<Models.IUser> GetUsers();
        Models.IUser GetUserByName(string userName);
        void Update(Models.IUser user);
        void Delete(Models.IUser user);

        IList<IdentityRole> GetUserRoles(Models.IUser user);
        IList<IdentityRole> GetNotUserRoles(Models.IUser user);
        IdentityResult AddUserRole(Models.IUser user, string roleName);
        void DeleteUserRole(Models.IUser user, string roleName);
    }
}
