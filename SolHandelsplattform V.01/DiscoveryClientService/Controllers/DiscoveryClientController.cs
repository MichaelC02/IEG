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

        private const string SERVICE_NAME = "DiscoveryClientService";
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Get Consul-Adress from Config-Server
            WebRequest request = WebRequest.Create("http://localhost:52888/api/GetConsulRegisterUrl");
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string line = reader.ReadLine();
            List<string> consulAddress = JsonConvert.DeserializeObject<string[]>(line).ToList();

            List<Uri> serverUrls = new List<Uri>();
            var consulClient = new ConsulClient(c => c.Address = new Uri("http://" + consulAddress.First() + ":" + consulAddress.Last()));
            var services = consulClient.Agent.Services().Result.Response;

            foreach (var service in services)
            {
                var isMyService = service.Value.Service.ToString().Equals("IEGEasyCreditCardService");
                if (isMyService)
                {
                    string uri = "http://" + service.Value.Address.ToString() + ":"
                                 + service.Value.Port.ToString() + "/" + "api/AcceptedCreditCards";
                    var serviceUri = new Uri(uri);

                    return CallCreditCardService(serviceUri);
                }
            }
            return null;
        }


        string GetToken()
        {
            // SecurityTokenService-Adresse sollte hier vom ConfigService kommen
            var request = WebRequest.Create("http://localhost:59528/api/SecurityTokenService/Login");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{ \"username\": \"admin\", \"password\": \"admin\", \"serviceId\": \""+SERVICE_NAME+"\" }";
                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadLine();
        }

         List<string> CallCreditCardService(Uri serviceUri)
        {
            WebRequest request = WebRequest.Create(serviceUri);
            request.ContentType = "application/json";
            request.Method = "POST";

            string token = GetToken();

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{ \"serviceId\": \"" + SERVICE_NAME + "\", \"token\": \"" + token + "\" }";
                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string line = reader.ReadLine();
            return JsonConvert.DeserializeObject<string[]>(line).ToList<string>();
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
