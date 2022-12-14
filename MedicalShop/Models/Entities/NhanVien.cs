using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class NhanVien
    {
        public int Id { get; set; }
        public int? Idnnv { get; set; }
        public string MaNv { get; set; }
        public string TenNv { get; set; }
        public bool? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string HomeTown { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Cccd { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string MoreImages { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual NhomNhanVien IdnnvNavigation { get; set; }
        public virtual TaiKhoan UserNameNavigation { get; set; }
    }
}
