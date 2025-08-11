using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Payments.Providers.Halkbank
{
    public class HalkbankPaymentProviderOptions
    {
        public string ClientId { get; set; }
        public string StoreKey { get; set; }
        public string PaymentGatewayUrl { get; set; }
        public string BaseUrl { get; set; }
    }
}