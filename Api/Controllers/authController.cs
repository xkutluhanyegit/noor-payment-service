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
using Infrastructure.ExternalServices.KizilbukSmsService;
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
        private readonly IVerificationSmsCodeService _verificationSmsCodeService;
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _memoryCache;
        private readonly IAuthRequestCodeService _authRequestCodeService;

        public authController(IYildatService yildatService,IVerificationSmsCodeService verificationSmsCodeService,ITokenService tokenService,IMemoryCache memoryCache, IKizilbukSmsService kizilbukSmsService, IAuthRequestCodeService authRequestCodeService)
        {
            
            _verificationSmsCodeService = verificationSmsCodeService;
            _tokenService = tokenService;
            _memoryCache = memoryCache;
            _authRequestCodeService = authRequestCodeService;
        }

        [HttpPost("request-code")]
        public async Task<IActionResult> request_code([FromBody] RequestCode requestCode)
        {
            var result = await _authRequestCodeService.RequestCodeAsync(requestCode.tckn);
            if (!result.Success)
            {
                return Ok(new ErrorDataResult<TokenResponse>(result.Message));
            }
            return Ok(result);
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
           
            // //Test Kontroller sonra silinecek
            // if (verifyCode.smsCode == "111111")
            // {
            //     isValid = true;
            // }  
                      
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