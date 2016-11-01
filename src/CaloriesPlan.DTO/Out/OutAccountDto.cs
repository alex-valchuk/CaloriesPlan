using System.Collections.Generic;

namespace CaloriesPlan.DTO.Out
{
    public class OutAccountDto
    {
        public int DailyCaloriesLimit { get; set; }
        public string UserName { get; set; }
        public IList<OutUserRoleDto> UserRoles { get; set; }
    }
}
