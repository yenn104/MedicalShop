using MedicalShop.Models;
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



    [HttpPost("/updatePrices")]
    public JsonResult updateRoles([FromBody] IEnumerable<PriceModel> list)
    {
      MedicalShopContext context = new MedicalShopContext();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      foreach (var item in list)
      {
        HhGia gia = context.HhGia.FirstOrDefault(n => n.Idhh == item.Idhh);
        if (gia == null)
        {
          HhGia giaa = new HhGia();
          giaa.Idhh = item.Idhh;
          if (item.TiLe == 0)
          {
            giaa.Price = item.Price;
          }
          if(item.Price == 0)
          {
            giaa.TiLe = item.TiLe;
          }
          giaa.CreatedBy = idUser;
          giaa.CreatedDate = DateTime.Now;
          giaa.ModifiedBy = idUser;
          giaa.ModifiedDate = DateTime.Now;
          giaa.Idcn = idcn;
          giaa.Active = true;
          context.HhGia.Add(giaa);
        }
        else
        {
          if (item.TiLe != 0)
          {
            gia.TiLe = item.TiLe;
            gia.Price = null;
          }
          if (item.Price != 0)
          {
            gia.Price = item.Price;
            gia.TiLe = null;
          }
          gia.ModifiedBy = idUser;
          gia.ModifiedDate = DateTime.Now;
          context.HhGia.Update(gia);
        }
      }
      context.SaveChanges();
      return Json(data: "Cập nhật thành công!");

    }




  }
}
