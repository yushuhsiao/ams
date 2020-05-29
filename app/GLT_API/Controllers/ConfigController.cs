using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GLT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpPost]
        [Api("sys.config.global.setvalue")]
        public void SetGlobalValue(string key, string value)
        {
        }

        [HttpPost]
        [Api("sys.config.setvalue")]
        public void SetValue(int corpId, string key, string value)
        {
        }
    }
}
