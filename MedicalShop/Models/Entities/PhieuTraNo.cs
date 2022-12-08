using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class PhieuTraNo
    {
        public PhieuTraNo()
        {
            ChiTietTraNo = new HashSet<ChiTietTraNo>();
        }

        public int Id { get; set; }
        public int? Idhttt { get; set; }
        public string SoPhieu { get; set; }
        public DateTime? DateOfPayment { get; set; }
        public decimal? TotalMoney { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Idcn { get; set; }
        public bool? Active { get; set; }

        public virtual Httt IdhtttNavigation { get; set; }
        public virtual ICollection<ChiTietTraNo> ChiTietTraNo { get; set; }
    }
}
