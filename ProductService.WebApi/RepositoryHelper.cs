using ProductService.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductService.WebApi
{
    public static class RepositoryHelper
    {
        //private static ProductService.Repository.Memory.ProductRepository repository = new ProductService.Repository.Memory.ProductRepository();
        private static ProductService.Repository.Mongolab.ProductRepository repository = new ProductService.Repository.Mongolab.ProductRepository();

        public static IProductRepository GetRepository()
        {
            return repository;
        }
    }
}