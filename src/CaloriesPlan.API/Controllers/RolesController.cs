﻿using System.Threading.Tasks;
using System.Web.Http;

using CaloriesPlan.BLL.Services.Abstractions;
using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.API.Filters;
using CaloriesPlan.UTL.Const;
using CaloriesPlan.DTO.In;

namespace CaloriesPlan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IAccountService accountService = null;

        public RolesController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //GET api/roles/?userName&getUserRoles
        [HttpGet]
        [AuthorizedInParamOrHasOneOfRoles(AuthorizationParams.RoleAdmin, AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Get([FromUri] string userName = null, [FromUri] bool getUserRoles = true)
        {
            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            var userRoles = (getUserRoles)
                ? await this.accountService.GetUserRolesAsync(userName)
                : await this.accountService.GetNotUserRolesAsync(userName);

            return this.Ok(userRoles);
        }

        //POST api/roles/?userName
        [HttpPost]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Post(InRoleDto roleDto, [FromUri] string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            await this.accountService.AddUserRoleAsync(userName, roleDto.RoleName);
            return this.Ok();
        }

        //DELETE api/roles/{roleName}/?userName
        [HttpDelete]
        [Route(ParamRoleName)]
        [Authorize(Roles = AuthorizationParams.RoleAdmin + "," + AuthorizationParams.RoleManager)]
        public async Task<IHttpActionResult> Delete(string roleName, [FromUri] string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
                userName = this.User.Identity.Name;

            await this.accountService.DeleteUserRoleAsync(userName, roleName);
            return this.Ok();
        }
    }
}
