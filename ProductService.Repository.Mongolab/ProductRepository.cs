using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using ProductService.Model;
using ProductService.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Repository.Mongolab
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository()
        {
            MongoHelper.RegisterClassMaps();
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
            throw new NotImplementedException();
        }

        public List<ProductService.Model.Review> GetReviews(string prodId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public string AddReview(string prodId, ProductService.Model.Review review)
        {
            throw new NotImplementedException();
        }

        public bool UpdateReview(string reviewId, ProductService.Model.Review review)
        {
            throw new NotImplementedException();
        }

        public void DeleteReview(string reviewId)
        {
            throw new NotImplementedException();
        }

        public List<ProductService.Model.Product> Search(string srchProperty, string srchValue, int pageIndex, int pageSize)
        {
            // Search first based on known Product model properties
            var srchProductResults = srchKnownProductModel(srchProperty, srchValue, pageIndex, pageSize);
            if (srchProductResults != null)
                return srchProductResults;
            else
            {
                // Search other properties in the collection that are not part
                // of the Product model I know about.

                // A query to check if the property exists in the collection.
                var qPropExists = Query.Exists(srchProperty);

                // A query to do a case-insensitive search of the value of the property.
                var qValueMatch = Query.Matches(srchProperty, new BsonRegularExpression(srchValue, "i"));

                // Pull the two query conditions together into one query.
                var query = Query.And(qPropExists, qValueMatch);

                var srchProductDocumentResults = MongoHelper.ProductsCollection.FindAs<Product>(query).
                    Skip(pageIndex * pageSize).Take(pageSize).ToList<Product>();

                return srchProductDocumentResults;
            }
        }

        private List<ProductService.Model.Product> srchKnownProductModel(string srchProperty, string srchValue, int pageIndex, int pageSize)
        {
            IQueryable<Product> productQuery = null;
            switch (srchProperty.ToLower())
            {
                case "name":
                    productQuery = MongoHelper.ProductsCollection.AsQueryable<Product>().
                        Where(p => p.Name.ToLower().Contains(srchValue.ToLower()));
                    break;

                case "categories":
                    // Maybe cache this result for subsequent queries....
                    var categories = GetCategories();

                    // If the category being searched for doesn't even exist then no need to continue.
                    if (!categories.Contains(srchValue, StringComparer.CurrentCultureIgnoreCase))
                        break;
                    else
                    {
                        // Get the exact casing for the category so Mongo can locate it.
                        srchValue = categories.Find(c => c.ToLower() == srchValue.ToLower());
                        productQuery = MongoHelper.ProductsCollection.AsQueryable<Product>().
                            Where(p => p.Categories.Contains(srchValue));
                    }
                    break;

                case "price":
                    double srchPrice;
                    if (double.TryParse(srchValue, out srchPrice))
                        productQuery = MongoHelper.ProductsCollection.AsQueryable<Product>().
                            Where(p => p.Price == srchPrice);
                    break;
            }

            if (productQuery != null)
                return productQuery.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            else
                return null;
        }
    }
}
