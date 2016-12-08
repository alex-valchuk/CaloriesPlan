using System;
using System.Threading.Tasks;

using CaloriesPlan.UTL.Config.Abstractions;
using CaloriesPlan.DAL.Dao.Abstractions;
using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.BLL.Services.Abstractions;
using CaloriesPlan.BLL.Mapping.Abstractions;

namespace CaloriesPlan.BLL.Services
{
    public class MealService : IMealService
    {
        private readonly IConfigProvider configProvider;
        private readonly IMealMapper mealMapper;

        private readonly IMealDao mealDao;
        private readonly IUserDao userDao;

        public MealService(IConfigProvider configProvider, IMealMapper mealMapper, IMealDao mealDao, IUserDao userDao)
        {
            this.configProvider = configProvider;
            this.mealMapper = mealMapper;

            this.mealDao = mealDao;
            this.userDao = userDao;
        }

        public async Task<OutNutritionReportDto> GetUserNutritionReportAsync(string userName, InMealReportFilterDto filter)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("UserName");

            if (filter == null)
                filter = new InMealReportFilterDto();

            if (filter.DateFrom == null)
                filter.DateFrom = DateTime.UtcNow.AddYears(-50);
                
            if (filter.DateTo == null)
                filter.DateTo = DateTime.UtcNow.AddYears(50);
                
            if (filter.TimeFrom == null)
                filter.TimeFrom = new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();
                
            if (filter.TimeTo == null)
                filter.TimeTo = new DateTime(1970, 1, 1, 23, 59, 59).ToUniversalTime();

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            if (filter.DateFrom.Value > filter.DateTo.Value)
                throw new InvalidDateRangeException("'Date:From' should be less or equal to 'Date:To'");

            var days = (filter.DateTo.Value - filter.DateFrom.Value).TotalDays + 1;
            filter.DateTo = filter.DateFrom.Value.AddDays(days);

            var totalItemsCount = await this.mealDao.CountAsync(m => m.UserID == user.Id);

            var nutritionReport = new OutNutritionReportDto(user.DailyCaloriesLimit, totalItemsCount);

            var offset = filter.Page * filter.PageSize;
            var rows = filter.PageSize;

            var dbMeals = await this.mealDao.GetMealsAsync(userName, 
                filter.DateFrom.Value, filter.DateTo.Value,
                filter.TimeFrom.Value, filter.TimeTo.Value,
                offset, rows);

            nutritionReport.Meals = this.mealMapper.ConvertToDtoList(dbMeals);

            return nutritionReport;
        }

        public async Task<OutMealDto> GetMealByIDAsync(int mealID)
        {
            var dbMeal = await this.mealDao.GetMealByIDAsync(mealID);
            if (dbMeal == null)
                return null;

            var dtoMeal = this.mealMapper.ConvertToDto(dbMeal);
            return dtoMeal;
        }

        public async Task CreateMealAsync(string userName, InMealDto mealDto)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("UserName");

            if (mealDto == null)
                throw new ArgumentNullException("Meal");


            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new AccountDoesNotExistException();

            var defaultCaloriesLimit = this.configProvider.GetDefaultCaloriesLimit();

            var dbMeal = this.mealDao.NewMealInstance();
            dbMeal.Text = mealDto.Text;
            dbMeal.Calories = mealDto.Calories ?? defaultCaloriesLimit;
            dbMeal.EatingDate = mealDto.EatingDate ?? DateTime.Now;
            dbMeal.UserID = user.Id;

            await this.mealDao.CreateAsync(dbMeal);
        }

        public async Task UpdateMealAsync(int id, InMealDto mealDto)
        {
            if (mealDto == null ||
                string.IsNullOrEmpty(mealDto.Text) ||
                mealDto.Calories == null ||
                mealDto.EatingDate == null)
                throw new ArgumentNullException("Meal");


            var dbMeal = await this.mealDao.GetMealByIDAsync(id);
            if (dbMeal == null)
                throw new MealDoesNotExistException();

            dbMeal.Text = mealDto.Text;
            dbMeal.Calories = mealDto.Calories.Value;
            dbMeal.EatingDate = mealDto.EatingDate.Value;

            await this.mealDao.UpdateAsync(dbMeal);
        }

        public async Task DeleteMealAsync(int id)
        {
            var dbMeal = await this.mealDao.GetMealByIDAsync(id);
            if (dbMeal == null)
                throw new MealDoesNotExistException();

            await this.mealDao.DeleteAsync(dbMeal);
        }

        public async Task <bool> IsOwnerOfMealAsync(string userName, int mealID)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");

            if (mealID <= 0)
                throw new ArgumentException("MealID should be more than 0");

            var user = await this.userDao.GetUserByNameAsync(userName);
            if (user == null)
                throw new MealDoesNotExistException();

            var isOwner = await this.mealDao
                .ContainsAsync(m =>
                    m.UserID == user.Id &&
                    m.ID == mealID);

            return isOwner;
        }
    }
}
