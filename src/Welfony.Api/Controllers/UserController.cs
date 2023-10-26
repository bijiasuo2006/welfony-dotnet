using Microsoft.AspNetCore.Mvc;
using Welfony.Shared.Domain;

namespace Welfony.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return new User[] {
                new User {
                    Id = 1,
                    UserName = "Test 1",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1984, 10, 11)
                },
                new User {
                    Id = 2,
                    UserName = "Test 2",
                    Gender = Gender.Female,
                    DateOfBirth = new DateOnly(1983, 10, 16)
                },
            };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
