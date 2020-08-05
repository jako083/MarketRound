using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using MarkedRound.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static MarkedRound.HelpClasses.UpdateUser;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

/*Notes:
 * Update Method:  ChangeUserInput(id, Document, StrInput, IntInput, collection)
 * Exempel: ChangeUserInput(1, "PhoneNumber", null, 6666, "Users");
 * 
 */




namespace MarkedRound.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        public static List<UserModel> GetAllUsers(int? id, string section)
        {
            /* Azure connectionstring
             @"mongodb://markedround:ssJnR833qFMonYSH6h3iYXwiCGGQ06SgvbPW72LKstejR1lGUWtCy5eZG7qzNPO00xVKhiC5jNVUo8oUge5p6Q==@markedround.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@markedround@?";
            */
                       
            //Database Connection
               IMongoDatabase client = new MongoClient($"mongodb://{"adminUser"}:{"silvereye"}@localhost:27017").GetDatabase("silkevejen");

            //Collection Query
            IMongoQueryable<UserModel> usageQuery;
            if (id == null)
            {
                //Get all users if no id is specified
                usageQuery = from c in client.GetCollection<UserModel>(section).AsQueryable()
                                select c;
            }
            else
            {
                //Get a specific user 
                usageQuery = from c in client.GetCollection<UserModel>(section).AsQueryable()
                             where c._id == id
                            select c;
            }
            var ListOfUsers = new List<UserModel>();
            ListOfUsers.AddRange(usageQuery);
            return ListOfUsers;
        }

        // GET: api/<MainController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(GetAllUsers(1, "Users"));
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
