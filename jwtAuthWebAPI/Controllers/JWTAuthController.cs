using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace jwtAuthWebAPI.Controllers
{
    public class JWTAuthController : ApiController
    {
		[Route("getUser")]
		[HttpGet]
        public HttpResponseMessage validateUser(string userName,string userPassword)
		{
            if(userName=="admin" && userPassword == "admin")
			{
                return Request.CreateResponse(HttpStatusCode.OK,TokenManager.GenerateToken(userName));
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.BadGateway, "UserName and Password is invalid");
			}
		}

		[HttpGet]
		[Route("GetEmployee")]
		[CustomAuthFilter]
		public HttpResponseMessage GetUser()
		{
			return Request.CreateResponse(HttpStatusCode.OK,"Successfully Valid");
		}
    }
}
