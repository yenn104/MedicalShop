using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            ChucNang = new HashSet<ChucNang>();
        }

        public int Id { get; set; }
        public string MaMenu { get; set; }
        public string TenMenu { get; set; }
        public int? Index { get; set; }
        public int? Stt { get; set; }
        public bool? Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<ChucNang> ChucNang { get; set; }
    }
}
