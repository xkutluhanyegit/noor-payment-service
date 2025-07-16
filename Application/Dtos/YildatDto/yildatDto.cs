using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.YildatDto
{
    public class yildatDto
    {
        public int id { get; set; }
        public string yil { get; set; }
        public string name { get; set; }
        public string blok_no { get; set; }
        public string daire_bb_no { get; set; }
        public string hizmet_turu { get; set; }
        public string odeme_turu { get; set; }
        public double tutar { get; set; }
        public double odenen_tutar { get; set; }
        public double kalan_kurulum_tutari { get; set; }
        public double kalan_yildat_tutari { get; set; }
        public double yildat_tutari { get; set; }
        public double kurulum_bedeli { get; set; }
    }
}