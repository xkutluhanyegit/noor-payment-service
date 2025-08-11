using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Persistence.Repositories.OdooYildatOdeme;

namespace Application.Services
{
    public class OdooAppService : IOdooAppService
    {
        private readonly IOdooService _odooService;
        public OdooAppService(IOdooService odooService)
        {
            _odooService = odooService;
        }


        public Task<string> CreateRecordAsync(int id, string kurulumTuru, string odemeTipi, decimal tutar)
        {
            return _odooService.CreateOdooYildatOdeme(id, kurulumTuru, odemeTipi, tutar);
        }

        public Task<string> CreateRecordSayacAsync(int id, int odeme_id, string kurulumTuru, string odemeTipi, decimal tutar)
        {
            return _odooService.CreateOdooSayacOdeme(id, odeme_id, kurulumTuru, odemeTipi, tutar);
        }
    }
}