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
using MarkedRound.HelpClasses;

namespace MarketRound.HelpClasses
{
    public class CreateUser
    {
        public static string CreateUserSecton(string StrCollection, UserModel user, IMongoDatabase client)
        {
               try
               {
                Encryptor _encrypter = new Encryptor("4556484548529632");
                var Hashsalt = HashSaltValues(user.password, null);
                user.password = Hashsalt.Pass;
                var EncryptedUser =_encrypter.ObjectToEncryptDecrypt(user, Hashsalt.Salt, "Encrypt");


            var currentTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                var newUser = new BsonDocument
                {
                    {"username", EncryptedUser.username },
                    {"password", EncryptedUser.password },
                    {"salt", Hashsalt.Salt },
                    {"firstName", EncryptedUser.firstName },
                    {"lastName", EncryptedUser.lastName },
                    {"phoneNumber", user.phoneNumber },
                    {"country", EncryptedUser.country },
                    {"city", EncryptedUser.city },
                    {"address", EncryptedUser.address },
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
