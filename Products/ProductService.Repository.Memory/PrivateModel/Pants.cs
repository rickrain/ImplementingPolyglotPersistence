using ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Memory.PrivateModel
{
    internal class Pants : Product
    {
        public string Fabric { get; set; }
        public List<string> Colors { get; set; }
        public List<int> Lengths { get; set; }
        public List<int> WaistSizes { get; set; }
    }
}
