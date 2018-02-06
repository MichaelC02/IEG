using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Consul;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace DiscoveryService.Controllers
{
    [Route("api/DiscoveryClientService")]
    public class DiscoveryClientController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Get Consul-Adress from Config-Server
            WebRequest request = WebRequest.Create("http://localhost:52888/api/GetConsulRegisterUrl");
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string line = reader.ReadLine();
            List<string> consulAdress = JsonConvert.DeserializeObject<string[]>(line).ToList<string>();
            
            
            List<Uri> serverUrls = new List<Uri>();
            var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500"));
            var services = consulClient.Agent.Services().Result.Response;

            foreach (var service in services)
            {
                var isMyService = service.Value.Service.ToString().Equals("IEGEasyCreditCardService");
                if (isMyService)
                {
                    string uri = "http://" + service.Value.Address.ToString() + ":" 
                                 + service.Value.Port.ToString() + "/" + "api/AcceptedCreditCards";
                    var serviceUri = new Uri(uri);

                    request = WebRequest.Create(serviceUri);
                    response = request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                    line = reader.ReadLine();
                    return JsonConvert.DeserializeObject<string[]>(line).ToList<string>();
                }
            }
            return null;
        }








        /*public async Task<string> Do()
        {
            List<Uri> serverUrls = new List<Uri>();
            var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500"));
            var services = consulClient.Agent.Services().Result.Response;

            foreach (var service in services)
            {
                               var isMyService = service.Value.Service.ToString().Equals("IEGEasyCreditCardService");
                if (isMyService)
                {
                    string uri = "http://" + service.Value.Address.ToString() + ":" + service.Value.Port.ToString() + "/" + "api/AcceptedCreditCards";
                    var serviceUri = new Uri(uri);

                    WebRequest request = WebRequest.Create(serviceUri);

                    WebResponse response;
                    response =  request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string line = await reader.ReadLineAsync();

                    return uri;
                }
                
            }
            return null;
           
        }*/
        
    }
}
