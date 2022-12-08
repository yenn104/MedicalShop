using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class TrangThai
    {
        public int Id { get; set; }
        public int? Idpx { get; set; }
        public int? Iddvvc { get; set; }
        public int? Nvxk { get; set; }
        public int? Nvtt { get; set; }
        public string MaTt { get; set; }
        public string TenTt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string NameOfShipper { get; set; }
        public string PhoneOfShipper { get; set; }
        public DateTime? DateOfPayment { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? Active { get; set; }

        public virtual Dvvc IddvvcNavigation { get; set; }
        public virtual PhieuXuat IdpxNavigation { get; set; }
    }
}
