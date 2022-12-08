using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class ChiTietPhieuNhap
    {
        public int Id { get; set; }
        public int? Idpn { get; set; }
        public int? Idhh { get; set; }
        public int? Quantity { get; set; }
        public string SoLo { get; set; }
        public decimal? Price { get; set; }
        public int? Idbh { get; set; }
        public int? Tgbh { get; set; }
        public DateTime? Nsx { get; set; }
        public DateTime? Hsd { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual Dvbh IdbhNavigation { get; set; }
        public virtual HangHoa IdhhNavigation { get; set; }
        public virtual PhieuNhap IdpnNavigation { get; set; }
    }
}
