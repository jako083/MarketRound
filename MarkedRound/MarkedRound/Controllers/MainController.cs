using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarkedRound.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkedRound.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        public static IMongoCollection<BsonDocument> BsonCollection(string user, string pass, string section)
        {

            IMongoDatabase client = new MongoClient($"mongodb://{user}:{pass}@localhost:27017").GetDatabase("silkevejen");

            return client.GetCollection<BsonDocument>(section);
        }
        public static List<List<UserClass>> testMethod()
        {
            //   BsonCollection("admin", "password", "Users");
            IMongoDatabase client = new MongoClient($"mongodb://{"admin"}:{"password"}@localhost:27017").GetDatabase("silkevejen");

            var userQuery = from c in client.GetCollection<UserClass>("Users").AsQueryable()
                            select c;
            var test = new List<List<UserClass>>();
            foreach (UserClass output in userQuery)
            {
                List<UserClass> userClasses = new List<UserClass> { output };
                test.Add(userClasses);
            }
                return test;
        }

            // GET: api/<MainController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            testMethod();
            return new string[] { "value12", "value2" };
        }

        // GET api/<MainController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MainController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MainController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MainController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
