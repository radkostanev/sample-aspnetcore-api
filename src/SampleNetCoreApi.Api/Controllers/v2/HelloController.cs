using SampleNetCoreApi.Api.Auth.Contracts;
using SampleNetCoreApi.Api.Helpers;
using SampleNetCoreApi.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SampleNetCoreApi.v2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Hello, world!");
        }
    }
}
