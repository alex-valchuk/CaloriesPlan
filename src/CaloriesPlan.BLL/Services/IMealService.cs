using System;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Services
{
    public interface IMealService
    {
        OutNutritionReportDto GetUserNutritionReport(string userName, InMealReportFilterDto filter);
        OutMealDto GetMealByID(int mealID);
        void CreateMeal(string userName, InMealDto mealDto);
        void UpdateMeal(int id, InMealDto mealDto);
        void DeleteMeal(int id);
    }
}
