using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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



    [HttpPost("/addRowGia")]
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
      //List<ChiTietPhieuNhap> listctnk = context.ChiTietPhieuNhap.Where(x => x.Idhh == idhh).OrderBy(x => x.Hsd).ToList();

      List<ChiTietPhieuNhap> listctnk = context.TonKho.Include(x => x.IdctpnNavigation)
        .Where(x => x.IdctpnNavigation.Idhh == idhh).Select(x => x.IdctpnNavigation)
        .OrderBy(x => x.Hsd).ToList();


      ViewBag.ListCTNK = listctnk;
      ViewBag.IDHH = idhh;
      return PartialView();
    }



    [Route("/loadGiaLT")]
    //public IActionResult loadGiaLT(int check)
    //{
    //  MedicalShopContext context = new MedicalShopContext();

    //  var results = context.NhanVien.Where(x => (check == 1 ? x.UserName == null : true)).ToList();

    //  ViewBag.ListNV = results;

    //  return PartialView("loadNVPQ");

    //  //return Ok(results);


    //}
    
    
    [Route("/loadGiaLT")]
    public IActionResult loadGiaLT(int check)
    {
      MedicalShopContext context = new MedicalShopContext();

      var results = context.HhGia.Where(x => (check == 1 ? x.Price == null : true) || (check == 1 ? x.TiLe == null : true)).ToList();


      //var giaLT = context.TonKho
      //          .Include(x => x.IdctpnNavigation)
      //          .Include(x => x.IdctpnNavigation.IdhhNavigation)
      //          .Include(x => x.IdctpnNavigation.IdhhNavigation.HhGia)
      //          .AsEnumerable()
      //          .GroupBy(x => x.IdctpnNavigation.IdhhNavigation.HhGia)
      //          .Where(x => x.Key.)
      //          ;



      //HhGia giaLT = (HhGia)(from tk in context.TonKho
      //join ctn in context.ChiTietPhieuNhap on tk.Idctpn equals ctn.Id
      //join hh in context.HangHoa on ctn.Idhh equals hh.Id
      //join hg in context.HhGia on hh.Id equals hg.Idhh
      //where hg.Price > 0 && hg.Price < (from c in context.ChiTietPhieuNhap
      //                                  where c.Idhh == hh.Id
      //                                  select c.Price).Max() * 1.05
      //select new { TonKho = tk, HH_Gia = hg });


      //ViewBag.GiaLT = giaLT;
      return PartialView("table1");
      //return Ok(results);


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
