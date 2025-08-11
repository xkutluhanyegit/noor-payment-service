using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.Payment
{
    public class PaymentRequestDto
    {
        public string key { get; set; }
        public double amount { get; set; }
    }
}