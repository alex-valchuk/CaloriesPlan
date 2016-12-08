using System.Threading.Tasks;
using System.Web.Http;

using CaloriesPlan.BLL.Services.Abstractions;
using CaloriesPlan.DTO.In;
using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.API.Filters;
using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService accountService = null;

        public AccountsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //GET api/accounts
        [HttpGet]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Get()
        {
            var accounts = await this.accountService.GetAccountsAsync();
            return this.Ok(accounts);
        }

        //GET api/accounts/:userName
        [HttpGet]
        [Route(ParamUserName)]
        [AuthorizedInRouteOrHasOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Get(string userName)
        {
            var account = await this.accountService.GetUserProfileAsync(userName);
            return this.Ok(account);
        }

        //POST api/accounts/signup
        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        public async Task<IHttpActionResult> Post(InSignUpDto signUpDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.accountService.SignUpAsync(signUpDto);
            return this.Ok();
        }

        //POST api/accounts/signout
        [HttpPost]
        [Route("signout")]
        public IHttpActionResult Post()
        {
            return this.Ok();
        }

        //PUT api/accounts/:userName
        [HttpPut]
        [Route(ParamUserName)]
        [AuthorizedInRouteOrHasOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Put(string userName, InAccountDto accountDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.accountService.UpdateAccountAsync(userName, accountDto);
            return this.Ok();
        }

        //DELETE api/accounts/:userName
        [HttpDelete]
        [Route(ParamUserName)]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Delete(string userName)
        {
            var authenticatedName = this.User.Identity.Name;
            if (authenticatedName.ToLower() == userName.ToLower())
            {
                return this.BadRequest("User cannot delete himself");
            }

            await this.accountService.DeleteAccountAsync(userName);
            return this.Ok();
        }
    }
}
