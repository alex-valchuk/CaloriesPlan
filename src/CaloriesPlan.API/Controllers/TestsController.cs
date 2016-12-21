using System;
using System.Linq;
using System.Web.Http;

using CaloriesPlan.API.Controllers.Base;
using CaloriesPlan.BLL.Services.Abstractions;
using System.Collections.Generic;

namespace CaloriesPlan.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/tests")]
    public class TestsController : ControllerBase
    {
        private readonly IAccountService accountService = null;

        public TestsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //GET api/tests/friends_r
        [HttpGet]
        [Route("friends_r")]
        public IHttpActionResult GetUsers([FromUri] int? userID = null)
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

        //GET api/tests/profile
        [HttpGet]
        [Route("profile")]
        public IHttpActionResult GetUserProfile([FromUri] int? userID = null)
        {
            var profile =
                new UserProfileDto
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Address = "Redmond, WA 98052-6399. UNITED STATES.",
                    DateOfBirth = DateTime.Now.AddYears(-38)
                };

            return this.Ok(profile);
        }

        [HttpPut]
        [Route("student")]
        public IHttpActionResult Put(int id, Student student)
        {
            return this.Ok();
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

        public class Student
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}
