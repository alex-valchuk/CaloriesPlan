using System;

namespace CaloriesPlan.DTO.Out
{
    public class OutMealDto
    {
        public int ID { get; set; }
        public int Calories { get; set; }
        public string Text { get; set; }
        public DateTime EatingDate { get; set; }
    }
}
