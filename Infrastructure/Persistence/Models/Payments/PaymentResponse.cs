using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Models.Payments
{
    public class PaymentResponse
    {
        public string postUrl { get; set; }
        public string postData { get; set; }
    }
}