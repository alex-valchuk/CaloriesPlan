using System;
using System.ComponentModel.DataAnnotations;

namespace CaloriesPlan.DTO.In
{
    public class MealReportFilterDto
    {
        [Required]
        public DateTime? DateFrom { get; set; }

        [Required]
        public DateTime? DateTo { get; set; }

        [Required]
        public DateTime? TimeFrom { get; set; }

        [Required]
        public DateTime? TimeTo { get; set; }
    }
}