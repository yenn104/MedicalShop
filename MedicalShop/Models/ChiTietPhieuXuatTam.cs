using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public partial class ChiTietPhieuXuatTam
  {
    public int Id { get; set; }
    public int? Idpx { get; set; }
    public int? Idctpn { get; set; }
    public int? Idhh { get; set; }
    public int? Iddvt { get; set; }
    public int? Quantity { get; set; }
    public double? Price { get; set; }
    public double? Cktm { get; set; }
    public double? Thue { get; set; }

  }
}
