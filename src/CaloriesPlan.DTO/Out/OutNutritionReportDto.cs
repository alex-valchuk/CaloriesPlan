using System.Collections.Generic;

namespace CaloriesPlan.DTO.Out
{
    public class OutNutritionReportDto
    {
        public int DailyCaloriesLimit { get; private set; }

        public int TotalItemsCount { get; private set; }

        public IList<OutMealDto> Meals { get; set; }

        public OutNutritionReportDto(int dailyCaloriesLimit, int totalItemsCount)
        {
            this.DailyCaloriesLimit = dailyCaloriesLimit;
            this.TotalItemsCount = totalItemsCount;
        }
    }
}
