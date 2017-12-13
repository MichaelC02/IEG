using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;

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
            WebRequest request = WebRequest.Create("http://productcataloge-marolt.azurewebsites.net/api/Products/");
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string line = reader.ReadLine();
            return JsonConvert.DeserializeObject<string[]>(line).ToList<string>();
        }
        
    }
}
