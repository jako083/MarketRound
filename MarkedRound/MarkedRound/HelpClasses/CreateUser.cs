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
            try
            {
                var newUser = new BsonDocument
                {
                    {"_id", user._id },
                    {"Username", user.Username },
                    {"Password", user.Password },
                    {"Salt", user.Salt },
                    {"FirstName", user.FirstName },
                    {"LastName", user.LastName },
                    {"PhoneNumber", user.PhoneNumber },
                    {"Country", user.Country },
                    {"City", user.City },
                    {"Address", user.Address },
                    {"Ongoingsales", new BsonArray() },
                    {"SalesHistory", new BsonArray() },
                    {"Reviews", new BsonArray() }
                };
                var collection = client.GetCollection<BsonDocument>(StrCollection);
            //    collection.InsertOne(newUser);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
