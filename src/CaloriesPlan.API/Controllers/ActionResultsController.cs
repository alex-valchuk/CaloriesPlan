using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CaloriesPlan.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/actionresults")]
    public class ActionResultsController : ApiController
    {
        [Route("BadRequest")]
        public IHttpActionResult GetBadRequestResult()
        {
            return this.BadRequest("Bad resquest result text");
        }

        [Route("Conflict")]
        public IHttpActionResult GetConflictResult()
        {
            return this.Conflict();
        }

        [Route("Content")]
        public IHttpActionResult GetContentResult()
        {
            return this.Content<int>(System.Net.HttpStatusCode.OK, 42);
        }

        [Route("Created")]
        public IHttpActionResult GetCreatedResult()
        {
            return this.Created<string>("http://google.com", "Hello World!");
        }

        [Route("CreatedAtRoute")]
        public IHttpActionResult GetCreatedAtRouteResult()
        {
            return this.CreatedAtRoute<double>("RouteWithAction", new { controller = "Tests", action = "friends_w" }, 42.42);
        }

        [Route("InternalServerError")]
        public IHttpActionResult GetInternalServerErrorResult()
        {
            return this.InternalServerError();
        }

        [Route("NotFound")]
        public IHttpActionResult GetNotFoundResult()
        {
            return this.NotFound();
        }

        [Route("Ok")]
        public IHttpActionResult GetOkResult()
        {
            return this.Ok();
        }

        [Route("Redirect")]
        public IHttpActionResult GetRedirectResult()
        {
            return this.Redirect("http://google.com");
        }

        [Route("RedirectToRoute")]
        public IHttpActionResult GetRedirectToRouteResult()
        {
            return this.RedirectToRoute("RouteWithAction", new { controller = "Tests", action = "friends_w" });
        }

        [Route("ResponseMessage")]
        public IHttpActionResult GetResponseMessageResult()
        {
            return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK));
        }

        [Route("StatusCode")]
        public IHttpActionResult GetStatusCodeResult()
        {
            return this.StatusCode(HttpStatusCode.OK);
        }

        [Route("Unauthorized")]
        public IHttpActionResult GetUnauthorizedResult()
        {
            return this.Unauthorized();
        }
    }
}