using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IHashService
    {
        string CalculatePaymentHash(Dictionary<string, string> parameters);
        bool VerifyPaymentResponse(Dictionary<string, string> responseParameters);
    }
}