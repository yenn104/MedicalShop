using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class CachTinhGia
    {
        public int Id { get; set; }
        public bool? Max { get; set; }
        public bool? Medium { get; set; }
        public bool? Min { get; set; }
        public int? Idcn { get; set; }
    }
}
