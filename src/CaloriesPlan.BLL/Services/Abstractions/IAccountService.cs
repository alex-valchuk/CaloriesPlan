using System.Collections.Generic;
using System.Threading.Tasks;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Services.Abstractions
{
    public interface IAccountService
    {
        Task SignUpAsync(InSignUpDto signUpDto);
        Task<IList<OutUserDto>> GetAccountsAsync();
        Task<OutUserProfileDto> GetUserProfileAsync(string userName);
        Task UpdateAccountAsync(string userName, InAccountDto accountDto);
        Task DeleteAccountAsync(string userName);

        Task<IList<OutUserRoleDto>> GetUserRolesAsync(string userName);
        Task<IList<OutUserRoleDto>> GetNotUserRolesAsync(string userName);
        Task AddUserRoleAsync(string userName, string roleName);
        Task DeleteUserRoleAsync(string userName, string roleName);

        Task<ICollection<OutShortUserInfoDto>> GetSubscribersAsync(string userName);
    }
}
