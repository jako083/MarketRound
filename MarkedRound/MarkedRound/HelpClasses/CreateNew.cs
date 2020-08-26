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
using static MarketRound.Controllers.MainController;
using static MarkedRound.Controllers.ProductController;
using static MarketRound.HelpClasses.UpdateUser;
using MarkedRound.HelpClasses;
using MarkedRound.Model;

namespace MarketRound.HelpClasses
{
    public class CreateNew
    {
        #region Create New User
        public static string CreateNewSecton(string StrCollection, UserModel user, IMongoDatabase client)
        {
               try
               {
                Encryptor _encrypter = new Encryptor(publicKey);
                var Hashsalt = HashSaltValues(user.password, null);
                user.password = Hashsalt.Pass;
                var EncryptedUser =_encrypter.ObjectToEncryptDecrypt(user, Hashsalt.Salt, "Encrypt");


            var currentTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                var newUser = new BsonDocument
                {
                    {"username", user.username },
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
        #endregion
        #region Create New Offer / Product
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

            client.GetCollection<BsonDocument>("products").InsertOne(newProduct);

            var productId = Products(null, sellerId, commentId)[0]._id;
            client.GetCollection<BsonDocument>("comments").InsertOne(createComment(client, productId, sellerId));
            //todo add encryption if time is avalible
            ChangeUserInput(productId.ToString(), "products", "commentId", null, null, null, null, null, commentId, "ChangeProductCommentId");
            ChangeUserInput(sellerId.ToString(), "users", "ongoingSales", null, null, null, null, null, productId, "AddProductToUser");
            return true;
        }
        #endregion
        #region Create New Comment Section (For new product creation)
        public static BsonDocument createComment(IMongoDatabase client, ObjectId productId, ObjectId sellerId)
        {
            var newCommentsSection = new BsonDocument
            {
                {"productId" , productId},
                {"sellerId" , sellerId},
                {"comments" , new BsonArray()},
            };

            return newCommentsSection; 
        }
        #endregion
    }
}
