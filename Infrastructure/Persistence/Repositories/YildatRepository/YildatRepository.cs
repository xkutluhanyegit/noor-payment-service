using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.GenericRepository;

namespace Infrastructure.Persistence.Repositories.YildatRepository
{
    public class YildatRepository : GenericRepository<Yildat>, IYildatRepository
    {
        public YildatRepository(Noor17Context context) : base(context)
        {
        }
    }
}