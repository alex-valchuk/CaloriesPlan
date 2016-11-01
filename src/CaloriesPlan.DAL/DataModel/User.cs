using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity.EntityFramework;

namespace CaloriesPlan.DAL.DataModel
{
    public class User : IdentityUser
    {
        [Required]        
        public int DailyCaloriesLimit { get; set; }
    }
}
