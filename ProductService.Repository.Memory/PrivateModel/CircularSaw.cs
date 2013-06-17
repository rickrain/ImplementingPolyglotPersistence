using ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Memory.PrivateModel
{
    internal class CircularSaw : Product
    {
        public string Voltage { get; set; }
        public string BatteryCellType { get; set; }
        public double Weight { get; set; }
    }
}
