using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using CaloriesPlan.UTL.Wrappers;
using CaloriesPlan.DAL.DataModel;

using Models = CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.Dao
{
    public interface IUserDao
    {
        Models.IUser NewUserInstance();

        IAccountRegistrationResult CreateUser(Models.IUser user, string password);
        Task<ClaimsIdentity> CreateIdentity(User user, string authType);
        Task<User> GetUserByCredentials(string userName, string password);

        IList<Models.IUser> GetUsers();
        Models.IUser GetUserByName(string userName);
        void Update(Models.IUser user);
        void Delete(Models.IUser user);

        IList<Models.IRole> GetUserRoles(Models.IUser user);
        IList<Models.IRole> GetNotUserRoles(Models.IUser user);
        IAccountRegistrationResult AddUserRole(Models.IUser user, string roleName);
        void DeleteUserRole(Models.IUser user, string roleName);
    }
}
