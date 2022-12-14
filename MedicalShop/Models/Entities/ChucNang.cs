using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class ChucNang
    {
        public ChucNang()
        {
            VaiTro = new HashSet<VaiTro>();
        }

        public int Id { get; set; }
        public int? Idmenu { get; set; }
        public string MaCnang { get; set; }
        public string TenCnang { get; set; }
        public bool? Import { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public bool? Print { get; set; }
        public bool? Export { get; set; }
        public bool? Person { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? Active { get; set; }

        public virtual Menu IdmenuNavigation { get; set; }
        public virtual ICollection<VaiTro> VaiTro { get; set; }
    }
}
