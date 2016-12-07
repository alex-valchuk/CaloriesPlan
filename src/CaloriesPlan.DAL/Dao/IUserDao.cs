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

        Task<IList<Models.IUser>> GetUsersAsync();
        Task<IList<Models.IUser>> GetSubscribersAsync(Models.IUser user);
        Task<IList<Models.IUser>> GetSubscribingUsersAsync(Models.IUser user);

        Task<Models.IUser> GetUserByNameAsync(string userName);
        Task UpdateAsync(Models.IUser user);
        Task DeleteAsync(Models.IUser user);

        Task<IList<Models.IRole>> GetUserRolesAsync(Models.IUser user);
        Task<IList<Models.IRole>> GetNotUserRolesAsync(Models.IUser user);
        Task<IAccountRegistrationResult> AddUserRoleAsync(Models.IUser user, string roleName);
        Task DeleteUserRoleAsync(Models.IUser user, string roleName);
    }
}
