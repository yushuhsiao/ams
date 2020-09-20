using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Api]
        public void Test([FromServices] IServiceProvider service)
        {
            var c = service.GetDbCache<TestRow>((sender, values) =>
            {
                return new TestRow[] { new TestRow() };
            });
            var v = c.GetValues();
        }

    }

    class TestRow
    {
    }
}