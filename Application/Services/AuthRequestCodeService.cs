using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Constant.Messages;
using Application.Dtos.Response.TokenResponse;
using Application.Interfaces;
using Domain.Utilities.Results.Implementations;
using Domain.Utilities.Results.Interfaces;
using Infrastructure.ExternalServices.KizilbukSmsService;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class AuthRequestCodeService : IAuthRequestCodeService
    {
        private readonly IYildatService _yildatService;
        private readonly IVerificationSmsCodeService _verificationSmsCodeService;
        private readonly ITokenService _tokenService;
        private readonly IKizilbukSmsService _kizilbukSmsService;
        private readonly IMemoryCache _memoryCache;

        public AuthRequestCodeService(IYildatService yildatService, IVerificationSmsCodeService verificationSmsCodeService, ITokenService tokenService, IKizilbukSmsService kizilbukSmsService, IMemoryCache memoryCache)
        {
            _yildatService = yildatService;
            _verificationSmsCodeService = verificationSmsCodeService;
            _tokenService = tokenService;
            _kizilbukSmsService = kizilbukSmsService;
            _memoryCache = memoryCache;
        }
        public async Task<IDataResult<TokenResponse>> RequestCodeAsync(string tckn)
        {
            var result = await _yildatService.GetUserByTckn(tckn);
            if (!result.Success)
            {
                return new ErrorDataResult<TokenResponse>(Messages.NotFoundTCKN);
            }

            

            var smsCode = _verificationSmsCodeService.GenerateCode(); // Generate Sms Code
            await _kizilbukSmsService.SendSmsAsync(result.Data.TelNo, smsCode); // Send SMS
            _verificationSmsCodeService.SaveCode(result.Data.TelNo, smsCode); //Memorycache Telno ve Code
            var smsToken = _tokenService.GenerateSmsVerificationToken(tckn); // Generate Token
            var response = new TokenResponse()
            {
                ExpireInMinutes = "5",
                Token = smsToken,
                TokenType = "Bearer",
                Description = "Tel No: " + result.Data.TelNo + " doğrulama kodu gönderildi."
            };
            return new SuccessDataResult<TokenResponse>(response, Messages.SuccessToken);
        }
    }
}