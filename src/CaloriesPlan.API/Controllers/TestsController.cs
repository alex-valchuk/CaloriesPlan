using System;
using System.Web.Http;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.API.Controllers.Base;

namespace CaloriesPlan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/tests")]
    public class TestsController : ControllerBase
    {
        private readonly IAccountService accountService = null;

        public TestsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //GET api/accounts/friends_w
        [HttpGet]
        [Route("friends_w")]
        public IHttpActionResult GetFriendsWrong([FromUri] int? userID = null)
        {
            try
            {
                var users = new System.Collections.Generic.List<AUser> {
                    new AUser {
                        ID = 125,
                        UserName = "john.smith123",
                        Password = "123456",
                        FirstName = "John",
                        LastName = "Smith",
                        Address = "Redmond, WA 98052-6399. UNITED STATES.",
                        DateOfBirth = DateTime.Now.AddYears(38)
                    },
                    new AUser {
                        ID = 168,
                        UserName = "john.doe456",
                        Password = "987654",
                        FirstName = "John",
                        LastName = "Doe",
                        Address = "Cupertino, CA 95014. UNITED STATES.",
                        DateOfBirth = DateTime.Now.AddYears(37)
                    },
                };

                return this.Ok(users);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //GET api/accounts/friends_r
        [HttpGet]
        [Route("friends_r")]
        public IHttpActionResult GetUsers([FromUri] int? userID = null)
        {
            try
            {
                var users = new System.Collections.Generic.List<OutUserDetailDto> {
                    new OutUserDetailDto {
                        ID = 125,
                        FirstName = "John",
                        LastName = "Smith",
                    },
                    new OutUserDetailDto {
                        ID = 168,
                        FirstName = "John",
                        LastName = "Doe",
                    },
                };

                return this.Ok(users);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        //GET api/accounts/profile
        [HttpGet]
        [Route("profile")]
        public IHttpActionResult GetUserProfile([FromUri] int? userID = null)
        {
            try
            {
                var profile =
                    new UserProfileDto {
                        FirstName = "John",
                        LastName = "Smith",
                        Address = "Redmond, WA 98052-6399. UNITED STATES.",
                        DateOfBirth = DateTime.Now.AddYears(38)
                    };

                return this.Ok(profile);
            }
            catch
            {
                //add logging functionality
                return this.InternalServerError();
            }
        }

        public class AUser
        {
            public int ID { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        public class OutUserDetailDto
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class UserProfileDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}
