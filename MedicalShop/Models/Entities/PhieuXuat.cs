using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class PhieuXuat
    {
        public PhieuXuat()
        {
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
            TrangThai = new HashSet<TrangThai>();
        }

        public int Id { get; set; }
        public int? Idkh { get; set; }
        public string SoPx { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual KhachHang IdkhNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual ICollection<TrangThai> TrangThai { get; set; }
    }
}
