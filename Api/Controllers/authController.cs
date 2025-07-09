using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Request.RequestCode;
using Application.Dtos.Response.TokenResponse;
using Application.Interfaces;
using Domain.Entities;
using Domain.Utilities.Results.Implementations;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly IYildatService _yildatService;
        private readonly IVerificationSmsCodeService _verificationSmsCodeService;
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _memoryCache;


        public authController(IYildatService yildatService,IVerificationSmsCodeService verificationSmsCodeService,ITokenService tokenService,IMemoryCache memoryCache)
        {
            _yildatService = yildatService;
            _verificationSmsCodeService = verificationSmsCodeService;
            _tokenService = tokenService;
            _memoryCache = memoryCache;        
        }

        [HttpPost("request-code")]
        public async Task<IActionResult> request_code([FromBody] string tckn)
        {
            var result = await _yildatService.GetUserByTckn(tckn);
            if (!result.Success)
            {
                return BadRequest(result.Success);
            }

            var smsCode = _verificationSmsCodeService.GenerateCode(); // Generate Sms Code
            //SMS Service
            Console.WriteLine("[SMS] ->"+smsCode);
            _verificationSmsCodeService.SaveCode(result.Data.TelNo,smsCode); //Memorycache Telno ve Code 

            var smsToken =  _tokenService.GenerateSmsVerificationToken(tckn); // Generate Token

            var response = new TokenResponse(){
                ExpireIn = "5",
                Token = smsToken,
                TokenType = "Bearer"
            };

            return Ok(new SuccessDataResult<TokenResponse>(response,"Success Token"));
        }

        [Authorize(Policy = "SmsVerificationOnly")]
        [HttpPost("verify-code")]
        public async Task<IActionResult> verify_code([FromBody] string smsCode)
        {
            var phoneNumber = User.Claims.FirstOrDefault(p=> p.Type == "phone").Value;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return Unauthorized("Telefon numarası token içinde bulunamadı.");
            }

            if (!_memoryCache.TryGetValue($"code:{phoneNumber}", out string cachedCode))
            {
                return BadRequest("Doğrulama kodu süresi dolmuş.");
            }

            var isValid = _verificationSmsCodeService.ValidateCode(phoneNumber,smsCode);

            if (!isValid)
            {
                return BadRequest("Geçersiz doğrulama kodu.");
            }

            var tckn = User.Claims.FirstOrDefault(p=> p.Type == "tckn").Value;

            var accessToken = _tokenService.GenerateAccessToken(tckn,phoneNumber);

            var response = new TokenResponse(){
                ExpireIn = "5",
                Token = accessToken,
                TokenType = "Bearer"
            };
            
            return Ok(new SuccessDataResult<TokenResponse>(response,"Success Token"));
        }

        
    }
}