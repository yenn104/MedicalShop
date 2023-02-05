using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class PhieuXuatModel
  {
    //public PhieuXuat phieuXuat { get; set; }
    public string NgayHd { get; set; }
    public string SoHd { get; set; }
    public int? KH { get; set; }
    public string Note { get; set; }

    public List<ChiTietPhieuXuatTam> listOfCTPXT { get; set; }
  }
}
