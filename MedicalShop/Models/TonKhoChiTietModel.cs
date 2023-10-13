using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class TonKhoChiTietModel
  {
    public int STT { get; set; }
    public DateTime NgayNhap { get; set; }
    public string NCC { get; set; }
    public string MaHH { get; set; }
    public string TenHH { get; set; }
    public string SoLo { get; set; }
    public DateTime HSD { get; set; }
    public double SLNhap { get; set; }
    public double SLXuat { get; set; }
    public double SLTon { get; set; }
    public string DVT { get; set; }
    public double DonGia { get; set; }
    public double ThanhTien { get; set; }



  }
}
