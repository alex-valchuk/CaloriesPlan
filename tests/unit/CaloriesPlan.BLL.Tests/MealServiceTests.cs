using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Services.Impl;
using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DTO.In;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.UTL;

namespace CaloriesPlan.BLL.Tests
{
    [TestClass]
    public class MealServiceTests
    {
        private Mock<IConfigProvider> configProviderMock;
        private Mock<IMealDao> mealDaoMock;
        private Mock<IUserDao> userDaoMock;

        private IMealService mealService;

        [TestInitialize]
        public void Setup()
        {
            this.configProviderMock = new Mock<IConfigProvider>();
            this.mealDaoMock = new Mock<IMealDao>();
            this.userDaoMock = new Mock<IUserDao>();
            this.mealService = new MealService(configProviderMock.Object, mealDaoMock.Object, userDaoMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public async Task GetUserNutritionReportAsync_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //act
            await this.mealService.GetUserNutritionReportAsync(null, null);
        }
        [TestMethod]
        [ExpectedException(typeof(AccountDoesNotExistException), "User does not exist")]
        public async Task GetUserNutritionReportAsync_OfNotExistedUser_AccountDoesNotExistExceptionThrown()
        {
            //arrange
            var userName = "Test";
            var filter = this.GetValidFilter();

            //act
            await this.mealService.GetUserNutritionReportAsync(userName, filter);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDateRangeException), "DateFrom should be less or equal to DateTo")]
        public async Task GetUserNutritionReportAsync_DateFromMoreThanDateTo_InvalidDateRangeExceptionThrown()
        {
            //arrange
            var userName = "Alex";
            var userTask = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(ud => ud.GetUserByNameAsync(It.IsAny<string>())).Returns(userTask);

            var filter = this.GetValidFilter();
            filter.DateFrom = filter.DateTo.Value.AddDays(1);

            //act
            await this.mealService.GetUserNutritionReportAsync(userName, filter);
        }

