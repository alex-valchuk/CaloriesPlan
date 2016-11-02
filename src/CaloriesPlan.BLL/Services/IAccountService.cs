using System.Collections.Generic;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.BLL.Entities;

namespace CaloriesPlan.BLL.Services
{
    public interface IAccountService
    {
        IRegistrationResult RegisterUser(InRegisterDto registerDto);
        IList<OutAccountDto> GetAccounts();
        OutAccountDto GetAccount(string userName);
        void UpdateAccount(string userName, InAccountDto accountDto);
        void DeleteAccount(string userName);

        IList<OutUserRoleDto> GetUserRoles(string userName);
        IList<OutUserRoleDto> GetNotUserRoles(string userName);
        void AddUserRole(string userName, string roleName);
        void DeleteUserRole(string userName, string roleName);
    }
}
