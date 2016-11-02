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

        public List<IMeal> GetMealsByUserName(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo)
        {
            var userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar, 200);
            var dateFromParam = new SqlParameter("@DateFrom", SqlDbType.DateTime);
            var dateToParam = new SqlParameter("@DateTo", SqlDbType.DateTime);
            var timeFromParam = new SqlParameter("@TimeFrom", SqlDbType.DateTime);
            var timeToParam = new SqlParameter("@TimeTo", SqlDbType.DateTime);

            userNameParam.Value = userName;
            dateFromParam.Value = dateFrom;
            dateToParam.Value = dateTo;
            timeFromParam.Value = timeFrom;
            timeToParam.Value = timeTo;

            var query = this.dbContext.Meals
                .SqlQuery("execute [dbo].sp_GetUserMeals @UserName, @DateFrom, @DateTo, @TimeFrom, @TimeTo",
                    userNameParam,
                    dateFromParam,
                    dateToParam,
                    timeFromParam,
                    timeToParam);

            return query.ToList<IMeal>();
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
