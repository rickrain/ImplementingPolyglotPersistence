using ProductService.Model;
using ProductService.Repository.Interface;
using ProductService.Repository.Memory.PrivateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Memory
{
    public class ProductRepository : IProductRepository
    {
        static int nextReviewId = 200;
        internal List<Product> products = new List<Product>()
        {
            new Book() {
                Id = "101",
                Categories = new List<string>() { "Book", "Database" },
                Name = "NoSQL Distilled",
                Price = 39.99,
                Authors = new List<string>() { "Martin Fowler", "Pramod Sadalage" },
                Pages = 164,
                Isbn = "978-0-321-82662-6"
            },

            new Book() {
                Id = "102",
                Categories = new List<string>() { "Book", "Database" },
                Name = "Seven Databases in Seven Weeks",
                Price = 35.00,
                Authors = new List<string>() { "Eric Redmond", "Jim Wilson" },
                Pages = 328,
                Isbn = "9781934356593"
            },

            new CircularSaw() {
                Id = "103",
                Categories = new List<string>() { "Tools" },
                Name = "Adatum Circular Saw",
                Price = 114.25,
                BatteryCellType = "NiCAD",
                Voltage = "18 Volts",
                Weight = 8.3
            },

            new Shirt() {
                Id = "104",
                Categories = new List<string>() { "Clothing" },
                Name = "Fabrikam Athletic Shirt",
                Price = 19.99,
                Colors = new List<string>() { "Gray", "Blue", "Red", "White" },
                Fabric = "cotton,polyester",
                Sizes = new List<string>() { "Small", "Medium", "Large" },
                SleeveLength = "Short"
            },

            new Television() {
                Id = "105",
                Categories = new List<string>() { "Electronics" },
                Name = "Contoso LCD Flat Screen",
                Price = 499.99,
                Dimensions = "40 inch wide screen",
                ManufDate = DateTime.Parse("03-04-2013"),
                SerialNo = "2398ASD129CONTOS1",
                SmartTVFeatures = false,
                Weight = 32.6
            },

            new CircularSaw() {
                Id = "106",
                Categories = new List<string>() { "Tools" },
                Name = "Contoso Circular Saw",
                Price = 89.99,
                BatteryCellType = "NiCAD",
                Voltage = "12 Volts",
                Weight = 5.1
            },

            new Shirt() {
                Id = "107",
                Categories = new List<string>() { "Clothing" },
                Name = "Adatum Executive Shirt",
                Price = 79.99,
                Colors = new List<string>() { "White" },
                Fabric = "cotton",
                Sizes = new List<string>() { "Small", "Medium", "Large", "X-Large" },
                SleeveLength = "Long"
            },

            new Pants() {
                Id = "108",
                Categories = new List<string>() { "Clothing" },
                Name = "Adatum Executive Slacks",
                Price = 54.99,
                Colors = new List<string>() { "Charcoal, Black, Brown" },
                Fabric = "polyester",
                Lengths = new List<int>() { 30, 32, 33, 34, 35, 36 },
                WaistSizes = new List<int>() { 28, 29, 30, 31, 32, 33, 34, 35, 36 }                
            }
        };

        internal List<Review> reviews = new List<Review>()
        {
            new Review() {
                Id = (++nextReviewId).ToString(),
                ProdId = "101",
                Rating = 5,
                UserId = "user1@hotmail.com",
                ReviewDate = DateTime.Now,
                Comments = "Very good read. Recommend it."
            },

            new Review() {
                Id = (++nextReviewId).ToString(),
                ProdId = "101",
                Rating = 4,
                UserId = "user1@gmail.com",
                ReviewDate = DateTime.Now,
                Comments = "Excellent!"
            },

            new Review() {
                Id = (++nextReviewId).ToString(),
                ProdId = "106",
                Rating = 2,
                UserId = "Anonymous",
                ReviewDate = DateTime.Now,
                Comments = "It's ok.  Doesn't cut hard woods very well."
            },

            new Review() {
                Id = (++nextReviewId).ToString(),
                ProdId = "104",
                Rating = 3,
                UserId = "RunnerUser",
                ReviewDate = DateTime.Now,
                Comments = "Nice fit."
            }
        };

        public Product GetProduct(string prodId)
        {
            var product = products.FirstOrDefault(p => p.Id == prodId);
            if (product != null)
                return new Product()
                {
                    Categories = product.Categories,
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                };
            else
                return null;
        }

        public Product GetProductDetails(string prodId)
        {
            return products.FirstOrDefault(p => p.Id == prodId);
        }

        public List<string> GetCategories()
        {
            var categories = new List<string>();
            foreach (Product p in products)
            {
                categories = categories.Union(p.Categories).ToList();
            }

            categories.Sort();
            return categories;
        }

        public Review GetReview(string reviewId)
        {
            return reviews.FirstOrDefault(r => r.Id == reviewId);
        }

        public List<Review> GetReviews(string prodId, int pageIndex, int pageSize)
        {
            return reviews.FindAll(r => r.ProdId == prodId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public string AddReview(string prodId, Review review)
        {
            review.Id = "";

            // Make sure the product exists
            if (products.Exists(p => p.Id == prodId))
            {
                review.ProdId = prodId;
                review.Id = (++nextReviewId).ToString();
                review.ReviewDate = DateTime.Now;

                reviews.Add(review);
            }
            return review.Id;
        }

        public bool UpdateReview(string reviewId, Review review)
        {
            var reviewIndex = reviews.FindIndex(r => r.Id == reviewId);
            if (reviewIndex >= 0)
            {
                reviews[reviewIndex].Comments = review.Comments;
                reviews[reviewIndex].Rating = review.Rating;
                reviews[reviewIndex].ReviewDate = DateTime.Now;
            }
            return (reviewIndex >= 0);
        }

        public void DeleteReview(string reviewId)
        {
            var reviewIndex = reviews.FindIndex(r => r.Id == reviewId);
            if (reviewIndex >= 0)
                reviews.RemoveAt(reviewIndex);
        }

        public List<Product> Search(string srchProperty, string srchValue, int pageIndex, int pageSize)
        {
            srchValue = srchValue.ToLower();

            List<Product> result = new List<Product>();
            foreach (Product p in products)
            {
                foreach (PropertyInfo pi in p.GetType().GetProperties())
                {
                    if ((string.Compare(srchProperty, pi.Name, true) == 0))
                    {
                        // Get the string representation of the property (or close enough)
                        string propValue = pi.GetValue(p) as string;
                        if (propValue == null)
                        {
                            List<string> listValue = pi.GetValue(p) as List<string>;
                            if (listValue != null)
                                foreach (string s in listValue)
                                    propValue += s;
                        }

                        // See if the searchStr is in the value of the property we're searching on.
                        if (propValue != null)
                        {
                            propValue = propValue.ToLower();
                            if (propValue.Contains(srchValue))
                                result.Add(p);
                        }
                    }
                }
            }
            return result.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
    }
}
