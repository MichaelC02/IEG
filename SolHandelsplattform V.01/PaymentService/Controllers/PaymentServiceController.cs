using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;

namespace PaymentService.Controllers
{
    [Route("api/PaymentService")]
    public class PaymentServiceController : Controller
    {
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var product = new Product();
            product.Id = 1;
            product.ProductName = "test";
            
            List<Product> products = new List<Product>();
            products.Add(product);
            return products;
        }
    }
}
