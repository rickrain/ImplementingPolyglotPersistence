using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using ProductService.Model;
using ProductService.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace ProductService.Repository.Mongolab
{
    public class ProductRepository : IProductRepository
    {
        private List<PropertyInfo> knownProperties;

        public ProductRepository()
        {
            MongoHelper.RegisterClassMaps();

            // Get a list of properties I know about (from the model).
            knownProperties = typeof(Product).GetProperties().ToList();
        }

        public ProductService.Model.Product GetProduct(string prodId)
        {
            var id = ObjectId.Parse(prodId);
            if (id != null)
                return MongoHelper.ProductsCollection.FindOneByIdAs<Product>(id);
            else
                return null;
        }

        public ProductService.Model.Product GetProductDetails(string prodId)
        {
            var id = ObjectId.Parse(prodId);
            if (id != null)
                return MongoHelper.ProductsCollection.FindOneById(id);
            else
                return null;
        }

        public List<string> GetCategories()
        {
            var categories = MongoHelper.ProductsCollection.Distinct<string>("Categories");
            if (categories != null)
                return categories.ToList();
            else
                return null;
        }

        public ProductService.Model.Review GetReview(string reviewId)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(reviewId, out objectId))
                return MongoHelper.ReviewsCollection.FindOneById(objectId);
            else
                return null;
        }

        public List<ProductService.Model.Review> GetReviews(string prodId, int pageIndex, int pageSize)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(prodId, out objectId))
            {
                var query = Query<Review>.EQ(r => r.ProdId, objectId.ToString());
                return MongoHelper.ReviewsCollection.Find(query).
                    Skip(pageIndex * pageSize).Take(pageSize).ToList<Review>();
            }
            else
                return null;
        }

        public string AddReview(string prodId, ProductService.Model.Review review)
        {
            review.Id = ObjectId.GenerateNewId().ToString();
            review.ProdId = prodId;
            review.ReviewDate = DateTime.UtcNow;
            MongoHelper.ReviewsCollection.Insert(review);
            return review.Id;
        }

        public bool UpdateReview(string reviewId, ProductService.Model.Review review)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(reviewId, out objectId))
            {
                var query = Query<Review>.EQ(r => r.Id, objectId.ToString());
                var updates = Update<Review>.Combine(
                    Update<Review>.Set(r => r.ReviewDate, DateTime.UtcNow),
                    Update<Review>.Set(r => r.Rating, review.Rating),
                    Update<Review>.Set(r => r.Comments, review.Comments));
                var result = MongoHelper.ReviewsCollection.Update(query, updates);
                return result.Ok;
            }
            else
                return false;
        }

        public void DeleteReview(string reviewId)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(reviewId, out objectId))
            {
                var query = Query<Review>.EQ(r => r.Id, objectId.ToString());
                MongoHelper.ReviewsCollection.Remove(query);
            }
        }

        public List<ProductService.Model.Product> Search(string srchProperty, string srchValue, int pageIndex, int pageSize)
        {
            // If this is a property/field I know about, then I can be more 
            // precise about how I construct the query.
            var knownProperty = knownProperties.Find(p => p.Name.ToLower() == srchProperty.ToLower());

            // Set the name of the property to exactly match casing.
            srchProperty = (knownProperty != null) ? knownProperty.Name : srchProperty;

            // Query for searching a property's value.  
            // This is the default query unless otherwise specified.
            IMongoQuery query = Query.Matches(
                srchProperty,
                new BsonRegularExpression(
                    new Regex(srchValue, RegexOptions.IgnoreCase)));

            if (knownProperty != null)
            {
                if (knownProperty.PropertyType == typeof(double))
                {
                    double srchDouble;
                    if (double.TryParse(srchValue, out srchDouble))
                        query = Query.EQ(knownProperty.Name, srchDouble);
                }
                else if (knownProperty.PropertyType == typeof(List<string>))
                {
                    var rx = new Regex(srchValue, RegexOptions.IgnoreCase);
                    query = Query.All(knownProperty.Name, new List<BsonValue>() { BsonValue.Create(rx) });
                }
            }
            else
            {
                // A query to check if the property exists in the collection.
                var qPropExists = Query.Exists(srchProperty);

                // Pull the two query conditions together into one query.
                query = Query.And(qPropExists, query);
            }

            return MongoHelper.ProductsCollection.FindAs<Product>(query).
                Skip(pageIndex * pageSize).Take(pageSize).ToList<Product>();
        }
    }
}