        [TestMethod]
        public async Task GetUserNutritionReportAsync_ValidInput_ReportReturned()
        {
            //arrange
            var userName = "Alex";
            var userTask = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(ud => ud.GetUserByNameAsync(It.IsAny<string>())).Returns(userTask);

            var filter = this.GetValidFilter();

            //act
            var report = await this.mealService.GetUserNutritionReportAsync(userName, filter);

            //assert
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public async Task GetUserNutritionReportAsync_UserDoesNotHaveMeals_ReportWithoutMealsReturned()
        {
            //arrange
            var userName = "Alex";
            var userTask = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(ud => ud.GetUserByNameAsync(It.IsAny<string>())).Returns(userTask);

            var filter = this.GetValidFilter();

            //act
            var report = await this.mealService.GetUserNutritionReportAsync(userName, filter);

            //assert
            Assert.IsNull(report.Meals);
        }

        [TestMethod]
        public async Task GetUserNutritionReportAsync_UserHasMeals_ReportWithMealsReturned()
        {
            //arrange
            var userName = "Alex";
            var userTask = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(ud => ud.GetUserByNameAsync(It.IsAny<string>())).Returns(userTask);

            var filter = this.GetValidFilter();

            IList<IMeal> dbMeals = new List<IMeal>
            {
                Mock.Of<IMeal>()
            };
            var dbMealsTask = Task.FromResult(dbMeals);

            this.mealDaoMock.Setup(md => md.GetMealsAsync(
                It.IsAny<string>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(dbMealsTask);

            //act
            var report = await this.mealService.GetUserNutritionReportAsync(userName, filter);

            //assert
            Assert.IsNotNull(report.Meals);
        }

        [TestMethod]
        public async Task GetMealByIDAsync_MealDoesNotExists_NullReturned()
        {
            //act
            var meal = await this.mealService.GetMealByIDAsync(It.IsAny<int>());

            //assert
            Assert.IsNull(meal);
        }

        [TestMethod]
        public async Task GetMealByIDAsync_MealExists_MealReturned()
        {
            //arrange
            var dbMealTask = Task.FromResult(Mock.Of<IMeal>());
            this.mealDaoMock.Setup(md => md.GetMealByIDAsync(It.IsAny<int>())).Returns(dbMealTask);

            //act
            var meal = await this.mealService.GetMealByIDAsync(It.IsAny<int>());

            //assert
            Assert.IsNotNull(meal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public async Task CreateMealAsync_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //act
            await this.mealService.CreateMealAsync(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified")]
        public async Task CreateMealAsync_EmptyMealDto_ArgumentNullExceptionThrown()
        {
            //act
            await this.mealService.CreateMealAsync("Alex", null);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountDoesNotExistException), "Account does not exist")]
        public async Task CreateMealAsync_ForNotExistentUser_AccountDoesNotExistExceptionThrown()
        {
            //act
            await this.mealService.CreateMealAsync("Alex", Mock.Of<InMealDto>());
        }

        [TestMethod]
        public async Task CreateMealAsync_ForExistentUser_NewMealCreated()
        {
            //arrange
            var userTask = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(d => d.GetUserByNameAsync(It.IsAny<string>())).Returns(userTask);
            this.mealDaoMock.Setup(d => d.NewMealInstance()).Returns(Mock.Of<IMeal>());

            //act
            await this.mealService.CreateMealAsync("Alex", Mock.Of<InMealDto>());

            //assert
            this.mealDaoMock.Verify(d => d.CreateAsync(It.IsAny<IMeal>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public async Task UpdateMealAsync_EmptyMealDto_ArgumentNullExceptionThrown()
        {
            //act
            await this.mealService.UpdateMealAsync(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public async Task UpdateMealAsync_EmptyMealText_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();
            meal.Text = null;

            //act
            await this.mealService.UpdateMealAsync(1, meal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public async Task UpdateMealAsync_EmptyMealCalories_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();
            meal.Calories = null;

            //act
            await this.mealService.UpdateMealAsync(1, meal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public async Task UpdateMealAsync_EmptyMealEatingDate_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();
            meal.EatingDate = null;

            //act
            await this.mealService.UpdateMealAsync(1, meal);
        }

        [TestMethod]
        [ExpectedException(typeof(MealDoesNotExistException), "Meal does not exist")]
        public async Task UpdateMealAsync_MealDoesNotExist_MealDoesNotExistExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();

            //act
            await this.mealService.UpdateMealAsync(1, meal);
        }

        [TestMethod]
        public async Task UpdateMealAsync_ValidInput_MealUpdated()
        {
            //arrange
            var meal = this.GetValidMealDto();
            var dbMealTask = Task.FromResult(Mock.Of<IMeal>());

            this.mealDaoMock.Setup(d => d.GetMealByIDAsync(It.IsAny<int>())).Returns(dbMealTask);

            //act
            await this.mealService.UpdateMealAsync(1, meal);

            //assert
            this.mealDaoMock.Verify(d => d.UpdateAsync(It.IsAny<IMeal>()));
        }

        [TestMethod]
        [ExpectedException(typeof(MealDoesNotExistException), "Meal does not exist")]
        public async Task DeleteMealAsync_NotExistentMeal_MealDoesNotExistExceptionThrown()
        {
            //act
            await this.mealService.DeleteMealAsync(1);
        }

        [TestMethod]
        public async Task DeleteMealAsync_ExistentMeal_MealDeleted()
        {
            //arrange
            var mealID = 1;
            var meal = Mock.Of<IMeal>();
            var mealTask = Task.FromResult(meal);

            this.mealDaoMock.Setup(d => d.GetMealByIDAsync(mealID)).Returns(mealTask);

            //act
            await this.mealService.DeleteMealAsync(1);

            //assert
            mealDaoMock.Verify(d => d.DeleteAsync(meal));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName parameter cannot be null")]
        public async Task IsOwnerOfMealAsync_AuthorizedUserNameIsNull_ArgumentNullExceptionThrown()
        {
            //arrange
            string authorizedUserName = null;
            int mealID = 1;

            //act
            await this.mealService.IsOwnerOfMealAsync(authorizedUserName, mealID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName parameter cannot be empty")]
        public async Task IsOwnerOfMealAsync_AuthorizedUserNameIsEmpty_ArgumentNullExceptionThrown()
        {
            //arrange
            string userName = "";
            int mealID = 1;

            //act
            await this.mealService.IsOwnerOfMealAsync(userName, mealID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MealID parameter should be more than 0")]
        public async Task IsOwnerOfMealAsync_MealIDEquals0_ArgumentExceptionThrown()
        {
            //arrange
            string userName = "asd";
            int mealID = 0;

            //act
            await this.mealService.IsOwnerOfMealAsync(userName, mealID);
        }

        [TestMethod]
        [ExpectedException(typeof(MealDoesNotExistException))]
        public async Task IsOwnerOfMealAsync_UserDoesNotExist_MealDoesNotExistExceptionThrown()
        {
            //arrange
            string userName = "asd";
            int mealID = 1;

            //act
            await this.mealService.IsOwnerOfMealAsync(userName, mealID);
        }

        [TestMethod]
        public async Task IsOwnerOfMealAsync_NotUserMeal_FalseReturned()
        {
            //arrange
            string userName = "asd";
            int mealID = 1;

            var userTask = Task.FromResult(Mock.Of<IUser>());

            this.userDaoMock.Setup(d => d.GetUserByNameAsync(userName)).Returns(userTask);

            //act
            var actual = await this.mealService.IsOwnerOfMealAsync(userName, mealID);

            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public async Task IsOwnerOfMealAsync_IsUserMeal_TrueReturned()
        {
            //arrange
            string userName = "asd";
            int mealID = 1;

            var userTask = Task.FromResult(Mock.Of<IUser>());
            var trueTask = Task.FromResult(true);

            this.userDaoMock.Setup(d => d.GetUserByNameAsync(userName)).Returns(userTask);
            this.mealDaoMock.Setup(d => d.ContainsAsync(It.IsAny<Expression<Func<IMeal, bool>>>())).Returns(trueTask);

            //act
            var actual = await this.mealService.IsOwnerOfMealAsync(userName, mealID);

            //assert
            Assert.IsTrue(actual);
        }

        private InMealDto GetValidMealDto()
        {
            return new InMealDto
            {
                Text = "Soup",
                Calories = 10,
                EatingDate = DateTime.Now
            };
        }

        private InMealReportFilterDto GetValidFilter()
        {
            return new InMealReportFilterDto
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddDays(1),
                TimeFrom = DateTime.Now,
                TimeTo = DateTime.Now
            };
        }
    }
}
