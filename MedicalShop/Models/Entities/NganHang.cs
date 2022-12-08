using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class NganHang
    {
        public NganHang()
        {
            NganHangKh = new HashSet<NganHangKh>();
            NganHangNcc = new HashSet<NganHangNcc>();
        }

        public int Id { get; set; }
        public int? Idhttt { get; set; }
        public string MaNh { get; set; }
        public string TenNh { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual Httt IdhtttNavigation { get; set; }
        public virtual ICollection<NganHangKh> NganHangKh { get; set; }
        public virtual ICollection<NganHangNcc> NganHangNcc { get; set; }
    }
}
