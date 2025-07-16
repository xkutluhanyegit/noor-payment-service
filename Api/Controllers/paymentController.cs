using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "PaymentOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class paymentController : ControllerBase
    {
        private readonly IYildatService _yildatService;
        public paymentController(IYildatService yildatService)
        {
            _yildatService = yildatService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tckn = User.Claims.FirstOrDefault(p=> p.Type == "tckn").Value;
            
            var result = await _yildatService.GetYildatsByTcknAsync(tckn);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}