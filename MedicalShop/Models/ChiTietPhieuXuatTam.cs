using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public partial class ChiTietPhieuXuatTam
  {
    public int Id { get; set; }
    public int? Idhh { get; set; }
    public int? IdDvt { get; set; }
    public double? Slg { get; set; }
    public double? DonGia { get; set; }
    public double? ThanhTien { get; set; }
    public double? Cktm { get; set; }
    public double? Thue { get; set; }
    public string SoLo { get; set; }
    public string Nsx { get; set; }
    public string Hsd { get; set; }

  }
}
