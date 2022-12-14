using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            NganHangKh = new HashSet<NganHangKh>();
            PhieuXuat = new HashSet<PhieuXuat>();
        }

        public int Id { get; set; }
        public int? Idnv { get; set; }
        public string MaKh { get; set; }
        public string TenKh { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual TaiKhoan UserNameNavigation { get; set; }
        public virtual ICollection<NganHangKh> NganHangKh { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
    }
}
