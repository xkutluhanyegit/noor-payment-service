using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Utilities.Results.Implementations;
using Domain.Utilities.Results.Interfaces;
using Infrastructure.Persistence.Repositories.YildatRepository;

namespace Application.Services
{
    public class YildatPaymentResponseService : IYildatPaymentResponseService
    {
        private readonly IYildatPaymentResponseRepository _yildatPaymentResponseRepository;

        public YildatPaymentResponseService(IYildatPaymentResponseRepository yildatPaymentResponseRepository)
        {
            _yildatPaymentResponseRepository = yildatPaymentResponseRepository;
        }

        public async Task<IResult> CreatePaymentResponseAsync(YildatPaymentResponse paymentResponse)
        {
            await _yildatPaymentResponseRepository.AddAsync(paymentResponse);
            return new SuccessResult();
        }

        public async Task<bool> IsExistsAsync(string hash)
        {
            var result = await _yildatPaymentResponseRepository.GetByFilterAsync(x=>x.Hash == hash);
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}