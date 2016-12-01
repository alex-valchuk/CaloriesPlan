using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.Dao
{
    public interface IMealDao
    {
        IMeal NewMealInstance();

        List<IMeal> GetMealsByUserName(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, int offset, int rows);
        IMeal GetMealByID(int mealID);
        void Create(IMeal dbMeal);
        void Update(IMeal dbMeal);
        void Delete(IMeal dbMeal);
        int Count(Expression<Func<IMeal, bool>> predicate);
        bool Contains(Expression<Func<IMeal, bool>> predicate);
    }
}
