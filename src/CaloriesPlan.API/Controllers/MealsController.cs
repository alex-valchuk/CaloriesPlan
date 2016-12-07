using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Get(
            [FromUri] string userName = null,
            [FromUri] InMealReportFilterDto filter = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            var userMeals = await this.mealService.GetUserNutritionReportAsync(userName, filter);

            return this.Ok(userMeals);
        }

        //GET api/meals/{id}
        [Route(ParamID)]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (await this.IsAuthorizedUserAnAdminOrOwnerOfMeal(id))
            {
                var meal = await this.mealService.GetMealByIDAsync(id);
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
        public async Task<IHttpActionResult> Post(InMealDto mealDto, [FromUri] string userName = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            await this.mealService.CreateMealAsync(userName, mealDto);
            return this.Ok();
        }

        //PUT api/meals/{id}
        [HttpPut]
        [Route(ParamID)]
        public async Task<IHttpActionResult> Put(int id, InMealDto mealDto)
        {
            if (await this.IsAuthorizedUserAnAdminOrOwnerOfMeal(id))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                await this.mealService.UpdateMealAsync(id, mealDto);
                return this.Ok();
            }

            return this.Unauthorized();
        }

        //DELETE api/meals/{id}
        [HttpDelete]
        [Route(ParamID)]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (await this.IsAuthorizedUserAnAdminOrOwnerOfMeal(id))
            {
                await this.mealService.DeleteMealAsync(id);
                return this.Ok();
            }

            return this.Unauthorized();
        }

        private async Task<bool> IsAuthorizedUserAnAdminOrOwnerOfMeal(int mealID)
        {
            return
                this.IsAuthorizedUserAnAdmin() ||
                await this.mealService.IsOwnerOfMealAsync(this.User.Identity.Name, mealID);
        }
    }
}
