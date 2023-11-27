using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class TonKho
    {
        public int Id { get; set; }
        public int? Idctpn { get; set; }
        public int? Idhh { get; set; }
        public double? SoLuong { get; set; }
        public double? GiaNhap { get; set; }
        public double? GiaVon { get; set; }
        public DateTime? NgayNhap { get; set; }
        public int? Idcn { get; set; }

        public virtual ChiTietPhieuNhap IdctpnNavigation { get; set; }
        public virtual HangHoa IdhhNavigation { get; set; }
    }
}
