using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Models.Payments
{
    public class PaymentRequest
    {
        public double Amount { get; set; }
        public string key { get; set; }
        
    }
}