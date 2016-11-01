using System.Collections.Generic;

using CaloriesPlan.DTO.Out;
using CaloriesPlan.BLL.Entities;

namespace CaloriesPlan.BLL.Services
{
    public interface IAccountService
    {
        IRegistrationResult RegisterUser(string userName, string password);
        IList<OutAccountDto> GetAccounts();
        OutAccountDto GetAccount(string userName);
        void UpdateAccount(string userName, int dailyCaloriesLimit);
        void DeleteAccount(string userName);

        IList<OutUserRoleDto> GetUserRoles(string userName);
        IList<OutUserRoleDto> GetNotUserRoles(string userName);
        void AddUserRole(string userName, string roleName);
        void DeleteUserRole(string userName, string roleName);
    }
}
