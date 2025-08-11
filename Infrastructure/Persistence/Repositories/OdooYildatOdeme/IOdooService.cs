using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories.OdooYildatOdeme
{
    public interface IOdooService
    {
        Task<string> CreateOdooYildatOdeme(int id, string kurulumTuru, string odemeTipi, decimal tutar);
        Task<string> CreateOdooSayacOdeme(int id,int odeme_id, string kurulumTuru, string odemeTipi, decimal tutar);
    }
}