using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class DVTController : Controller
  {

    private MedicalShopContext context = new MedicalShopContext();
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục đơn vị tính";
      return View("TableDVT");
    }


    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm đơn vị tính";
      return View();
    }



    [HttpPost("/loadTableDVT")]
    public IActionResult loadTableDVT(bool active)
    {
      if (active)
      {
        ViewBag.DVT = context.Dvt.Where(x => x.Active == true).ToList();
      }
      else
      {
        ViewBag.DVT = context.Dvt.ToList();
      }
      return PartialView();
    }

    //Trả về view sửa đơn vị tính
    //[Route("/DonViTinh/ViewUpdateDVT/{id}")]
    public IActionResult ViewUpdate(int id)
    {
      ViewData["Title"] = "Sửa đơn vị tính";
      Dvt dvt = context.Dvt.Find(id);
      return View(dvt);
    }

    //insert đơn vị tính
    [HttpPost]
    public IActionResult insertDVT(Dvt dvt)
    {
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      int idcn = int.Parse(User.Claims.ElementAt(5).Value);
      dvt.CreatedBy = idUser;
      dvt.ModifiedBy = idUser;
      dvt.Idcn = idcn;
      dvt.Active = true;
      dvt.CreatedDate = DateTime.Now;
      dvt.ModifiedDate = DateTime.Now;
      context.Dvt.Add(dvt);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }

    //update đơn vị tính
    [HttpPost]
    public IActionResult updateDVt(Dvt dvt)
    {
      Dvt dv = context.Dvt.Find(dvt.Id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      dv.ModifiedBy = idUser;
      dv.ModifiedDate = DateTime.Now;
      dv.TenDvt = dvt.TenDvt;
      dv.MaDvt = dvt.MaDvt;

      context.Dvt.Update(dv);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    //Xoá đơn vị tính
    //[Route("/DonViTinh/xoa/{id}")]
    public IActionResult Delete(int id)
    {
      Dvt dvt = context.Dvt.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      dvt.ModifiedBy = idUser;
      dvt.ModifiedDate = DateTime.Now;
      dvt.Active = false;
      context.Dvt.Update(dvt);
      context.SaveChanges();
      return RedirectToAction("Table");
    }
    //[Route("/DonViTinh/khoiphuc/{id}")]
    public IActionResult Restore(int id)
    {
      Dvt dvt = context.Dvt.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      dvt.ModifiedBy = idUser;
      dvt.ModifiedDate = DateTime.Now;
      dvt.Active = true;
      context.Dvt.Update(dvt);
      context.SaveChanges();
      //TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }


  }
}
