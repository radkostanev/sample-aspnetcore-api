using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleNetCoreApi.Services.DTOs
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
