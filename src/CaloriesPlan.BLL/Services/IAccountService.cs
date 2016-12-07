﻿using System.Collections.Generic;
using System.Threading.Tasks;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Services
{
    public interface IAccountService
    {
        Task SignUpAsync(InSignUpDto signUpDto);
        Task<IList<OutAccountDto>> GetAccountsAsync();
        OutAccountDto GetAccount(string userName);
        void UpdateAccount(string userName, InAccountDto accountDto);
        void DeleteAccount(string userName);

        IList<OutUserRoleDto> GetUserRoles(string userName);
        IList<OutUserRoleDto> GetNotUserRoles(string userName);
        Task AddUserRoleAsync(string userName, string roleName);
        void DeleteUserRole(string userName, string roleName);

        ICollection<OutShortUserInfoDto> GetSubscribers(string userName);
    }
}
