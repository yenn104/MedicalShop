using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class NganHangNcc
    {
        public int Id { get; set; }
        public int? Idncc { get; set; }
        public int? Idnh { get; set; }
        public string Stk { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? Active { get; set; }

        public virtual NhaCungCap IdnccNavigation { get; set; }
        public virtual NganHang IdnhNavigation { get; set; }
    }
}
