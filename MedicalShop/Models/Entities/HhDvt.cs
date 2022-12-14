using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class HhDvt
    {
        public int Id { get; set; }
        public int? Idhh { get; set; }
        public int? Iddvt { get; set; }
        public int? Slqd { get; set; }
        public bool? Dvqd { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual Dvt IddvtNavigation { get; set; }
        public virtual HangHoa IdhhNavigation { get; set; }
    }
}
