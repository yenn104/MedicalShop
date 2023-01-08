using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            ChiTietPhieuNhap = new HashSet<ChiTietPhieuNhap>();
            ChiTietTraNo = new HashSet<ChiTietTraNo>();
        }

        public int Id { get; set; }
        public int? Idncc { get; set; }
        public int? Idnv { get; set; }
        public string SoPn { get; set; }
        public string TenPn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual NhaCungCap IdnccNavigation { get; set; }
        public virtual NhanVien IdnvNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual ICollection<ChiTietTraNo> ChiTietTraNo { get; set; }
    }
}
