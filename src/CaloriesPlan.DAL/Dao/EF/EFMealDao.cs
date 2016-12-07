using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using CaloriesPlan.DAL.DataModel;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DAL.Dao.EF.Base;

namespace CaloriesPlan.DAL.Dao.EF
{
    public class EFMealDao : EFDaoBase, IMealDao
    {
        public IMeal NewMealInstance()
        {
            return new Meal();
        }

        public IList<IMeal> GetMeals(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, int offset, int rows)
        {
            var userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar, 200) { Value = userName };
            var dateFromParam = new SqlParameter("@DateFrom", SqlDbType.DateTime) { Value = dateFrom };
            var dateToParam = new SqlParameter("@DateTo", SqlDbType.DateTime) { Value = dateTo };
            var timeFromParam = new SqlParameter("@TimeFrom", SqlDbType.DateTime) { Value = timeFrom };
            var timeToParam = new SqlParameter("@TimeTo", SqlDbType.DateTime) { Value = timeTo };
            var offsetParam = new SqlParameter("@Offset", SqlDbType.Int) { Value = offset };
            var rowsParam = new SqlParameter("@Rows", SqlDbType.Int) { Value = rows };

            var query = this.dbContext.Database
                .SqlQuery<Meal>("execute [dbo].sp_GetUserMeals @UserName, @DateFrom, @DateTo, @TimeFrom, @TimeTo, @Offset, @Rows",
                    userNameParam,
                    dateFromParam,
                    dateToParam,
                    timeFromParam,
                    timeToParam,
                    offsetParam,
                    rowsParam);

            return query.ToList<IMeal>();
        }

        public async Task<IMeal> GetMealByIDAsync(int mealID)
        {
            return await
                this.dbContext.Meals
                    .FirstOrDefaultAsync(m => m.ID == mealID);
        }

        public async Task CreateAsync(IMeal dbMeal)
        {
            this.dbContext.Meals.Add((Meal)dbMeal);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IMeal dbMeal)
        {
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IMeal dbMeal)
        {
            this.dbContext.Meals.Remove((Meal)dbMeal);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync(Expression<Func<IMeal, bool>> predicate)
        {
            var count = await this.dbContext.Meals.CountAsync(predicate);
            return count;
        }

        public async Task<bool> ContainsAsync(Expression<Func<IMeal, bool>> predicate)
        {
            var contains = await this.dbContext.Meals.AnyAsync(predicate);
            return contains;
        }
    }
}
