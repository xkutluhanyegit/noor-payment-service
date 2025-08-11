using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.GenericRepository;

namespace Infrastructure.Persistence.Repositories.YildatRepository
{
    public interface IYildatPaymentResponseRepository: IGenericRepository<YildatPaymentResponse>
    {
        
    }
}