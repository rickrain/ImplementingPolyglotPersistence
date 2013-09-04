using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Model
{
    public class Product
    {
        public string Id { get; set; }
        public List<string> Categories { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
