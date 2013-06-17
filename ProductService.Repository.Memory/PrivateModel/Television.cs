using ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repository.Memory.PrivateModel
{
    internal class Television : Product
    {
        public DateTime ManufDate { get; set; }
        public double Weight { get; set; }
        public string Dimensions { get; set; }
        public string SerialNo { get; set; }
        public bool SmartTVFeatures { get; set; }
    }
}
