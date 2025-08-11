using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Payments.Providers.Halkbank;
using Microsoft.Extensions.Options;

namespace Infrastructure.Payments.HashHelper
{
    public class HashHelper : IHashHelper
    {
        private readonly string StoreKey;
        public HashHelper(IOptions<HalkbankPaymentProviderOptions> options)
        {
            StoreKey = options.Value.StoreKey;
        }

        public string CalculateHashV3(Dictionary<string, string> parameters)
        {
            var sortedParams = new SortedDictionary<string, string>(parameters);
            sortedParams.Remove("hash");
            sortedParams.Remove("encoding");

            var hashString = new StringBuilder();
            foreach (var item in sortedParams)
            {
                var escapedValue = EscapeForHash(item.Value ?? string.Empty);
                hashString.Append(escapedValue);
                hashString.Append("|");
            }

            hashString.Append(StoreKey);
            using (var sha512 = SHA512.Create())
            {
                var hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(hashString.ToString()));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public string EscapeForHash(string value)
        {
            return value.Replace("\\", "\\\\").Replace("|", "\\|");
        }

        public bool VerifyHash(string hash, Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}