using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.DataModel
{
    [Table("Meal")]
    public class Meal : IMeal
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        [Required]
        public int Calories { get; set; }

        [Required]
        public DateTime EatingDate { get; set; }

        [Required]
        public string UserID { get; set; }

        public User User { get; set; }
    }
}
