using System.Threading.Tasks;
using System.Web.Http;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.API.Filters;
using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/subscribers")]
    public class SubscribersController : ControllerBase
    {
        private readonly IAccountService accountService = null;

        public SubscribersController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //GET api/subscribers/?userName
        [HttpGet]
        [AuthorizedInParamOrHasOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Get([FromUri] string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            var subscribers = await this.accountService.GetSubscribersAsync(userName);
            return this.Ok(subscribers);
        }
    }
}
