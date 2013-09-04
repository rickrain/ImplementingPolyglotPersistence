using ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Memory.PrivateModel
{
    internal class Shirt : Product
    {
        public string Fabric { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> Colors { get; set; }
        public string SleeveLength { get; set; }
    }
}
