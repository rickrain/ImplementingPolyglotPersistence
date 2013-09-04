using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Model
{
    public class Review
    {
        public string Id { get; set; }
        public string ProdId { get; set; }
        public string UserId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
