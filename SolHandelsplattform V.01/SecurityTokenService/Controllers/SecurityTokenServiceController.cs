using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecurityTokenService.Models;
using Microsoft.AspNetCore.Http;

namespace SecurityTokenService.Controllers
{
    [Route("api/[controller]")]
    public class SecurityTokenServiceController : Controller
    {
        Dictionary<string, string> tokenDictionary = new Dictionary<string, string>();

        // GET api/values/5
        [HttpPost("Login")]
        public string Login([FromBody]UserInfo userInfo)
        {
            // Hardcoded Prüfung zur Veranschaulichung
            if ("admin".Equals(userInfo.username) && "admin".Equals(userInfo.password))
            {
                using (var ctx = new TokenDBContext())
                {
                    Token tkn = new Token();
                    tkn.serviceId = userInfo.serviceId;
                    tkn.token = Guid.NewGuid().ToString();
                    Token current = ctx.Token.Find(userInfo.serviceId);
                    if (current != null)
                    {
                        ctx.Token.Remove(current);
                        ctx.SaveChanges();
                    }
                    ctx.Token.Add(tkn);
                    ctx.SaveChanges();
                    return tkn.token;
                }
            }
            else
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }
        }


        [HttpPost("Check")]
        public void Check([FromBody]Token token)
        {
            using (var ctx = new TokenDBContext())
            {
                Token current = ctx.Token.Find(token.serviceId);

                Response.StatusCode = StatusCodes.Status200OK;

                if (current == null || !current.token.Equals(token.token))
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            }
        }
    }
}
