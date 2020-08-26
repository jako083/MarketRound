using MarkedRound.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MarkedRound.Controllers.ProductController;
using static MarkedRound.HelpClasses.CreateComments;

namespace MarkedRound.HelpClasses
{
    public class CreateProduct
    {
        public static bool createOffer(CreateProductModel product, IMongoDatabase client, ObjectId sellerId)
        {
            var tags = new BsonArray();
            var commentId = ObjectId.GenerateNewId();
            foreach (var item in product.tags)
            {
                tags.Add(item);
            }
            var newProduct = new BsonDocument
            {
                {"pictureId" , product.pictureId}, 
                {"sellerId" , sellerId},
                {"commentId" , commentId},
                {"price" , product.price},
                {"description" , product.description},
                {"title" , product.title},
                { "tags" , tags }
            };
           
            var collection = client.GetCollection<BsonDocument>("products");
            collection.InsertOne(newProduct);

            var productId = Products(null, sellerId, commentId)[0]._id;
            createComment(client, productId, sellerId);

            return true;
        }

        public void createCommentSection(IMongoDatabase client, ObjectId sellerId)
        {

        }
    }
}
