using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogeLocal.Models;

namespace ProductCatalogeLocal.Controllers
{
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        // GET api/values
        [HttpGet]
        public List<string> Get()
        {
            var repo = new ProductRepository();
            List<Product> all = repo.GetAll();
            List<string> productNames = new List<string>();

            foreach(Product p in all)
            {
                productNames.Add(p.ProductName);
            }

            return productNames;
        }
    }
}
