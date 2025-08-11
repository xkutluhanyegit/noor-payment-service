using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Utilities.Results.Interfaces;
using Infrastructure.Persistence.Models.Payments;

namespace Infrastructure.Payments.Interfaces
{
    public interface IPaymentProvider
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        //Task<IDataResult<PaymentRequest>> ProcessPaymentAsync(PaymentRequest request);
    }
}