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
        [AuthorizedInParamOrHasOneOfRoles(AuthorizationParams.RoleAdmin)]
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

        //GET api/meals/{id}
        [Route(ParamID)]
        public IHttpActionResult Get(int id)
        {
            if (this.IsAuthorizedUserAnAdmin() ||
                this.mealService.IsOwnerOfMeal(this.User.Identity.Name, id))
            {
                var meal = this.mealService.GetMealByID(id);

                if (meal != null)
                {
                    return this.Ok(meal);
                }

                return this.NotFound();
            }

            return this.Unauthorized();
        }

        //POST api/meals/?userName
        [HttpPost]
        [AuthorizedInParamOrHasOneOfRoles(AuthorizationParams.RoleAdmin)]
        public IHttpActionResult Post(InMealDto mealDto, [FromUri] string userName = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            this.mealService.CreateMeal(userName, mealDto);
            return this.Ok();
        }

        //PUT api/meals/{id}
        [HttpPut]
        [Route(ParamID)]
        public IHttpActionResult Put(int id, InMealDto mealDto)
        {
            if (this.IsAuthorizedUserAnAdmin() ||
                this.mealService.IsOwnerOfMeal(this.User.Identity.Name, id))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this.mealService.UpdateMeal(id, mealDto);
                return this.Ok();
            }

            return this.Unauthorized();
        }

        //DELETE api/meals/{id}
        [HttpDelete]
        [Route(ParamID)]
        public IHttpActionResult Delete(int id)
        {
            if (this.IsAuthorizedUserAnAdmin() ||
                this.mealService.IsOwnerOfMeal(this.User.Identity.Name, id))
            {
                this.mealService.DeleteMeal(id);
                return this.Ok();
            }

            return this.Unauthorized();
        }
    }
}
