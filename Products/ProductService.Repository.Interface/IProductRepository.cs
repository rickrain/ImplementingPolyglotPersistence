using ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Interface
{
    public interface IProductRepository
    {
        // Product Operations
        Product GetProduct(string prodId);
        Product GetProductDetails(string prodId);
        List<string> GetCategories();

        // Review Operations
        Review GetReview(string reviewId);
        List<Review> GetReviews(string prodId, int pageIndex, int pageSize);
        string AddReview(string prodId, Review review);
        bool UpdateReview(string reviewId, Review review);
        void DeleteReview(string reviewId);

        // Search Operations
        List<Product> Search(string srchProperty, string srchValue, int pageIndex, int pageSize);
    }
}
