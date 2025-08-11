using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOdooAppService
    {
        Task<string> CreateRecordAsync(int id, string kurulumTuru, string odemeTipi, decimal tutar);
        Task<string> CreateRecordSayacAsync(int id, int odeme_id, string kurulumTuru, string odemeTipi, decimal tutar);
    }
}