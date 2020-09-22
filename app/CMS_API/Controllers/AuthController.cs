using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Api]
        [AllowAnonymous]
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
