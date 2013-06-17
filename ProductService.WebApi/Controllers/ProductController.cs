using Newtonsoft.Json;
using ProductService.Model;
using ProductService.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductService.WebApi.Controllers
{
    public class ProductController : ApiController
    {
        // GET api/product/categories
        [HttpGet]
        [ActionName("categories")]
        public HttpResponseMessage GetCategories()
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var categories = repository.GetCategories();

            return Request.CreateResponse<List<string>>(HttpStatusCode.OK, categories);
        }

        // GET api/product/{prodId}
        [HttpGet]
        [ActionName("default")]
        public HttpResponseMessage GetProduct(string prodId)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var product = repository.GetProduct(prodId);

            if (product != null)
                return Request.CreateResponse<Product>(HttpStatusCode.OK, product);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Product Id {0} not found.", prodId));
        }

        // GET api/product/{prodId}/details
        [HttpGet]
        [ActionName("details")]
        public HttpResponseMessage GetProductDetails(string prodId)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();

            var product = repository.GetProductDetails(prodId);

            if (product != null)
                return Request.CreateResponse<Product>(
                    HttpStatusCode.OK, product);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Product Id {0} not found.", prodId));
        }

        // POST api/product/{prodId}/review/
        [HttpPost]
        [ActionName("review")]
        public HttpResponseMessage AddProductReview(string prodId, Review review)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var reviewId = repository.AddReview(prodId, review);

            var responseMsg = Request.CreateResponse(HttpStatusCode.Created);
            // Set location header such that anything passed after the action "review" is discarded.
            var locationUri = Request.RequestUri.ToString();
            var actionIndex = locationUri.IndexOf("review", StringComparison.CurrentCultureIgnoreCase);
            if (actionIndex != -1)
                responseMsg.Headers.Location = new Uri(locationUri.Substring(0, actionIndex) + "review/" + reviewId);
            return responseMsg;
        }

        // GET api/product/{prodId}/review
        [HttpGet]
        [ActionName("review")]
        public HttpResponseMessage GetReviews(string prodId, int pageIndex = 0, int pageSize = 3)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var reviews = repository.GetReviews(prodId, pageIndex, pageSize);
            return Request.CreateResponse<List<Review>>(HttpStatusCode.OK, reviews);
        }

        // GET api/product/{prodId}/review/{reviewId}
        [HttpGet]
        [ActionName("review")]
        public HttpResponseMessage GetReview(string prodId, string reviewId)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();

            var review = repository.GetReview(reviewId);
            if ((review != null) && (review.ProdId == prodId))
                return Request.CreateResponse<Review>(
                    HttpStatusCode.OK, review);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Review Id {0} for Product Id {1} not found.", reviewId, prodId));
        }

        // PUT api/product/{prodId}/review/{reviewId}
        [HttpPut]
        [ActionName("review")]
        public HttpResponseMessage UpdateReview(string prodId, string reviewId, Review review)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var existingReview = repository.GetReview(reviewId);
            if ((existingReview != null) && (existingReview.ProdId == prodId))
                if (repository.UpdateReview(reviewId, review))
                {
                    var responseMsg = Request.CreateResponse(HttpStatusCode.Created);
                    responseMsg.Headers.Location = Request.RequestUri;
                    return responseMsg;
                }
                else
                    return Request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format("Review Id {0} not found.", reviewId));
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Review Id {0} for Product Id {1} not found.", reviewId, prodId));
        }

        // DELETE api/product/{prodId}/review/{reviewId}
        [HttpDelete]
        [ActionName("review")]
        public HttpResponseMessage DeleteReview(string prodId, string reviewId)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var existingReview = repository.GetReview(reviewId);
            if ((existingReview != null) && (existingReview.ProdId == prodId))
            {
                repository.DeleteReview(reviewId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else if (existingReview == null)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Review Id {0} for Product Id {1} not found.", reviewId, prodId));
        }

        // GET api/product/search?property=categories?value=clothing
        [HttpGet]
        [ActionName("search")]
        public HttpResponseMessage Search([FromUri]string property, [FromUri]string value, int pageIndex = 0, int pageSize = 3)
        {
            IProductRepository repository = RepositoryHelper.GetRepository();
            var products = repository.Search(property, value, pageIndex, pageSize);
            return Request.CreateResponse<List<Product>>(HttpStatusCode.OK, products);
        }
    }
}
