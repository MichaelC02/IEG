using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogeLocal.Models
{
    public class ProductRepository : IProductRepository
    {

        public List<Product> GetAll()
        {
            using (var ctx = new ProductDBContext())
            {
                return ctx.products.OrderBy(a => a.Id).ToList();
            }
        }

        
    }
}
