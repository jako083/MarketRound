using MarkedRound.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkedRound.HelpClasses
{
    public class CreateComments
    {
        public static bool createComment(IMongoDatabase client, ObjectId productId, ObjectId sellerId)
        {
            var newComments = new BsonDocument
            {
                {"productId" , productId},
                {"sellerId" , sellerId},
                {"comments" , new BsonArray()},
            };

            var collection = client.GetCollection<BsonDocument>("comments");
            collection.InsertOne(newComments);

            return true;
        }
    }
}
