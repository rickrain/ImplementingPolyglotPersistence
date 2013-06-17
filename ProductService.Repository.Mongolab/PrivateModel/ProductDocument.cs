using ProductService.Model;
using System.Collections.Generic;

namespace ProductService.Repository.Mongolab.PrivateModel
{
    internal class ProductDocument : Product
    {
        public Dictionary<string, object> ExtraElements { get; set; }
    }
}
