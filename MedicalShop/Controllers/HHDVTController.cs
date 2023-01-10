using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class HHDVTController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục Hàng hoá - Đơn vị tính";
      return View("TableHH_DVT");
    }

    //Trả về view thêm đơn vị tính
    public IActionResult ViewInsertHH_DVT()
    {
      ViewData["Title"] = "Thêm đơn hàng hoá - đơn vị tính";
      return View();
    }
    //Trả về view sửa đơn vị tính
    [Route("/HH_DVT/ViewUpdateHH_DVT/{id}")]
    public IActionResult ViewUpdateHH_DVT(int id)
    {
      ViewData["Title"] = "Sửa đơn hàng hoá - đơn vị tính";
      HhDvt dvt = context.HhDvt.Find(id);
      return View(dvt);
    }

    //insert đơn vị tính
    [HttpPost]
    public IActionResult insertHH_DVT(HhDvt dvt)
    {
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      int idcn = int.Parse(User.Claims.ElementAt(5).Value);
      dvt.CreatedBy = idUser;
      dvt.ModifiedBy = idUser;
      dvt.Idcn = idcn;
      dvt.Active = true;
      dvt.CreatedDate = DateTime.Now;
      dvt.ModifiedDate = DateTime.Now;
      context.HhDvt.Add(dvt);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }

    //update đơn vị tính
    [HttpPost]
    public IActionResult updateHH_DVT(HhDvt dvt)
    {
      HhDvt dv = context.HhDvt.Find(dvt.Id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      dv.CreatedDate = DateTime.Now;
      dv.ModifiedDate = DateTime.Now;
      dv.Slqd = dvt.Slqd;
      dv.Iddvt = dvt.Iddvt;
      dv.Idhh = dvt.Idhh;

      context.HhDvt.Update(dv);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    //Xoá đơn vị tính
    [Route("/HH_DVT/xoa/{id}")]
    public IActionResult deleteHH_DVT(int id)
    {
      HhDvt dvt = context.HhDvt.Find(id);
      dvt.Active = false;

      context.HhDvt.Update(dvt);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    [HttpPost("/loadTableHangHoa")]
    public IActionResult loadTableHangHoa(int nhomHH)
    {
      ViewBag.HangHoas = context.HangHoa.Where(x => x.Idnhh == nhomHH);
      return PartialView();
    }
    [HttpPost("/searchTableHH")]
    public IActionResult searchTableHH(int nhomHH, string key)
    {
      if (nhomHH == 0)
      {
        ViewBag.HangHoas = context.HangHoa.FromSqlRaw("select*from HangHoa where concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%';").ToList();
      }
      else
      {
        ViewBag.HangHoas = context.HangHoa.FromSqlRaw("select*from HangHoa where IdNhh = '" + nhomHH + "' and concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%';").ToList();
      }
      return PartialView("loadTableHangHoa");
    }

    [HttpPost("/loadTableHH_DVT")]
    public IActionResult loadTableHH_DVT(int idHH)
    {
      ViewBag.HH_DVT = context.HhDvt.Where(x => x.Idhh == idHH && x.Active == true);
      HangHoa hh = context.HangHoa.Find(idHH);
      ViewBag.DVC = context.Dvt.FirstOrDefault(x => x.Id == hh.Iddvtc);
      return PartialView();
    }

    [HttpPost("/addHH_DVT")]
    public string addHH_DVT(int idHH, int sl, int idDVT)
    {
      HhDvt h = new HhDvt();
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      int idcn = int.Parse(User.Claims.ElementAt(5).Value);
      h.Idcn = idcn;
      h.CreatedBy = idUser;
      h.ModifiedBy = idUser;
      h.CreatedDate = DateTime.Now;
      h.ModifiedDate = DateTime.Now;
      h.Active = true;
      h.Idhh = idHH;
      h.Slqd = sl;
      h.Iddvt = idDVT;
      context.HhDvt.Add(h);
      context.SaveChanges();
      return "Thêm thành công";
    }

    [HttpPost("/updateHH_DVT")]
    public string updateHH_DVT(int idHH, int sl, int idDVT, int id)
    {
      HhDvt h = context.HhDvt.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      h.Active = true;
      h.ModifiedBy = idUser;
      h.ModifiedDate = DateTime.Now;
      h.Idhh = idHH;
      h.Slqd = sl;
      h.Iddvt = idDVT;
      context.HhDvt.Update(h);
      context.SaveChanges();
      return "Sửa thành công";
    }

    [HttpPost("/addNewRowHH_DVT")]
    public IActionResult addNewRowHH_DVT(int idDVT, int sl, int id)
    {
      if (idDVT == 0)
        ViewBag.idDVT = null;
      else
      {
        ViewBag.idDVT = idDVT;
      }
      ViewBag.ID = id;
      ViewBag.SL = sl;
      return PartialView();
    }
    [HttpPost("/removeHH_DVT")]
    public string removeHH_DVT(int id)
    {
      HhDvt h = context.HhDvt.Find(id);
      h.Active = false;
      context.Update(h);
      context.SaveChanges();
      return "Xoá thành công!";
    }

  }
}
