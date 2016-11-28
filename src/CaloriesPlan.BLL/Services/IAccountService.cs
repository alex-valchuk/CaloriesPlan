using System.Collections.Generic;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Services
{
    public interface IAccountService
    {
        void SignUp(InSignUpDto signUpDto);
        IList<OutAccountDto> GetAccounts();
        OutAccountDto GetAccount(string userName);
        void UpdateAccount(string userName, InAccountDto accountDto);
        void DeleteAccount(string userName);

        IList<OutUserRoleDto> GetUserRoles(string userName);
        IList<OutUserRoleDto> GetNotUserRoles(string userName);
        void AddUserRole(string userName, string roleName);
        void DeleteUserRole(string userName, string roleName);

        ICollection<OutShortUserInfoDto> GetSubscribers(string userName);
    }
}
