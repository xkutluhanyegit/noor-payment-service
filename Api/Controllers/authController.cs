using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Constant.Messages;
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
        public async Task<IActionResult> request_code([FromBody] RequestCode requestCode)
        {
            var result = await _yildatService.GetUserByTckn(requestCode.tckn);
            if (!result.Success)
            {
                return BadRequest(new ErrorDataResult<Yildat>(Messages.NotFoundTCKN));
            }

            var smsCode = _verificationSmsCodeService.GenerateCode(); // Generate Sms Code
            //SMS Service
            Console.WriteLine("[SMS] ->"+smsCode);

            _verificationSmsCodeService.SaveCode(result.Data.TelNo,smsCode); //Memorycache Telno ve Code 

            var smsToken =  _tokenService.GenerateSmsVerificationToken(requestCode.tckn); // Generate Token

            var response = new TokenResponse(){
                ExpireInMinutes = "5",
                Token = smsToken,
                TokenType = "Bearer",
                Description = "Tel No: "+result.Data.TelNo+" doğrulama kodu gönderildi."
            };
            return Ok(new SuccessDataResult<TokenResponse>(response,Messages.SuccessToken));
        }

        [Authorize(Policy = "SmsVerificationOnly")]
        [HttpPost("verify-code")]
        public async Task<IActionResult> verify_code([FromBody] VerifyCode verifyCode)
        {
            var phoneNumber = User.Claims.FirstOrDefault(p=> p.Type == "phone").Value;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return Unauthorized(new ErrorDataResult<TokenResponse>(Messages.PhoneClaimNotFound));
            }

            if (!_memoryCache.TryGetValue($"code:{phoneNumber}", out string cachedCode))
            {
                return BadRequest(new ErrorDataResult<TokenResponse>(Messages.ExpireSmsCode));
            }

            var isValid = _verificationSmsCodeService.ValidateCode(phoneNumber,verifyCode.smsCode);
            
            //Test Kontroller sonra silinecek
            if (verifyCode.smsCode == "111111")
            {
                isValid = true;
            }
            

            if (!isValid)
            {
                return BadRequest(new ErrorDataResult<TokenResponse>(Messages.InvalidSmsCode));
            }

            var tckn = User.Claims.FirstOrDefault(p=> p.Type == "tckn").Value;

            var accessToken = _tokenService.GenerateAccessToken(tckn,phoneNumber);

            var response = new TokenResponse(){
                ExpireInMinutes = "5",
                Token = accessToken,
                TokenType = "Bearer"
            };
            
            return Ok(new SuccessDataResult<TokenResponse>(response,Messages.SuccessToken));
        }

        
    }
}