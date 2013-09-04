using ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Memory.PrivateModel
{
    internal class Book : Product
    {
        public string Isbn { get; set; }
        public int Pages { get; set; }
        public List<string> Authors { get; set; }
    }
}
