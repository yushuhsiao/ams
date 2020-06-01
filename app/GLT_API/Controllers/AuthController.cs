using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GLT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("/api/login")]
        [AllowAnonymous]
        public LoginResponse Login([FromBody] LoginRequest login)
        {
            return new LoginResponse() { Token = "xxx" };
        }

        [HttpPost("/api/logout")]
        public void Logout()
        {
        }
    }

    public class LoginRequest
    {
        public string UserName;
        public string Password;
    }

    public class LoginResponse
    {
        public string Token;
    }
}
