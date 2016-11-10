using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity.EntityFramework;

using CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.DataModel
{
    public class User : IdentityUser, IUser
    {
        [Required]
        public string PasswordSalt { get; set; }

        [Required]        
        public int DailyCaloriesLimit { get; set; }
    }
}
