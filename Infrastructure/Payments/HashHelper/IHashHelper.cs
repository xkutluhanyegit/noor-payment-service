using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Payments.HashHelper
{
    public interface IHashHelper
    {
        public string CalculateHashV3(Dictionary<string, string> parameters);
        public string EscapeForHash(string value);
        public bool VerifyHash(string hash, Dictionary<string, string> parameters);
    }
}