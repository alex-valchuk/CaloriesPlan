using System.ComponentModel.DataAnnotations;

namespace CaloriesPlan.DTO.In
{
    public class InRoleDto
    {
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string RoleName { get; set; }
    }
}
