using System;
using System.ComponentModel.DataAnnotations;

namespace CaloriesPlan.DTO.In
{
    public class InMealDto
    {
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Text { get; set; }

        [RegularExpression(pattern: @"^\d+$", ErrorMessage = "{0} expects integer value")]
        [Required(ErrorMessage = "{0} is required")]
        public int? Calories { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public DateTime? EatingDate { get; set; }
    }
}
