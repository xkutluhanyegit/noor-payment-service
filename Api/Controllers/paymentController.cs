using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "PaymentOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class paymentController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //TODO: Implement Realistic Implementation
            await Task.Yield();
            return Ok();
        }
    }
}