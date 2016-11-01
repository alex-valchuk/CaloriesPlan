using System;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Services
{
    public interface IMealService
    {
        OutNutritionReportDto GetUserNutritionReport(string userName, MealReportFilterDto filter);
        OutMealDto GetMealByID(int mealID);
        void CreateMeal(string text, int calories, DateTime eatenDate, string userName);
        void UpdateMeal(int id, string text, int calories, DateTime eatenDate);
        void DeleteMeal(int id);
    }
}
