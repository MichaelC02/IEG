using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net;
using System.IO;

namespace ProductCatalogeFTP.Controllers
{
    [Route("api/ProductCatalogeFTP")]
    public class ProductCatalogeFTPController : Controller
    {
        // GET api/values
        [HttpGet]
        public List<string> Get()
        {

            WebClient request = new WebClient();
            string url = "ftp://waws-prod-sn1-155.ftp.azurewebsites.windows.net/products.txt";
            request.Credentials = new NetworkCredential("ProductCatalogeFTP\\git4IEG2017", "git4IEG2017");

            
                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                Console.WriteLine(fileString);

            return fileString.Replace("\r","").Split("\n").ToList<string>();


                /*StringBuilder result = new StringBuilder();
                FtpWebRequest requestDir = (FtpWebRequest)WebRequest.Create("ftp://urserverip/");
                requestDir.Method = WebRequestMethods.Ftp.DownloadFile;
                requestDir.Credentials = new NetworkCredential("BlackFriday-Marolt\\git4IEG2017", "git4IEG2017");
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                response.Close();
                return result.ToString().Split('\n');*/

        }
       
    }
}
