using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            KhachHang = new HashSet<KhachHang>();
            NhanVien = new HashSet<NhanVien>();
            VaiTroTk = new HashSet<VaiTroTk>();
        }

        public int Id { get; set; }
        public int? Iduser { get; set; }
        public int? Idvt { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Staff { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<KhachHang> KhachHang { get; set; }
        public virtual ICollection<NhanVien> NhanVien { get; set; }
        public virtual ICollection<VaiTroTk> VaiTroTk { get; set; }
    }
}
