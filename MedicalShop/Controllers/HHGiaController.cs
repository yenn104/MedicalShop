using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  [Authorize(Roles = "NV")]

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
    public IActionResult loadGiaLT(int check)
    {
      MedicalShopContext context = new MedicalShopContext();

      var results = context.HhGia.Where(x => (check == 1 ? x.Price == null : true) || (check == 1 ? x.TiLe == null : true)).ToList();

     var hh1 = context.HangHoa
        .Where(hh => (check == 1 ? !hh.HhGia.Any() : true))
        .ToList();


      //ViewBag.Loithoi = context.
      // var results = context.TonKho

      //.Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
      //.Join(context.HangHoa.Include(hh => hh.IddvtcNavigation).Where(hh => (idnhh == 0 ? true : hh.Idnhh == idnhh) && (idhh == 0 ? true : hh.Id == idhh)), x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
      //.Join(context.PhieuNhap, x => x.ctn.Idpn, pn => pn.Id, (x, pn) => new { x.tk, x.ctn, x.hh, pn })
      //.Join(context.NhaCungCap.Where(ncc => (idncc == 0 ? true : ncc.Id == idncc)), x => x.pn.Idncc, ncc => ncc.Id, (x, ncc) => new { x.tk, x.ctn, x.hh, x.pn, ncc })
      //.Where(tk => tk.tk.NgayNhap >= FromDay && tk.tk.NgayNhap <= ToDay && tk.ctn.Hsd <= DateDay)
      //.AsEnumerable()
      //.GroupBy(x => new { x.hh.MaHh, x.hh.TenHh, x.ncc.TenNcc })
      //.Select(g => new TonKhoChiTietModel

      //sai
      //.Where(x => x.IdctpnNavigation.IdhhNavigation.HhGia.Select(x => x.Price) <= (x.IdctpnNavigation.Price * 1.05))

      var hh2 = context.TonKho
                  .Include(x => x.IdctpnNavigation)
                  .Include(x => x.IdctpnNavigation.IdhhNavigation)
                  .Include(x => x.IdctpnNavigation.IdhhNavigation.HhGia)
                  .Where(x => x.IdctpnNavigation.IdhhNavigation.HhGia.Any(hg => hg.Price <= (x.IdctpnNavigation.Price * 1.05)))
                  .Select(x => x.IdctpnNavigation.IdhhNavigation)
                  .Distinct()
                  .ToList();


      ViewBag.Load = hh1.Union(hh2).ToList();


      //HhGia giaLT = (HhGia)(from tk in context.TonKho
      //                      join ctn in context.ChiTietPhieuNhap on tk.Idctpn equals ctn.Id
      //                      join hh in context.HangHoa on ctn.Idhh equals hh.Id
      //                      join hg in context.HhGia on hh.Id equals hg.Idhh
      //                      where hg.Price > 0 && hg.Price < (from c in context.ChiTietPhieuNhap
      //                                                        where c.Idhh == hh.Id
      //                                                        select c.Price).Max() * 1.05
      //                      select new { TonKho = tk, HH_Gia = hg });


      //ViewBag.GiaLT = giaLT;
      return PartialView("LoadGiaLT");
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
