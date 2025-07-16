using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.GenericRepository;
using Infrastructure.Persistence.Models.YildatDto;

namespace Infrastructure.Persistence.Repositories.YildatRepository
{
    public interface IYildatRepository:IGenericRepository<Yildat>
    {
        Task<IEnumerable<yildatQueryDto>> GetYildatsByTcknAsync(string tckn);
    }
}