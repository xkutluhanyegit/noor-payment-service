using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Payments.Providers.Halkbank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class HashService : IHashService
    {
        private readonly string StoreKey;

        public HashService(IOptions<HalkbankPaymentProviderOptions> options)
        {
            StoreKey = options.Value.StoreKey;
        }

        public string CalculatePaymentHash(Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        

        public bool VerifyPaymentResponse(Dictionary<string, string> responseParameters)
        {

            

            var excludedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "HASH", "hash", "encoding", "countdown", "callbackCall"
            };

            if (responseParameters.ContainsKey("amount"))
            {
                responseParameters["amount"] = responseParameters["amount"].Replace(",", ".");
            }

            var sortedParams = responseParameters
                .Where(p => !excludedKeys.Contains(p.Key))
                .OrderBy(p => p.Key, StringComparer.Create(new System.Globalization.CultureInfo("en-US"), false))
                .ToList();

            var hashVal = new StringBuilder();
            foreach (var pair in sortedParams)
            {
                string escapedValue = (pair.Value ?? string.Empty)
                    .Replace("\\", "\\\\")
                    .Replace("|", "\\|");
                    
                hashVal.Append(escapedValue);
                hashVal.Append("|");
            }

            string storeKeyEscaped = StoreKey.Replace("\\", "\\\\").Replace("|", "\\|");
            hashVal.Append(storeKeyEscaped);

           

            string plainTextForHash = hashVal.ToString();
            string calculatedHash;
            using (var sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(plainTextForHash));
                calculatedHash = Convert.ToBase64String(hashBytes);
            }

            string receivedHash = responseParameters
                .FirstOrDefault(p => p.Key.Equals("HASH", StringComparison.OrdinalIgnoreCase))
                .Value;

            bool hashesMatch = CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(calculatedHash),
                Encoding.UTF8.GetBytes(receivedHash)
            );


            return hashesMatch;
        }

        public string EscapeForHash(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("\\", "\\\\")  // \ -> \\
                .Replace("|", "\\|");   // | -> \|
        }



    }
}