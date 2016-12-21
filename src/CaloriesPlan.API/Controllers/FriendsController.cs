using CaloriesPlan.API.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CaloriesPlan.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/friends")]
    public class FriendsController : ControllerBase
    {
        private static List<AUser> users = new List<AUser> {
                    new AUser {
                        ID = 125,
                        UserName = "john.smith123",
                        Password = "123456",
                        FirstName = "John",
                        LastName = "Smith",
                        Address = "Redmond, WA 98052-6399. UNITED STATES.",
                        DateOfBirth = DateTime.Now.AddYears(-38)
                    },
                    new AUser {
                        ID = 168,
                        UserName = "john.doe456",
                        Password = "987654",
                        FirstName = "John",
                        LastName = "Doe",
                        Address = "Cupertino, CA 95014. UNITED STATES.",
                        DateOfBirth = DateTime.Now.AddYears(-37)
                    },
                };

        //GET api/friends
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return this.Ok(users);
        }

        //POST api/friends
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(AUser user)
        {
            users.Add(user);
            return this.Ok();
        }

        //DELETE api/friends/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.ID == id);
            if (user != null)
            {
                users.Remove(user);
                return this.Ok();
            }
            else
            {
                return this.NotFound();
            }
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
}
