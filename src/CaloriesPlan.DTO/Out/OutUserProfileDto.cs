using System.Collections.Generic;

namespace CaloriesPlan.DTO.Out
{
    public class OutUserProfileDto : OutUserDto
    {
        public IList<OutUserRoleDto> UserRoles { get; set; }
    }
}
