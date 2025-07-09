using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IVerificationSmsCodeService
    {
        string GenerateCode();
        void SaveCode(string phoneNumber,string code);
        bool ValidateCode(string phoneNumber,string code);
    }
}