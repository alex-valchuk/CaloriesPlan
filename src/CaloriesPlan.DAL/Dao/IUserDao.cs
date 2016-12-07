using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using CaloriesPlan.UTL.Wrappers;

using Models = CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.Dao
{
    public interface IUserDao
    {
        Models.IUser NewUserInstance();

        Task<IAccountRegistrationResult> CreateUserAsync(Models.IUser user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(Models.IUser user, string authType);
        Task<Models.IUser> GetUserByCredentialsAsync(string userName, string password);

        IList<Models.IUser> GetUsers();
        IList<Models.IUser> GetSubscribers(Models.IUser user);
        IList<Models.IUser> GetSubscribingUsers(Models.IUser user);

        Models.IUser GetUserByName(string userName);
        void Update(Models.IUser user);
        void Delete(Models.IUser user);

        IList<Models.IRole> GetUserRoles(Models.IUser user);
        IList<Models.IRole> GetNotUserRoles(Models.IUser user);
        Task<IAccountRegistrationResult> AddUserRoleAsync(Models.IUser user, string roleName);
        void DeleteUserRole(Models.IUser user, string roleName);
    }
}
