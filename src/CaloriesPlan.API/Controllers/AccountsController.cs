using System;
using System.Web.Http;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Exceptions;
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

        //POST api/accounts/signup
        [AllowAnonymous]
        [Route("signup")]
        [HttpPost]
        public IHttpActionResult SignUp(InSignUpDto signUpDto)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this.accountService.SignUp(signUpDto);
                return this.Ok();
            }
            catch (PropertyInconsistencyException ex)
            {
                //add logging functionality
                return this.BadProperty(ex);
            }
            catch (RegistrationException ex)
            {
                //add logging functionality
                return this.BadRegistration(ex);
            }
            catch (ArgumentNullException ex)
            {
                //add logging functionality
                return this.BadArgument(ex);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //POST api/accounts/signout
        [AllowAnonymous]
        [Route("signout")]
        [HttpPost]
        public IHttpActionResult SignOut()
        {
            try
            {
                //TODO:

                return this.Ok();
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //GET api/accounts
        [HttpGet]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        public IHttpActionResult Get()
        {
            try
            {
                var accounts = this.accountService.GetAccounts();
                return this.Ok(accounts);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //GET api/accounts/{userName}/user-roles
        [HttpGet]
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        [Route(ParamUserName + "/user-roles")]
        public IHttpActionResult GetUserRoles(string userName)
        {
            try
            {
                var userRoles = this.accountService.GetUserRoles(userName);
                return this.Ok(userRoles);
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

        //GET api/accounts/{userName}/not-user-roles
        [HttpGet]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        [Route(ParamUserName + "/not-user-roles")]
        public IHttpActionResult GetNotUserRoles(string userName)
        {
            try
            {
                var userRoles = this.accountService.GetNotUserRoles(userName);
                return this.Ok(userRoles);
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

        //GET api/accounts/:userName
        [HttpGet]
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        [Route(ParamUserName)]
        public IHttpActionResult Get(string userName)
        {
            try
            {
                var account = this.accountService.GetAccount(userName);
                return this.Ok(account);
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

        //PUT api/accounts/:userName
        [HttpPut]
        [OwnerRouteOrIsInOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        [Route(ParamUserName)]
        public IHttpActionResult Put(string userName, InAccountDto accountDto)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this.accountService.UpdateAccount(userName, accountDto);
                return this.Ok();
            }
            catch (AccountDoesNotExistException)
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

        //DELETE api/accounts/:userName
        [HttpDelete]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        [Route(ParamUserName)]
        public IHttpActionResult Delete(string userName)
        {
            try
            {
                var authenticatedName = this.User.Identity.Name;
                if (authenticatedName.ToLower() == userName.ToLower())
                {
                    return this.BadRequest("User cannot delete himself");
                }

                this.accountService.DeleteAccount(userName);
                return this.Ok();
            }
            catch (AccountDoesNotExistException)
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

        //POST api/accounts/{userName}/user-roles/{roleName}
        [HttpPost]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        [Route(ParamUserName + "/user-roles/" + ParamRoleName)]
        public IHttpActionResult Post(string userName, string roleName)
        {
            try
            {
                this.accountService.AddUserRole(userName, roleName);
                return this.Ok();
            }
            catch (AccountDoesNotExistException)
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

        //DELETE api/accounts/{userName}/user-roles/{roleName}
        [HttpDelete]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        [Route(ParamUserName + "/user-roles/" + ParamRoleName)]
        public IHttpActionResult Delete(string userName, string roleName)
        {
            try
            {
                /*var authenticatedName = this.User.Identity.Name;
                if (authenticatedName.ToLower() == userName.ToLower())
                {
                    return this.BadRequest("User cannot delete himself");
                }*/

                this.accountService.DeleteUserRole(userName, roleName);
                return this.Ok();
            }
            catch (AccountDoesNotExistException)
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

        //GET api/accounts/{userName}/subscribers
        [HttpGet]
        [Route(ParamUserName + "/subscribers")]
        public IHttpActionResult GetSubscribers(string userName)
        {
            try
            {
                var subscribers = this.accountService.GetSubscribers(userName);
                return this.Ok(subscribers);
            }
            catch (AccountDoesNotExistException)
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
    }
}
