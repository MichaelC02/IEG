using IEGEasyCreditCardService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IEGEasyCreditCardService.Controllers
{
    [Route("api/AcceptedCreditCards")]
    public class AcceptedCreditCardsController : Controller
    {
        /*
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "American", "Diners", "Master", "Visa", "Black Friday" };
        }
        */

        [HttpPost]
        public IEnumerable<string> Get([FromBody]Token token)
        {

            if (checkauth(token))
            {
                return new string[] { "American", "Diners", "Master", "Visa", "Black Friday" };
            }

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return null;
        }



        private Boolean checkauth(Token token)
        {
            var request = HttpWebRequest.Create("http://localhost:59528/api/SecurityTokenService/Check");

            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{ \"serviceId\": \"" + token.serviceId + "\", \"token\": \"" + token.token + "\" }";
                streamWriter.Write(json);
            }

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                httpResponse = (HttpWebResponse)e.Response;
            }

            return HttpStatusCode.OK.Equals(httpResponse.StatusCode);
        }

    }
}
