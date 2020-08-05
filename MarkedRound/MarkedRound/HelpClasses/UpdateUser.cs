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
        public static bool ChangeUserInput(int id, string Document, string StrInput, int? IntInput, string collection)
        {
            try
            {
                //DB
                IMongoDatabase client = new MongoClient($"mongodb://{"adminUser"}:{"silvereye"}@localhost:27017").GetDatabase("silkevejen");
                var BsonColl = client.GetCollection<BsonDocument>(collection);

                //Section for changing users input on DB
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
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
