using MedicalShop.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class PhieuNhapModel
  {
    public PhieuNhap phieuNhap { get; set; }
    public List<ChiTietPhieuNhap> chiTietPhieuNhaps { get; set; }
    public List<ChiTietTraNo> chiTietTraNos { get; set; }
  }
}
