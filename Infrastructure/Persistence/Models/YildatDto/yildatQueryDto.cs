using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Models.YildatDto
{
    public class yildatQueryDto
    {
        public int id { get; set; }
        public int odeme_id { get; set; }
        public string Tckn { get; set; }
        public string yil { get; set; }
        public string name { get; set; }
        public string blok_no { get; set; }
        public string daire_bb_no { get; set; }
        public string hafta_no { get; set; }
        public string hizmet_turu { get; set; }
        public double odenen_hizmet_tutari { get; set; }
        public double kalan_hizmet_tutari { get; set; }
        public double hizmet_tutari { get; set; }
        public string key { get; set; }
    }
}