namespace Yoneticim
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Daire
    {
        public Daire()
        {
            this.Kalemler = new List<Kalem>();
        }

        [Key]
        public int Id { get; set; }
        public int No { get; set; }
        public Nullable<byte> Kat { get; set; }
        public string SahibiAdi { get; set; }
        public string SahibiSoyadi { get; set; }
        public string SahibiTel { get; set; }
        public string KiraciAdi { get; set; }
        public string KiraciSoyadi { get; set; }
        public string KiraciTel { get; set; }
        public int MulkId { get; set; }
        public string EPosta { get; set; }
        public string Sifre { get; set; }

        public virtual Mulk Mulkler { get; set; }
        public virtual ICollection<Kalem> Kalemler { get; set; }
    }
}