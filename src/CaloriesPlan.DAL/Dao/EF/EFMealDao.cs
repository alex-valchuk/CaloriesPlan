using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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

        public List<IMeal> GetMealsByUserName(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, int offset, int rows)
        {
            var userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar, 200) { Value = userName };
            var dateFromParam = new SqlParameter("@DateFrom", SqlDbType.DateTime) { Value = dateFrom };
            var dateToParam = new SqlParameter("@DateTo", SqlDbType.DateTime) { Value = dateTo };
            var timeFromParam = new SqlParameter("@TimeFrom", SqlDbType.DateTime) { Value = timeFrom };
            var timeToParam = new SqlParameter("@TimeTo", SqlDbType.DateTime) { Value = timeTo };
            var offsetParam = new SqlParameter("@Offset", SqlDbType.Int) { Value = offset };
            var rowsParam = new SqlParameter("@Rows", SqlDbType.Int) { Value = rows };

            var query = this.dbContext.Meals
                .SqlQuery("execute [dbo].sp_GetUserMeals @UserName, @DateFrom, @DateTo, @TimeFrom, @TimeTo, @Offset, @Rows",
                    userNameParam,
                    dateFromParam,
                    dateToParam,
                    timeFromParam,
                    timeToParam,
                    offsetParam,
                    rowsParam);

            return query.ToList<IMeal>();
        }

        public int GetUserMealsCount(string userID)
        {
            return
                this.dbContext.Meals.Count(m => m.UserID == userID);
        }

        public IMeal GetMealByID(int mealID)
        {
            return 
                this.dbContext.Meals
                    .FirstOrDefault(m => m.ID == mealID);
        }

        public void Create(IMeal dbMeal)
        {
            this.dbContext.Meals.Add((Meal)dbMeal);
            this.dbContext.SaveChanges();
        }

        public void Update(IMeal dbMeal)
        {
            this.dbContext.SaveChanges();
        }

        public void Delete(IMeal dbMeal)
        {
            this.dbContext.Meals.Remove((Meal)dbMeal);
            this.dbContext.SaveChanges();
        }
    }
}
