using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogeLocal.Models
{
    interface IProductRepository
    {
        List<Product> GetAll();
    }
}
