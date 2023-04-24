using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class HHGiaController : Controller
  {
    public IActionResult Table()
    {
      ViewData["Title"] = "Cài đặt giá bán";
      return View("TableHHGia");
    }



    [HttpPost("/addRowVT")]
    public IActionResult addRowGia(int idHH)
    {
      MedicalShopContext context = new MedicalShopContext();
      if (idHH == 0)
      {
        return PartialView();
      }
      else
      {
        ViewBag.HangHoa = context.HangHoa.FirstOrDefault(x => x.Active == true && x.Id == idHH);
        return PartialView();
      }
    }



    [HttpPost("/loadDSNhap")]
    public IActionResult loadDSNhap(int idhh)
    {
      MedicalShopContext context = new MedicalShopContext();
      List<ChiTietPhieuNhap> listctnk = context.ChiTietPhieuNhap.Where(x => x.Idhh == idhh).OrderBy(x => x.CreatedDate).ToList();
      ViewBag.ListCTNK = listctnk;
      ViewBag.IDHH = idhh;
      return PartialView();
    }




  }
}
