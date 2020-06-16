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
        /// <summary>
        /// 指定站台為子網站
        /// </summary>
        /// <param name="data"></param>
        [HttpPost, Api("sys.config.global.setDefaultCorp", RootOnly = true)]
        public void SetDefaultCorp([FromBody] SetDefaultIdRequest data)
        {
        }

        [HttpPost, Api("sys.config.global.setvalue", RootOnly = true)]
        public void SetGlobalValue(string key, string value)
        {
        }

        [HttpPost, Api("sys.config.setvalue")]
        public void SetValue(int corpId, string key, string value)
        {
        }
    }

    public class SetDefaultIdRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
