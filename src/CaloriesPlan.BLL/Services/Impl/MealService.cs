using System;
using System.Collections.Generic;
using System.Linq;

using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DAL.DataModel;
using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.BLL.Exceptions;

namespace CaloriesPlan.BLL.Services.Impl
{
    public class MealService : IMealService
    {
        private readonly IMealDao mealDao;
        private readonly IUserDao userDao;

        public MealService(IMealDao mealDao, IUserDao userDao)
        {
            this.mealDao = mealDao;
            this.userDao = userDao;
        }

        public OutNutritionReportDto GetUserNutritionReport(string userName, MealReportFilterDto filter)
        {
            if (filter.DateFrom.Value > filter.DateTo.Value)
                throw new InvalidDateRangeException("'Date:From' should be less or equal to 'Date:To'");

            var days = (filter.DateTo.Value - filter.DateFrom.Value).TotalDays + 1;
            filter.DateTo = filter.DateFrom.Value.AddDays(days);

            var user = this.userDao.GetUserByName(userName);
            var nutritionReport = new OutNutritionReportDto(user.DailyCaloriesLimit);

            var dbMeals = this.mealDao.GetMealsByUserName(userName, filter.DateFrom.Value, filter.DateTo.Value, filter.TimeFrom.Value, filter.TimeTo.Value);
            if (dbMeals != null)
            {
                nutritionReport.Meals = this.ConvertToDtoList(dbMeals);
            }

            return nutritionReport;
        }

        public OutMealDto GetMealByID(int mealID)
        {
            var dbMeal = this.mealDao.GetMealByID(mealID);
            var dtoMeal = this.ConvertToDto(dbMeal);

            return dtoMeal;
        }

        public void CreateMeal(string text, int calories, DateTime eatenDate, string userName)
        {
            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var dbMeal = new Meal
            {
                Text = text,
                Calories = calories,
                EatingDate = eatenDate,
                UserID = user.Id
            };

            this.mealDao.Create(dbMeal);
        }

        public void UpdateMeal(int id, string text, int calories, DateTime eatenDate)
        {
            var dbMeal = this.mealDao.GetMealByID(id);
            if (dbMeal == null)
                throw new MealDoesNotExistException();

            dbMeal.Text = text;
            dbMeal.Calories = calories;
            dbMeal.EatingDate = eatenDate;

            this.mealDao.Update(dbMeal);
        }

        public void DeleteMeal(int id)
        {
            var dbMeal = this.mealDao.GetMealByID(id);
            if (dbMeal == null)
                throw new MealDoesNotExistException();

            this.mealDao.Delete(dbMeal);
        }

        private List<OutMealDto> ConvertToDtoList(IList<Meal> dbMeals)
        {
            var dtoMeals = new List<OutMealDto>();

            foreach (var dbMeal in dbMeals)
            {
                var dto = this.ConvertToDto(dbMeal);
                dtoMeals.Add(dto);
            }

            return dtoMeals;
        }

        private OutMealDto ConvertToDto(Meal dbMeal)
        {
            var dto = new OutMealDto();
            dto.ID = dbMeal.ID;
            dto.Calories = dbMeal.Calories;
            dto.Text = dbMeal.Text;
            dto.EatingDate = dbMeal.EatingDate;

            return dto;
        }
    }
}
