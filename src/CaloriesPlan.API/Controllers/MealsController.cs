using System.Web.Http;

using CaloriesPlan.DTO.In;
using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Exceptions;
using CaloriesPlan.API.Filters;
using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.UTL.Const;
using System;

namespace CaloriesPlan.API.Controllers
{
    [Authorize]
    [OwnerOrIsInOneOfRoles(AuthorizationParams.RoleAdmin)]
    [RoutePrefix("api/meals")]
    public class MealsController : ControllerBase
    {
        private readonly IMealService mealService;
        private readonly IAccountService accountService;

        public MealsController(IMealService foodService, IAccountService accountService)
        {
            this.mealService = foodService;
            this.accountService = accountService;
        }

        //POST api/meals/{userName}
        [HttpPost]
        [Route(ParamUserName)]
        public IHttpActionResult Report(string userName, InMealReportFilterDto filter)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var userMeals = this.mealService.GetUserNutritionReport(userName, filter);
                return this.Ok(userMeals);
            }
            catch (InvalidDateRangeException ex)
            {
                //add logging functionality
                return this.BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                //add logging functionality
                return this.BadRequest(ex.Message);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //GET api/meals/{userName}/meal/{id}
        [Route(ParamUserName + "/meal/" + ParamID)]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var meal = this.mealService.GetMealByID(id);
                return this.Ok(meal);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //POST api/meals/{userName}/meal
        [HttpPost]
        [Route(ParamUserName + "/meal")]
        public IHttpActionResult Post(string userName, InMealDto mealDto)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this.mealService.CreateMeal(userName, mealDto);
                return this.Ok();
            }
            catch (ArgumentNullException ex)
            {
                //add logging functionality
                return this.BadRequest(ex.Message);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //PUT api/meals/{userName}/meal/{id}
        [HttpPut]
        [Route(ParamUserName + "/meal/" + ParamID)]
        public IHttpActionResult Put(int id, InMealDto mealDto)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this.mealService.UpdateMeal(id, mealDto);
                return this.Ok();
            }
            catch (MealDoesNotExistException)
            {
                //add logging functionality
                return this.BadRequest();
            }
            catch (ArgumentNullException ex)
            {
                //add logging functionality
                return this.BadRequest(ex.Message);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //DELETE api/meals/{userName}/meal/{id}
        [HttpDelete]
        [Route(ParamUserName + "/meal/" + ParamID)]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.mealService.DeleteMeal(id);
                return this.Ok();
            }
            catch (MealDoesNotExistException)
            {
                //add logging functionality
                return this.BadRequest();
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }
    }
}
