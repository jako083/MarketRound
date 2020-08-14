using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using MarketRound.Model;
using MongoDB.Driver;
using static MarketRound.HelpClasses.HashingSalting;

namespace MarketRound.HelpClasses
{
    public class CreateUser
    {
        public static string CreateUserSecton(string StrCollection, UserModel user, IMongoDatabase client)
        {
               try
               {
            var Hashsalt = HashSaltValues(user.password, null);
            var currentTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                var newUser = new BsonDocument
                {
                    {"username", user.username },
                    {"password", Hashsalt.Pass },
                    {"salt", Hashsalt.Salt },
                    {"firstName", user.firstName },
                    {"lastName", user.lastName },
                    {"phoneNumber", user.phoneNumber },
                    {"country", user.country },
                    {"city", user.city },
                    {"address", user.address },
                    {"ongoingSales", new BsonArray() },
                    {"salesHistory", new BsonArray() },
                    {"reviews", new BsonArray() },
                    {"failedLoginAttempts", new BsonArray() },
                    {"loginBan", ""},
                    {"creationDate", currentTime}
                };
                var collection = client.GetCollection<BsonDocument>(StrCollection);
                collection.InsertOne(newUser);
                
                return "Success";
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }
    }
}
