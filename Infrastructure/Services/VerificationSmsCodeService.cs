using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class VerificationSmsCodeService : IVerificationSmsCodeService
    {
        private readonly IMemoryCache _memoryCache;
        private const int ExpireMinutes = 5;
        public VerificationSmsCodeService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public string GenerateCode()
        {
            return new Random().Next(100000,999999).ToString();
        }

        public void SaveCode(string phoneNumber, string code)
        {
            _memoryCache.Set($"code:{phoneNumber}", code, TimeSpan.FromMinutes(ExpireMinutes));
        }

        public bool ValidateCode(string phoneNumber, string code)
        {
            if (_memoryCache.TryGetValue($"code:{phoneNumber}", out string cachedCode))
            {
                if (cachedCode == code)
                {
                    _memoryCache.Remove($"code:{phoneNumber}");
                    return true;
                }
            }
            return false;
        }
    }
}