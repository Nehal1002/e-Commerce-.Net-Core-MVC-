using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class ProductModel
    {
        public List<Product> FindAll()
        {
            return new List<Product>
            { new Product
            {
                Id="p01",
                Name="Name 1",
                Photo="thumb1.gif",
                Price=4.5,
               Quantity=2
            },
             new Product
            {
                Id="p02",
                Name="Name 2",
               Photo="thumb2.gif",
                Price=5,
               Quantity=3
            },
              new Product
            {
                Id="p03",
                Name="Name 3",
                Photo="thumb3.gif",
                Price=10,
               Quantity=3
            }
            };
        }
    }
}
