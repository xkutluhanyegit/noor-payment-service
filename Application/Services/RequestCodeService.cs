using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Response.TokenResponse;
using Application.Interfaces;
using Domain.Utilities.Results.Interfaces;
using Infrastructure.ExternalServices.KizilbukSmsService;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class RequestCodeService : IRequestCodeService
    {
        private readonly IYildatService _yildatService;
        private readonly IVerificationSmsCodeService _verificationSmsCodeService;
        private readonly ITokenService _tokenService;
        private readonly IKizilbukSmsService _kizilbukSmsService;
        private readonly IMemoryCache _memoryCache;

        public RequestCodeService(IYildatService yildatService, IVerificationSmsCodeService verificationSmsCodeService, ITokenService tokenService, IKizilbukSmsService kizilbukSmsService, IMemoryCache memoryCache)
        {
            _yildatService = yildatService;
            _verificationSmsCodeService = verificationSmsCodeService;
            _tokenService = tokenService;
            _kizilbukSmsService = kizilbukSmsService;
            _memoryCache = memoryCache;
        }

        public Task<IDataResult<TokenResponse>> RequestCodeAsync(string tckn)
        {
            throw new NotImplementedException();
        }
    }
}