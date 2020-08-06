using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using MarkedRound.Model;
using MongoDB.Driver;

namespace MarkedRound.HelpClasses
{
    public class CreateUser
    {
        public static bool CreaUserSecton(string StrCollection, UserModel user, IMongoDatabase client)
        {
         //   try
           // {
                var newUser = new BsonDocument
                {
                 //   {"_id", user._id },
                    {"username", user.username },
                    {"password", user.password },
                    {"salt", user.salt },
                    {"firstName", user.firstName },
                    {"lastName", user.lastName },
                    {"phoneNumber", user.phoneNumber },
                    {"country", user.country },
                    {"city", user.city },
                    {"address", user.address },
                    {"ongoingSales", new BsonArray() },
                    {"salesHistory", new BsonArray() },
                    {"reviews", new BsonArray() }
                };
                var collection = client.GetCollection<BsonDocument>(StrCollection);
                collection.InsertOne(newUser);
                
                return true;
         /*   }
            catch
            {
                return false;
            }*/
        }
    }
}
