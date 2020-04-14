using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GLT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public void Test([FromServices] IServiceProvider service)
        {
            var ss = service.GetService<DbConfigServie>();
            var xx = ss.CoreDB_R();
            var c = service.GetDbCache<TestRow>((sender, values) =>
            {
                return null;
            });
            var v = c.GetValues();
        }

    }

    class TestRow
    {
    }
}