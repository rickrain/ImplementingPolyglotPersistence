using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProductService.Model;
using ProductService.Repository.Mongolab.PrivateModel;
using System.Configuration;

namespace ProductService.Repository.Mongolab
{
    internal static class MongoHelper
    {
        private static MongoClient mongoClient = 
            new MongoClient(ConfigurationManager.AppSettings["mongoConnectionString"]);

        private static MongoDatabase mongoDatabase = 
            mongoClient.GetServer().GetDatabase(ConfigurationManager.AppSettings["mongoDatabase"]);
        
        private static string productsCollectionName = 
            ConfigurationManager.AppSettings["mongoProductsCollection"];

        private static string reviewsCollectionName =
            ConfigurationManager.AppSettings["mongoReviewsCollection"];
        
        public static void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.IdMemberMap.SetRepresentation(BsonType.ObjectId);
            });

            BsonClassMap.RegisterClassMap<ProductDocument>(cm =>
            {
                cm.AutoMap();
                cm.SetExtraElementsMember(cm.GetMemberMap(c => c.ExtraElements));
            });

            BsonClassMap.RegisterClassMap<Review>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap.SetRepresentation(BsonType.ObjectId);
            });
        }

        public static MongoCollection<ProductDocument> ProductsCollection {
            get { return mongoDatabase.GetCollection<ProductDocument>(productsCollectionName); }
        }

        public static MongoCollection<Review> ReviewsCollection {
            get { return mongoDatabase.GetCollection<Review>(reviewsCollectionName); }
        }
    }
}
