using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlackFriday.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;

namespace BlackFriday.Controllers
{
    [Produces("application/json")]
    [Route("api/CashDesk")]
    public class CashDeskController : Controller
    {
        private TelemetryClient telemetry = new TelemetryClient();

        private readonly ILogger<CashDeskController> _logger;
        private static readonly string creditcardServiceBaseAddress= "http://iegeasycreditcardservice-marolt.azurewebsites.net/";

        public CashDeskController(ILogger<CashDeskController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public List<string> Get()
        {

            //return new string[] { "value1", "value2" };

            List<string> result = new List<string>();

            bool success = false;

            WebResponse response = null;
            for (int i=1; i<=3; i++)
            {
                WebRequest request = WebRequest.Create("http://iegeasycreditcardservice-marolt" + i + ".azurewebsites.net/api/acceptedcreditcards/");
                response = ExecRequest(request);
                if (response != null)
                {
                    success = true;
                    break;
                }
            }

            if (!success)
                return null;
            
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string line = reader.ReadLine();
            return JsonConvert.DeserializeObject<string[]>(line).ToList<string>();

         
        }

        private WebResponse ExecRequest(WebRequest req)
        {
            int maxTrys = 5;

            for (int i = 0; i < maxTrys; i++)
            {
                try
                {
                    return req.GetResponse();
                }
                catch(Exception e)
                {
                    //System.Diagnostics.Trace.TraceError("ErrorOccurred: "+e.Message);
                    telemetry.TrackException(e);
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
            }

            return null;
        }

     

        [HttpGet("{id}")]
        public string Get(int id)
        {

            return "value" + id;
        }


        [HttpPost]
        public IActionResult Post([FromBody]Basket basket)
        {
           _logger.LogError("TransactionInfo Creditcard: {0} Product:{1} Amount: {2}", new object[] { basket.CustomerCreditCardnumber, basket.Product, basket.AmountInEuro});

            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                ReceiverName = basket.Vendor
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(creditcardServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response =  client.PostAsJsonAsync(creditcardServiceBaseAddress + "/api/CreditcardTransactions", creditCardTransaction).Result;
            response.EnsureSuccessStatusCode();
           
            
            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() });
        }
    }
}