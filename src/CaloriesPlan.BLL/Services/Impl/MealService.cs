﻿using System;
using System.Collections.Generic;

using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.UTL;

namespace CaloriesPlan.BLL.Services.Impl
{
    public class MealService : IMealService
    {
        private readonly IConfigProvider configProvider;

        private readonly IMealDao mealDao;
        private readonly IUserDao userDao;

        public MealService(IConfigProvider configProvider, IMealDao mealDao, IUserDao userDao)
        {
            this.configProvider = configProvider;

            this.mealDao = mealDao;
            this.userDao = userDao;
        }

        public OutNutritionReportDto GetUserNutritionReport(string userName, InMealReportFilterDto filter)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("UserName not specified");

            if (filter == null ||
                filter.DateFrom == null || filter.DateTo == null ||
                filter.TimeFrom == null || filter.TimeTo == null)
                throw new ArgumentNullException("Filter not specified correctly");


            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            if (filter.DateFrom.Value > filter.DateTo.Value)
                throw new InvalidDateRangeException("'Date:From' should be less or equal to 'Date:To'");

            var days = (filter.DateTo.Value - filter.DateFrom.Value).TotalDays + 1;
            filter.DateTo = filter.DateFrom.Value.AddDays(days);

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
            if (dbMeal == null)
                return null;

            var dtoMeal = this.ConvertToDto(dbMeal);
            return dtoMeal;
        }

        public void CreateMeal(string userName, InMealDto mealDto)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("UserName not specified");

            if (mealDto == null)
                throw new ArgumentNullException("Meal not specified");


            var user = this.userDao.GetUserByName(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var defaultCaloriesLimit = this.configProvider.GetDefaultCaloriesLimit();

            var dbMeal = this.mealDao.NewMealInstance();
            dbMeal.Text = mealDto.Text;
            dbMeal.Calories = mealDto.Calories ?? defaultCaloriesLimit;
            dbMeal.EatingDate = mealDto.EatingDate ?? DateTime.Now;
            dbMeal.UserID = user.Id;

            this.mealDao.Create(dbMeal);
        }

        public void UpdateMeal(int id, InMealDto mealDto)
        {
            if (mealDto == null ||
                string.IsNullOrEmpty(mealDto.Text) ||
                mealDto.Calories == null ||
                mealDto.EatingDate == null)
                throw new ArgumentNullException("Meal not specified correctly");


            var dbMeal = this.mealDao.GetMealByID(id);
            if (dbMeal == null)
                throw new MealDoesNotExistException();

            dbMeal.Text = mealDto.Text;
            dbMeal.Calories = mealDto.Calories.Value;
            dbMeal.EatingDate = mealDto.EatingDate.Value;

            this.mealDao.Update(dbMeal);
        }

        public void DeleteMeal(int id)
        {
            var dbMeal = this.mealDao.GetMealByID(id);
            if (dbMeal == null)
                throw new MealDoesNotExistException();

            this.mealDao.Delete(dbMeal);
        }

        private List<OutMealDto> ConvertToDtoList(IList<IMeal> dbMeals)
        {
            if (dbMeals == null)
                return null;


            var dtoMeals = new List<OutMealDto>();

            foreach (var dbMeal in dbMeals)
            {
                var dto = this.ConvertToDto(dbMeal);
                dtoMeals.Add(dto);
            }

            return dtoMeals;
        }

        private OutMealDto ConvertToDto(IMeal dbMeal)
        {
            if (dbMeal == null)
                return null;

            var dto = new OutMealDto();
            dto.ID = dbMeal.ID;
            dto.Calories = dbMeal.Calories;
            dto.Text = dbMeal.Text;
            dto.EatingDate = dbMeal.EatingDate;

            return dto;
        }
    }
}
