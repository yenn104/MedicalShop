using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class KhachHangController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();

    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục khách hàng";
      TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "KH" && x.Active == true).FirstOrDefault().Id;


      //TempData["Menu"] = context.Menu.Where( menu => EF.Functions.Like( menu.TenMenu, "%Nhà cung cấp%") && menu.Active == true).Select(menu => menu.Id);
      // EF.Functions.Like(c.Name, "a%")     menu.TenMenu.Contains("/Nhà cung cấp/")
      return View("TableKH");
    }

    public IActionResult Details(int id)
    {
      ViewData["Title"] = "Chi tiết nhà cung cấp";
      KhachHang kh = context.KhachHang.FirstOrDefault(x => x.Id == id);
      return View(kh);
    }



    //hiển thị view insert
    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm nhà cung cấp";
      return View();
    }

    //thêm nhà cung cấp
    public IActionResult Insert(KhachHang kh)
    {
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      kh.CreatedBy = idUser;
      kh.ModifiedBy = idUser;
      kh.Idcn = idcn;
      kh.CreatedDate = DateTime.Now;
      kh.ModifiedDate = DateTime.Now;
      kh.Active = true;
      context.KhachHang.Add(kh);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }

    //HIển thị view update
    // [Route("/KhachHang/ViewUpdate/{id}")]
    public IActionResult ViewUpdate(int id)

    {
      KhachHang kh = context.KhachHang.Find(id);
      ViewData["Title"] = "Sửa nhà cung cấp";
      return View(kh);
    }

    public IActionResult Update(KhachHang kh)
    {
      KhachHang n = context.KhachHang.Find(kh.Id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      n.ModifiedBy = idUser;
      n.ModifiedDate = DateTime.Now;
      n.MaKh = kh.MaKh;
      n.TenKh = kh.TenKh;
      n.Address = kh.Address;
      n.Phone = kh.Phone;
      n.Mail = kh.Mail;
      n.Note = kh.Note;
      context.KhachHang.Update(n);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    //xóa nhà cung cấp
    //[Route("/KhachHang/Delete/{id}")] huhu 
    public IActionResult Delete(int id)
    {
      KhachHang kh = context.KhachHang.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      kh.Active = false;
      kh.ModifiedBy = idUser;
      kh.ModifiedDate = DateTime.Now;
      context.KhachHang.Update(kh);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    [HttpPost("/loadTableKH")]
    public IActionResult loadTableKH(bool active)
    {
      if (active)
      {
        ViewBag.KH = context.KhachHang.Where(x => x.Active == true).ToList();
      }
      else
      {
        ViewBag.KH = context.KhachHang.ToList();
      }
      return PartialView();
    }

    //[Route("/KhachHang/khoiphuc/{id}")]
    public IActionResult Restore(int id)
    {
      KhachHang kh = context.KhachHang.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      kh.Active = true;
      kh.ModifiedBy = idUser;
      kh.ModifiedDate = DateTime.Now;
      context.KhachHang.Update(kh);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }

    [HttpPost("/restoreKH")]
    public string Restoree(int id)
    {
      KhachHang kh = context.KhachHang.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      kh.Active = true;
      kh.ModifiedBy = idUser;
      kh.ModifiedDate = DateTime.Now;
      context.KhachHang.Update(kh);
      context.SaveChanges();
      return "Khôi phục thành công!";
    }

    [HttpPost("/loadDetailKH")]
    public IActionResult LoadDetailKH(int id)
    {
      ViewData["Title"] = "Chi tiết nhà cung cấp";
      KhachHang kh = context.KhachHang.FirstOrDefault(x => x.Id == id);
      return View(kh);
    }
  }
}
