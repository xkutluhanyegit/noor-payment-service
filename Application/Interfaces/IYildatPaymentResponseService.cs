using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Utilities.Results.Interfaces;

namespace Application.Interfaces
{
    public interface IYildatPaymentResponseService
    {
        Task<IResult> CreatePaymentResponseAsync(YildatPaymentResponse paymentResponse);
        Task<bool> IsExistsAsync(string hash);

    }
}