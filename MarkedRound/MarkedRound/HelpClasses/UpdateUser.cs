using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkedRound.HelpClasses
{
    public class UpdateUser
    {
        public static bool ChangeUserInput(string username, string collection, string Document, string StrInput, int? IntInput)
        {
            try
            {
                //Gets database then selects the collection called
                IMongoDatabase client = new MongoClient($"mongodb://{"adminUser"}:{"silvereye"}@localhost:27017").GetDatabase("silkevejen");
                
                //Collection to edit
                var BsonColl = client.GetCollection<BsonDocument>(collection);

                //Filter for changes
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
                
                UpdateDefinition<BsonDocument> update;
                if (StrInput != null)
                {
                    // Used for when a string value is given
                    update = Builders<BsonDocument>.Update.Set(Document, StrInput);
                }
                else
                {
                    // Used for when an int value is given
                    update = Builders<BsonDocument>.Update.Set(Document, IntInput);
                }
                BsonColl.UpdateOne(filter, update);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
