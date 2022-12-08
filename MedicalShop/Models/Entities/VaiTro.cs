using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class VaiTro
    {
        public VaiTro()
        {
            VaiTroTk = new HashSet<VaiTroTk>();
        }

        public int Id { get; set; }
        public int? Idcn { get; set; }
        public int? Idcnang { get; set; }
        public string MaVt { get; set; }
        public string TenVt { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? Active { get; set; }

        public virtual ChiNhanh IdcnNavigation { get; set; }
        public virtual ChucNang IdcnangNavigation { get; set; }
        public virtual ICollection<VaiTroTk> VaiTroTk { get; set; }
    }
}
