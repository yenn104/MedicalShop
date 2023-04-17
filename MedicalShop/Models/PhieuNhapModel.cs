using MedicalShop.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class PhieuNhapModel
  {
    //public PhieuNhap phieuNhap { get; set; }
    public string NgayHd { get; set; }
    public string SoHd { get; set; }
    public int? NCC { get; set; }
    public string Note { get; set; }

    public List<ChiTietPhieuNhapTam> listOfCTPNT { get; set; }

  }
}
