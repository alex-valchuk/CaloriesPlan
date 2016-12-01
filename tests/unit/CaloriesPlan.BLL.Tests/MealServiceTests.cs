using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        public void GetUserNutritionReport_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //act
            this.mealService.GetUserNutritionReport(null, null);
        }
        [TestMethod]
        [ExpectedException(typeof(AccountDoesNotExistException), "User does not exist")]
        public void GetUserNutritionReport_OfNotExistedUser_AccountDoesNotExistExceptionThrown()
        {
            //arrange
            var userName = "Test";
            var filter = this.GetValidFilter();

            //act
            this.mealService.GetUserNutritionReport(userName, filter);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDateRangeException), "DateFrom should be less or equal to DateTo")]
        public void GetUserNutritionReport_DateFromMoreThanDateTo_InvalidDateRangeExceptionThrown()
        {
            //arrange
            var userName = "Alex";
            this.userDaoMock.Setup(ud => ud.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());

            var filter = this.GetValidFilter();
            filter.DateFrom = filter.DateTo.Value.AddDays(1);

            //act
            this.mealService.GetUserNutritionReport(userName, filter);
        }

        [TestMethod]
        public void GetUserNutritionReport_ValidInput_ReportReturned()
        {
            //arrange
            var userName = "Alex";
            this.userDaoMock.Setup(ud => ud.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());

            var filter = this.GetValidFilter();

            //act
            var report = this.mealService.GetUserNutritionReport(userName, filter);

            //assert
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void GetUserNutritionReport_UserDoesNotHaveMeals_ReportWithoutMealsReturned()
        {
            //arrange
            var userName = "Alex";
            this.userDaoMock.Setup(ud => ud.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());

            var filter = this.GetValidFilter();

            //act
            var report = this.mealService.GetUserNutritionReport(userName, filter);

            //assert
            Assert.IsNull(report.Meals);
        }

        [TestMethod]
        public void GetUserNutritionReport_UserHasMeals_ReportWithMealsReturned()
        {
            //arrange
            var userName = "Alex";
            this.userDaoMock.Setup(ud => ud.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());

            var filter = this.GetValidFilter();

            var dbMeals = new List<IMeal>
            {
                Mock.Of<IMeal>()
            };
            this.mealDaoMock.Setup(md => md.GetMealsByUserName(
                It.IsAny<string>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(dbMeals);

            //act
            var report = this.mealService.GetUserNutritionReport(userName, filter);

            //assert
            Assert.IsNotNull(report.Meals);
        }

        [TestMethod]
        public void GetMealByID_MealDoesNotExists_NullReturned()
        {
            //act
            var meal = this.mealService.GetMealByID(It.IsAny<int>());

            //assert
            Assert.IsNull(meal);
        }

        [TestMethod]
        public void GetMealByID_MealExists_MealReturned()
        {
            //arrange
            this.mealDaoMock.Setup(md => md.GetMealByID(It.IsAny<int>())).Returns(Mock.Of<IMeal>());

            //act
            var meal = this.mealService.GetMealByID(It.IsAny<int>());

            //assert
            Assert.IsNotNull(meal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName not specified")]
        public void CreateMeal_EmptyUserName_ArgumentNullExceptionThrown()
        {
            //act
            this.mealService.CreateMeal(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified")]
        public void CreateMeal_EmptyMealDto_ArgumentNullExceptionThrown()
        {
            //act
            this.mealService.CreateMeal("Alex", null);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountDoesNotExistException), "Account does not exist")]
        public void CreateMeal_ForNotExistentUser_AccountDoesNotExistExceptionThrown()
        {
            //act
            this.mealService.CreateMeal("Alex", Mock.Of<InMealDto>());
        }

        [TestMethod]
        public void CreateMeal_ForExistentUser_NewMealCreated()
        {
            //arrange
            this.userDaoMock.Setup(d => d.GetUserByName(It.IsAny<string>())).Returns(Mock.Of<IUser>());
            this.mealDaoMock.Setup(d => d.NewMealInstance()).Returns(Mock.Of<IMeal>());

            //act
            this.mealService.CreateMeal("Alex", Mock.Of<InMealDto>());

            //assert
            this.mealDaoMock.Verify(d => d.Create(It.IsAny<IMeal>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public void UpdateMeal_EmptyMealDto_ArgumentNullExceptionThrown()
        {
            //act
            this.mealService.UpdateMeal(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public void UpdateMeal_EmptyMealText_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();
            meal.Text = null;

            //act
            this.mealService.UpdateMeal(1, meal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public void UpdateMeal_EmptyMealCalories_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();
            meal.Calories = null;

            //act
            this.mealService.UpdateMeal(1, meal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Meal not specified correctly")]
        public void UpdateMeal_EmptyMealEatingDate_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();
            meal.EatingDate = null;

            //act
            this.mealService.UpdateMeal(1, meal);
        }

        [TestMethod]
        [ExpectedException(typeof(MealDoesNotExistException), "Meal does not exist")]
        public void UpdateMeal_MealDoesNotExist_ArgumentNullExceptionThrown()
        {
            //arrange
            var meal = this.GetValidMealDto();

            //act
            this.mealService.UpdateMeal(1, meal);
        }

        [TestMethod]
        public void UpdateMeal_ValidInput_MealUpdated()
        {
            //arrange
            var meal = this.GetValidMealDto();
            this.mealDaoMock.Setup(d => d.GetMealByID(It.IsAny<int>())).Returns(Mock.Of<IMeal>());

            //act
            this.mealService.UpdateMeal(1, meal);

            //assert
            this.mealDaoMock.Verify(d => d.Update(It.IsAny<IMeal>()));
        }

        [TestMethod]
        [ExpectedException(typeof(MealDoesNotExistException), "Meal does not exist")]
        public void DeleteMeal_NotExistentMeal_MealDoesNotExistExceptionThrown()
        {
            //act
            this.mealService.DeleteMeal(1);
        }

        [TestMethod]
        public void DeleteMeal_ExistentMeal_MealDeleted()
        {
            //arrange
            var mealID = 1;
            var meal = Mock.Of<IMeal>();
            this.mealDaoMock.Setup(d => d.GetMealByID(mealID)).Returns(meal);

            //act
            this.mealService.DeleteMeal(1);

            //assert
            mealDaoMock.Verify(d => d.Delete(meal));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName parameter cannot be null")]
        public void CanObtainMeal_AuthorizedUserNameIsNull_ArgumentNullExceptionThrown()
        {
            //arrange
            string authorizedUserName = null;
            int mealID = 1;

            //act
            this.mealService.IsOwnerOfMeal(authorizedUserName, mealID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserName parameter cannot be empty")]
        public void IsOwnerOfMeal_AuthorizedUserNameIsEmpty_ArgumentNullExceptionThrown()
        {
            //arrange
            string userName = "";
            int mealID = 1;

            //act
            this.mealService.IsOwnerOfMeal(userName, mealID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MealID parameter should be more than 0")]
        public void IsOwnerOfMeal_MealIDEquals0_ArgumentNullExceptionThrown()
        {
            //arrange
            string userName = "asd";
            int mealID = 0;

            //act
            this.mealService.IsOwnerOfMeal(userName, mealID);
        }

        [TestMethod]
        [ExpectedException(typeof(MealDoesNotExistException))]
        public void IsOwnerOfMeal_UserDoesNotExist_MealDoesNotExistExceptionThrown()
        {
            //arrange
            string userName = "asd";
            int mealID = 1;

            //act
            this.mealService.IsOwnerOfMeal(userName, mealID);
        }

        [TestMethod]
        public void IsOwnerOfMeal_NotUserMeal_FalseReturned()
        {
            //arrange
            string userName = "asd";
            int mealID = 1;

            this.userDaoMock.Setup(d => d.GetUserByName(userName)).Returns(Mock.Of<IUser>());
            //this.mealDaoMock.Setup(d => d.Contains(It.IsAny<Expression<Func<IMeal, bool>>>())).Returns(false);

            //act
            var actual = this.mealService.IsOwnerOfMeal(userName, mealID);

            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsOwnerOfMeal_IsUserMeal_TrueReturned()
        {
            //arrange
            string userName = "asd";
            int mealID = 1;

            this.userDaoMock.Setup(d => d.GetUserByName(userName)).Returns(Mock.Of<IUser>());
            this.mealDaoMock.Setup(d => d.Contains(It.IsAny<Expression<Func<IMeal, bool>>>())).Returns(true);

            //act
            var actual = this.mealService.IsOwnerOfMeal(userName, mealID);

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
