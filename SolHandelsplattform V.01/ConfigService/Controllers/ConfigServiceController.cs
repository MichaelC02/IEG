using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ConfigService.Controllers
{
    [Route("api/GetConsulRegisterUrl")]
    public class ConfigServiceController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "127.0.0.1", "8500" };
        }
        
    }
}
