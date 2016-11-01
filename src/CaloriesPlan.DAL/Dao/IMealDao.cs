using System;
using System.Collections.Generic;

using CaloriesPlan.DAL.DataModel;

namespace CaloriesPlan.DAL.Dao
{
    public interface IMealDao
    {
        List<Meal> GetMealsByUserName(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo);
        Meal GetMealByID(int mealID);
        void Create(Meal dbMeal);
        void Update(Meal dbMeal);
        void Delete(Meal dbMeal);
    }
}
