using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using MarkedRound.HelpClasses;
using MarkedRound.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static MarkedRound.HelpClasses.UpdateUser;
using static MarkedRound.HelpClasses.CreateUser;
using System.Net;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

/*Notes:
 * Update Method:  ChangeUserInput(id, Document, StrInput, IntInput, collection)
 * Exempel: ChangeUserInput(1, "PhoneNumber", null, 6666, "Users");
 * 
 * Add new User method: CreateUserSecton(Collection, UserModel, DBClient);
 */
namespace MarkedRound.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        //Database Connection
        public static IMongoDatabase client = new MongoClient($"mongodb://{"adminUser"}:{"silvereye"}@localhost:27017").GetDatabase("silkevejen");

        public static List<UserModel> GetAllUsers(string username, string section)
        {
            /* Azure connectionstring
             @"mongodb://markedround:ssJnR833qFMonYSH6h3iYXwiCGGQ06SgvbPW72LKstejR1lGUWtCy5eZG7qzNPO00xVKhiC5jNVUo8oUge5p6Q==@markedround.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@markedround@?";
            */


            //Collection Query
            IMongoQueryable<UserModel> usageQuery;
            if (username == null)
            {
                //Get all users if no id is specified
                usageQuery = from c in client.GetCollection<UserModel>(section).AsQueryable()
                             select c;
            }
            else
            {
                //Get a specific user 
                usageQuery = from c in client.GetCollection<UserModel>(section).AsQueryable()
                             where c.username == username
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
            return Ok(GetAllUsers(null, "Users"));
        }

        // GET api/<MainController>/5
        [HttpGet("{username}")]
        public ActionResult Get(string username)
        {
            return Ok(GetAllUsers(username, "Users"));
        }

        // POST api/<MainController>
        [HttpPost]
        public ActionResult Post([FromBody] UserModel user)
        {
            //password hashing
            user.password = "YEs";

            //pasword salting
            user.salt = "salt";
            CreateUserSecton("Users", user, client);
            return Ok("Test");
        }

        // PUT api/<MainController>/5
        [HttpPut]
        public ActionResult Put([FromBody] ChangeUserModel changeUser)
        {
            if (changeUser.key.Length != changeUser.change.Length || changeUser.dataType.Length != changeUser.key.Length)
                return Ok("Missing Input");

            try
            {
                for (int i = 0; i < changeUser.key.Length; i++)
                {
                    switch (changeUser.dataType[i])
                    {
                        case "string":
                            ChangeUserInput(changeUser.username, changeUser.collection, changeUser.key[i], changeUser.change[i], null);
                            break;
                        case "int":
                            ChangeUserInput(changeUser.username, changeUser.collection, changeUser.key[i], null, Convert.ToInt32(changeUser.change[i]));
                            break;
                        default:
                            break;
                    }
                }
                    return Ok("Success");
            }
            catch
            {
                return Ok("Error");
            }
        }

        // DELETE api/<MainController>/5
        [HttpDelete("{username}")]
        public ActionResult Delete(string username)
        {
            var collection = client.GetCollection<BsonDocument>("Users");
            var deletefilter = Builders<BsonDocument>.Filter.Eq("username", username);

            collection.DeleteOne(deletefilter);
            return Ok("Success");
        }
    }
}
