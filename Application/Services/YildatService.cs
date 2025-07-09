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
    public class YildatService : IYildatService
    {
        private readonly IYildatRepository _yildatRepository;

        public YildatService(IYildatRepository yildatRepository)
        {
            _yildatRepository = yildatRepository;
        }
        public async Task<IDataResult<IEnumerable<Yildat>>> GetAllYildatAsync()
        {
            var result = await _yildatRepository.GetAllAsync();
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<Yildat>>(result);
            }

            return new SuccessDataResult<IEnumerable<Yildat>>(result);
        }

        public async Task<IDataResult<Yildat>> GetUserByTckn(string tckn)
        {
            var result = await _yildatRepository.GetByFilterAsync(p=> p.Tckn == tckn);
            if (result == null)
            {
                return new ErrorDataResult<Yildat>(result);
            }
            return new SuccessDataResult<Yildat>(result);
        }
    }
}