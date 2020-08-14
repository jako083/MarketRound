using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarketRound.Model;

using static MarketRound.Controllers.MainController;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MarkedRound.Model;

namespace MarkedRound.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public List<ProductModel> Products(string _id)
        {
            IMongoQueryable<ProductModel> usageQuery;
            var liste = new List<ProductModel>();
            //Either gets all products or just 1, depending on if _id is defined

            if (_id == null)
            {
                usageQuery = from c in client.GetCollection<ProductModel>("products").AsQueryable()
                             select c;
            }
            else
            {
                usageQuery = from c in client.GetCollection<ProductModel>("products").AsQueryable()
                             where c._id == ObjectId.Parse(_id)
                             select c;
            }
            liste.AddRange(usageQuery);
            return liste;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(Products(null));
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProductPostModel content)
        {
            switch (content.section)
            {
                case "Search":
                    // Example: https://gyazo.com/f6e11a26e75e07d8c32e3607ef6ee63c
                    var products = Products(null);
                    var Matches = products.Where(x => x.title.Contains(content.input.search, StringComparison.CurrentCultureIgnoreCase)
                                         || x.description.Contains(content.input.search, StringComparison.CurrentCultureIgnoreCase)
                                         || x.tags.Contains(content.input.search, StringComparer.OrdinalIgnoreCase)).ToList();
                    return Ok(Matches);
                case "Test":
                    //Add more cases here when given
                    break;
            }
            return Ok();
        }
    }
}
