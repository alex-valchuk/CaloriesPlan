using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

using CaloriesPlan.DAL.DataModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CaloriesPlan.DAL.Dao
{
    public interface IUserDao
    {
        IdentityResult CreateUser(User user, string password);
        Task<ClaimsIdentity> CreateIdentity(User user, string authType);
        Task<User> GetUserByCredentials(string userName, string password);

        IList<User> GetUsers();
        User GetUserByName(string userName);
        void Update(User user);
        void Delete(User user);

        IList<IdentityRole> GetUserRoles(User user);
        IList<IdentityRole> GetNotUserRoles(User user);
        IdentityResult AddUserRole(User user, string roleName);
        void DeleteUserRole(User user, string roleName);
    }
}
