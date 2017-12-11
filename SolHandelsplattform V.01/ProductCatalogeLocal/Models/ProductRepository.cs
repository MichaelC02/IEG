using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogeLocal.Models
{
    public class ProductRepository : IProductRepository
    {
        static ProductRepository()
        {
            using (var ctx = new ProductDBContext())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
                ctx.SaveChanges();
            }
        }

        public List<Product> GetAll()
        {
            using (var ctx = new ProductDBContext())
            {
                return ctx.auftritte.OrderBy(a => a.Id).ToList();
            }
        }

        
    }
}
