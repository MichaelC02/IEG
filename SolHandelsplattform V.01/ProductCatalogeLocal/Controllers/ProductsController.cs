using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogeLocal.Models;

namespace ProductCatalogeLocal.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        // GET api/values
        [HttpGet]
        public List<string> Get()
        {
            var repo = new ProductRepository();
            return repo.GetAll();
        }
    }
}
