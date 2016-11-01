using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using CaloriesPlan.DAL.DataModel;
using CaloriesPlan.DAL.Dao.EF.Base;

namespace CaloriesPlan.DAL.Dao.EF
{
    public class EFMealDao : EFDaoBase, IMealDao
    {
        public List<Meal> GetMealsByUserName(string userName, DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo)
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

            return query.ToList();
        }

        public Meal GetMealByID(int mealID)
        {
            return 
                this.dbContext.Meals
                    .FirstOrDefault(m => m.ID == mealID);
        }

        public void Create(Meal dbMeal)
        {
            this.dbContext.Meals.Add(dbMeal);
            this.dbContext.SaveChanges();
        }

        public void Update(Meal dbMeal)
        {
            this.dbContext.SaveChanges();
        }

        public void Delete(Meal dbMeal)
        {
            this.dbContext.Meals.Remove(dbMeal);
            this.dbContext.SaveChanges();
        }
    }
}
