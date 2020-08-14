﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using MarketRound.HelpClasses;
using MarketRound.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Net;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;

using static MarketRound.HelpClasses.UpdateUser;
using static MarketRound.HelpClasses.CreateUser;
using static MarketRound.HelpClasses.LoginClasses;
using MarkedRound.Model;

//TODO
//isnullorempty
//regex for security?
//Try catches?
//___________________________________________________________________________________________

/*Notes:
 * Update Method:  ChangeUserInput(id, Document, StrInput, IntInput, collection)
 * Exempel: ChangeUserInput(1, "PhoneNumber", null, 6666, "Users");
 * 
 * Add new User method: CreateUserSecton(Collection, UserModel, DBClient);
 */
namespace MarketRound.Controllers
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
        public ActionResult Post([FromBody] Usermodel_Post user)
        {
            switch (user.section)
            {
                case "createUser":
                    var result = CreateUserSecton("Users", user.userContent, client);
                    if (result != "Success")
                    {
                        //Exception error
                        return Ok(result);
                    }
                    return Ok();
                case "login":
                    // Example: https://gyazo.com/ceb108a3cf4755b641828e5d445b152c
                    var dbUser = GetAllUsers(user.userContent.username, "Users");
                    return Ok(login(user.userContent, dbUser));
            }
            return Ok("Error, invalid Section detected!");
        }

        // PUT api/<MainController>/5
        [HttpPut]
        public ActionResult Put([FromBody] ChangeUserModel changeUser)
        {
            if (changeUser.key.Length != changeUser.change.Length || changeUser.dataType.Length != changeUser.key.Length)
                return Ok("Missing Input");
            try
            {
                var result = false;
                for (int i = 0; i < changeUser.key.Length; i++)
                {
                    switch (changeUser.dataType[i])
                    {
                        case "string":
                          result = ChangeUserInput(changeUser.username, changeUser.collection, changeUser.key[i], changeUser.change[i], null, null, null);
                            break;
                        case "int":
                        result = ChangeUserInput(changeUser.username, changeUser.collection, changeUser.key[i], null, Convert.ToInt32(changeUser.change[i]), null, null);
                            break;
                        default:
                            break;
                    }
                }
                    return Ok(result);
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
