using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Utilities.Results.Interfaces;

namespace Application.Interfaces
{
    public interface IYildatService
    {
        Task<IDataResult<IEnumerable<Yildat>>> GetAllYildatAsync();
        Task<IDataResult<Yildat>> GetUserByTckn(string tckn);
    }
}