using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLT.GLT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GLT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [Api]
        public LoginResponse Login([FromBody] LoginRequest login)
        {
            var corp = HttpContext.RequestServices.DataService().Corps.GetCorp(login.CorpId);
            if (corp == null)
                throw new ApiException(Status.CorpNotExist);
            //throw new ArgumentNullException();
            return new LoginResponse() { Token = "xxx" };
        }

        [HttpPost]
        [Api]
        public void Logout()
        {
        }
    }

    public class LoginRequest
    {
        public int CorpId;
        public string UserName;
        public string Password;
    }

    public class LoginResponse
    {
        public string Token;
    }
}
