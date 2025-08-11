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
            // var rawData = await (from y in _context.Yildats
            //             join dh in _context.DaireHafta on y.DaireHaftaId equals dh.Id
            //             join yo in _context.YildatOdemes on y.Id equals yo.YildatId into yoGroup
            //             from yo in yoGroup.DefaultIfEmpty()
            //             where y.Tckn == tckn
            //             select new yildatQueryDto
            //             {
            //                 id = y.Id,
            //                 yil = y.Yil!,
            //                 Tckn = y.Tckn!,
            //                 name = dh.Name!,
            //                 blok_no = y.BlokNo!,
            //                 daire_bb_no = y.DaireBbNo!,
            //                 hafta_no = dh.Name!,
            //                 hizmet_turu = yo != null ? yo.HizmetTuru! : null
            //             }).ToListAsync();

            //     var result = rawData.Select(x => new yildatQueryDto
            //     {
            //         id = x.id,
            //         Tckn = x.Tckn,
            //         yil = x.yil,
            //         name = x.name,
            //         blok_no = x.blok_no,
            //         daire_bb_no = x.daire_bb_no,
            //         hafta_no = x.name.Split('-').Last(),
            //         hizmet_turu = x.hizmet_turu,
            //         key = $"{x.Tckn}~{x.id}~{x.hizmet_turu}"
            //     });



            var yildatQRY = from y in _context.Yildats
                join dh in _context.DaireHafta on y.DaireHaftaId equals dh.Id
                where y.Tckn == tckn && y.KalanYildatTutari > 0
                select new yildatQueryDto
                {
                    id = y.Id,
                    odeme_id = 0,
                    Tckn = y.Tckn!,
                    yil = y.Yil!,
                    name = dh.Name, 
                    blok_no = y.BlokNo,
                    daire_bb_no = y.DaireBbNo,
                    hafta_no = dh.Name, 
                    hizmet_turu = "yildat",
                    odenen_hizmet_tutari = y.YildatOdemes.Where(x=>x.HizmetTuru == "yildat").Sum(x=> x.Tutar),
                    kalan_hizmet_tutari = Convert.ToDouble(y.KalanYildatTutari),
                    hizmet_tutari = Convert.ToDouble(y.YildatTutari),
                    key = $"{y.Id}-yildat-{y.Yil}-pos-0"
                    
                };

            var kurulumQry = from y in _context.Yildats
                join dh in _context.DaireHafta on y.DaireHaftaId equals dh.Id
                where y.Tckn == tckn && y.KalanKurulumTutari > 0
                select new yildatQueryDto
                {
                    id = y.Id,
                    odeme_id = 0,
                    Tckn = y.Tckn!,
                    yil = y.Yil!,
                    name = dh.Name, 
                    blok_no = y.BlokNo,
                    daire_bb_no = y.DaireBbNo,
                    hafta_no = dh.Name, 
                    hizmet_turu = "kurulum",
                    odenen_hizmet_tutari = y.YildatOdemes.Where(x=>x.HizmetTuru == "kurulum").Sum(x=> x.Tutar),
                    kalan_hizmet_tutari = Convert.ToDouble(y.KalanKurulumTutari),
                    hizmet_tutari = Convert.ToDouble(y.KurulumBedeli),
                    key = $"{y.Id}-kurulum-{y.Yil}-pos-0"
                };

            var sayacQry = from y in _context.Yildats
                join dh in _context.DaireHafta on y.DaireHaftaId equals dh.Id
                join yo in _context.YildatOdemes on y.Id equals yo.YildatId into yoGroup
                from yo in yoGroup.DefaultIfEmpty()
                where y.Tckn == tckn && yo.HizmetTuru == "sayac" 
                && (yo.TahTutar- yo.Tutar) > 0
                select new  yildatQueryDto
                {
                    id = y.Id,
                    odeme_id = yo.Id,
                    Tckn = y.Tckn,
                    yil = y.Yil,
                    name = dh.Name,
                    blok_no = y.BlokNo,
                    daire_bb_no = y.DaireBbNo,
                    hafta_no = dh.Name,
                    hizmet_turu = "sayac",
                    odenen_hizmet_tutari = Convert.ToDouble(yo.Tutar),
                    kalan_hizmet_tutari = Convert.ToDouble(yo.TahTutar)-(yo.Tutar),
                    //kalan_hizmet_tutari = Convert.ToDouble((y.YildatOdemes.Where(x=>x.HizmetTuru == "sayac").Sum(x=> x.TahTutar))-(y.YildatOdemes.Where(x=>x.HizmetTuru == "sayac").Sum(x=> x.Tutar))),
                    //hizmet_tutari = Convert.ToDouble(y.YildatOdemes.Where(x=>x.HizmetTuru == "sayac").Sum(x=> x.TahTutar)),
                    hizmet_tutari = Convert.ToDouble(yo.TahTutar),
                    key = $"{y.Id}-sayac-{y.Yil}-pos-{yo.Id}"
                };


            // Combine kurulum, yildat, sayac queries
            var kurulumList = kurulumQry.ToList();
            var yildatList = yildatQRY.ToList();
            var sayacList = sayacQry.ToList();
            var combinedList = yildatList.Concat(kurulumList).Concat(sayacList).ToList();

            var result = combinedList.Select(x => new yildatQueryDto
            {
                id = x.id,
                odeme_id = x.odeme_id,
                Tckn = x.Tckn,
                yil = x.yil,
                name = x.name,
                blok_no = x.blok_no,
                daire_bb_no = x.daire_bb_no,
                hafta_no = x.name.Split('-').Last(),
                hizmet_turu = x.hizmet_turu,
                odenen_hizmet_tutari = x.odenen_hizmet_tutari,
                kalan_hizmet_tutari = x.kalan_hizmet_tutari,
                hizmet_tutari = x.hizmet_tutari,
                key = x.key + $"-{Guid.NewGuid()}"
            }).ToList();
            return result;
        }
    }
}