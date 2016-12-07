using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.Dao
{
    public interface IMealDao
    {
        IMeal NewMealInstance();

        IList<IMeal> GetMeals(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, int offset, int rows);
        Task<IMeal> GetMealByIDAsync(int mealID);
        Task CreateAsync(IMeal dbMeal);
        Task UpdateAsync(IMeal dbMeal);
        Task DeleteAsync(IMeal dbMeal);
        Task<int> CountAsync(Expression<Func<IMeal, bool>> predicate);
        Task<bool> ContainsAsync(Expression<Func<IMeal, bool>> predicate);
    }
}
