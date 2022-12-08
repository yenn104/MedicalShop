using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class HangHoa
    {
        public HangHoa()
        {
            ChiTietPhieuNhap = new HashSet<ChiTietPhieuNhap>();
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
            HhDvt = new HashSet<HhDvt>();
        }

        public int Id { get; set; }
        public int? Idnhh { get; set; }
        public int? Idhsx { get; set; }
        public int? Idnsx { get; set; }
        public string MaHh { get; set; }
        public string TenHh { get; set; }
        public string BarCode { get; set; }
        public string Image { get; set; }
        public string MoreImages { get; set; }
        public int? Quantity { get; set; }
        public string Detail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual Hsx IdhsxNavigation { get; set; }
        public virtual NhomHangHoa IdnhhNavigation { get; set; }
        public virtual Nsx IdnsxNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual ICollection<HhDvt> HhDvt { get; set; }
    }
}
