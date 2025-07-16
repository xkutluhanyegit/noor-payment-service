using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Models.YildatDto;
using Infrastructure.Persistence.Repositories.GenericRepository;

namespace Infrastructure.Persistence.Repositories.YildatRepository
{
    public class YildatRepository : GenericRepository<Yildat>, IYildatRepository
    {
         private readonly Noor17Context _context;
        public YildatRepository(Noor17Context context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<yildatQueryDto>> GetYildatsByTcknAsync(string tckn)
        {
            var rawData = await (from y in _context.Yildats
                        join yo in _context.YildatOdemes on y.Id equals yo.YildatId
                        join dh in _context.DaireHafta on y.DaireHaftaId equals dh.Id
                        where y.Tckn == tckn
                        select new yildatQueryDto
                        {
                            yil = y.Yil!,
                            Tckn = y.Tckn!,
                            name = dh.Name!,
                            blok_no = y.BlokNo!,
                            daire_bb_no = y.DaireBbNo!,
                            hafta_no = dh.Name!,
                            hizmet_turu = yo.HizmetTuru!,
                            odeme_turu = yo.OdemeTuru!,
                            tutar = yo.Tutar,
                            odenen_tutar = Convert.ToDouble(y.OdenenTutar),
                            kalan_kurulum_tutari = Convert.ToDouble(y.KalanKurulumTutari),
                            kalan_yildat_tutari = Convert.ToDouble(y.KalanYildatTutari),
                            yildat_tutari = Convert.ToDouble(y.YildatTutari),
                            kurulum_bedeli = Convert.ToDouble(y.KurulumBedeli)
                        }).ToListAsync();

                var result = rawData.Select(x => new yildatQueryDto
                {
                    id = x.id,
                    Tckn = x.Tckn,
                    yil = x.yil,
                    name = x.name,
                    blok_no = x.blok_no,
                    daire_bb_no = x.daire_bb_no,
                    hafta_no = x.name.Split('-').Last(),
                    hizmet_turu = x.hizmet_turu,
                    odeme_turu = x.odeme_turu,
                    tutar = x.tutar,
                    odenen_tutar = x.odenen_tutar,
                    kalan_kurulum_tutari = x.kalan_kurulum_tutari,
                    kalan_yildat_tutari = x.kalan_yildat_tutari,
                    yildat_tutari = x.yildat_tutari,
                    kurulum_bedeli = x.kurulum_bedeli
                });

            return result;
        }
    }
}