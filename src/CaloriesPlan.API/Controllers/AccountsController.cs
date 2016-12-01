using System.Web.Http;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.DTO.In;
using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.API.Filters;
using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Controllers
{
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
        public IHttpActionResult Get()
        {
            var accounts = this.accountService.GetAccounts();
            return this.Ok(accounts);
        }

        //GET api/accounts/:userName
        [HttpGet]
        [Route(ParamUserName)]
        [AuthorizedInRouteOrHasOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        public IHttpActionResult Get(string userName)
        {
            var account = this.accountService.GetAccount(userName);
            return this.Ok(account);
        }

        //POST api/accounts/signup
        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        public IHttpActionResult Post(InSignUpDto signUpDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.accountService.SignUp(signUpDto);
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
        public IHttpActionResult Put(string userName, InAccountDto accountDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.accountService.UpdateAccount(userName, accountDto);
            return this.Ok();
        }

        //DELETE api/accounts/:userName
        [HttpDelete]
        [Route(ParamUserName)]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        public IHttpActionResult Delete(string userName)
        {
            var authenticatedName = this.User.Identity.Name;
            if (authenticatedName.ToLower() == userName.ToLower())
            {
                return this.BadRequest("User cannot delete himself");
            }

            this.accountService.DeleteAccount(userName);
            return this.Ok();
        }
    }
}
