using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetapp.Data
{
    public class CakeDTO
    {
        public int CakeId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public IFormFile CakeImage { get; set; }
    }
}
