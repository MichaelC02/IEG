using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlackFriday.Controllers
{
    [Route("api/ProductList")]
    public class ProductListController : Controller
    {
        // GET: http://blackfriday-marolt.azurewebsites.net/api/productlist
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Windows Phone", "BlackBerry" };
        }
        
    }
}
