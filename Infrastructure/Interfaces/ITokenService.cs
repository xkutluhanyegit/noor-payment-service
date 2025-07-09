using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITokenService
    {
        string GenerateSmsVerificationToken(string tckn);
        string GenerateAccessToken(string tckn,string phone);
    }
}