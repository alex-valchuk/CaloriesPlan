using System.Web.Http;

using CaloriesPlan.DTO.In;
using CaloriesPlan.BLL.Services;
using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.API.Filters;
using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/meals")]
    public class MealsController : ControllerBase
    {
        private readonly IMealService mealService;

        public MealsController(IMealService mealService)
        {
            this.mealService = mealService;
        }

        //GET api/meals/
        [HttpGet]
        [Route("")]
        [AuthorizedOrOwnerParamOrIsInOneOfRoles(AuthorizationParams.RoleAdmin)]
        public IHttpActionResult Get(
            [FromUri] string userName = null,
            [FromUri] InMealReportFilterDto filter = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            var userMeals = this.mealService.GetUserNutritionReport(userName, filter);

            return this.Ok(userMeals);
        }

        //GET api/meals/{userName}/meal/{id}
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin)]
        [Route(ParamUserName + "/meal/" + ParamID)]
        public IHttpActionResult Get(int id)
        {
            var meal = this.mealService.GetMealByID(id);
            return this.Ok(meal);
        }

        //POST api/meals/{userName}/meal
        [HttpPost]
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin)]
        [Route(ParamUserName + "/meal")]
        public IHttpActionResult Post(string userName, InMealDto mealDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.mealService.CreateMeal(userName, mealDto);
            return this.Ok();
        }

        //PUT api/meals/{userName}/meal/{id}
        [HttpPut]
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin)]
        [Route(ParamUserName + "/meal/" + ParamID)]
        public IHttpActionResult Put(int id, InMealDto mealDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.mealService.UpdateMeal(id, mealDto);
            return this.Ok();
        }

        //DELETE api/meals/{userName}/meal/{id}
        [HttpDelete]
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin)]
        [Route(ParamUserName + "/meal/" + ParamID)]
        public IHttpActionResult Delete(int id)
        {
            this.mealService.DeleteMeal(id);
            return this.Ok();
        }
    }
}
