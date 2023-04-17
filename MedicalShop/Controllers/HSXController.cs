using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class HSXController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục hãng sản xuất";
      TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "HSX" && x.Active == true).FirstOrDefault().Id;
      return View("TableHSX");
    }

    //hiển thị view insert
    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm hãng sản xuất";
      return View();
    }

    public IActionResult Details(int id)
    {
      Hsx hsx = context.Hsx.FirstOrDefault(x => x.Id == id);
      return View(hsx);
    }


    //thêm hãng sản xuất
    public IActionResult Insert(Hsx hsx)
    {
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      hsx.CreatedBy = idUser;
      hsx.ModifiedBy = idUser;
      hsx.Idcn = idcn;
      hsx.CreatedDate = DateTime.Now;
      hsx.ModifiedDate = DateTime.Now;
      hsx.Active = true;
      context.Hsx.Add(hsx);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";

      return RedirectToAction("Table");
    }

    //[Route("/HangSanXuat/Delete/{id}")]
    public IActionResult Delete(int id)

    {
      Hsx hsx = context.Hsx.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      hsx.ModifiedBy = idUser;
      hsx.ModifiedDate = DateTime.Now;
      hsx.Active = false;
      context.Hsx.Update(hsx);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    //[Route("/HangSanXuat/Update/{id}")]
    public IActionResult ViewUpdate(int id)
    {
      Hsx hsx = context.Hsx.Find(id);
      ViewData["Title"] = "Sửa hãng sản xuất";

      return View(hsx);
    }

    [HttpPost]
    public IActionResult Update(Hsx hsx)
    {
      Hsx h = context.Hsx.Find(hsx.Id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      h.MaHsx = hsx.MaHsx;
      h.TenHsx = hsx.TenHsx;
      h.Address = hsx.Address;
      h.Phone = hsx.Phone;
      h.Mail = hsx.Mail;
      h.ModifiedBy = idUser;
      h.ModifiedDate = DateTime.Now;
      context.Hsx.Update(h);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("table");
    }

    [HttpPost("/loadTableHSX")]
    public IActionResult loadTableHSX(bool active)
    {
      if (active)
      {
        ViewBag.HSX = context.Hsx.Where(x => x.Active == true).ToList();
      }
      else
      {
        ViewBag.HSX = context.Hsx.ToList();
      }
      return PartialView();
    }

    //[Route("/HangSanXuat/khoiphuc/{id}")]
    public IActionResult Restore(int id)
    {
      Hsx hsx = context.Hsx.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      hsx.ModifiedBy = idUser;
      hsx.ModifiedDate = DateTime.Now;
      hsx.Active = true;
      context.Hsx.Update(hsx);
      context.SaveChanges();
      //TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }


    [HttpPost("/restoreHSX")]
    public string Restoree(int id)
    {
      Hsx h = context.Hsx.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      h.Active = true;
      h.ModifiedBy = idUser;
      h.ModifiedDate = DateTime.Now;
      context.Hsx.Update(h);
      context.SaveChanges();
      return "Khôi phục thành công!";
    }



    [HttpPost("/loadDetailHSX")]
    public IActionResult LoadDetail(int id)
    {
      ViewData["Title"] = "Chi tiết hãng sản xuất";
      Hsx h = context.Hsx.FirstOrDefault(x => x.Id == id);
      return View(h);
    }


  }
}
