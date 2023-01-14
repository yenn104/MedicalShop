using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class NhaCungCapController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();

    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục nhà cung cấp";
      int abc = context.Menu.FirstOrDefault(menu => EF.Functions.Like(menu.TenMenu, "%Nhà cung cấp%") && menu.Active == true).Id;

      TempData["Menu"] = abc;

      //TempData["Menu"] = context.Menu.Where( menu => EF.Functions.Like( menu.TenMenu, "%Nhà cung cấp%") && menu.Active == true).Select(menu => menu.Id);
      // EF.Functions.Like(c.Name, "a%")     menu.TenMenu.Contains("/Nhà cung cấp/")
      return View("TableNCC");
    }

    public IActionResult Details(int id)
    {
      NhaCungCap ncc = context.NhaCungCap.FirstOrDefault(x => x.Id == id);
      return View(ncc);
    }

    //hiển thị view insert
    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm nhà cung cấp";
      return View();
    }

    //thêm nhà cung cấp
    public IActionResult Insert(NhaCungCap ncc)
    {
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      int idcn = int.Parse(User.Claims.ElementAt(5).Value);
      ncc.CreatedBy = idUser;
      ncc.ModifiedBy = idUser;
      ncc.Idcn = idcn;
      ncc.CreatedDate = DateTime.Now;
      ncc.ModifiedDate = DateTime.Now;
      ncc.Active = true;
      context.NhaCungCap.Add(ncc);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }

    //HIển thị view update
    // [Route("/NhaCungCap/ViewUpdate/{id}")]
    public IActionResult ViewUpdate(int id)

    {
      NhaCungCap ncc = context.NhaCungCap.Find(id);
      ViewData["Title"] = "Sửa nhà cung cấp";
      return View(ncc);
    }

    public IActionResult Update(NhaCungCap ncc)
    {
      NhaCungCap n = context.NhaCungCap.Find(ncc.Id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      n.ModifiedBy = idUser;
      n.ModifiedDate = DateTime.Now;
      n.MaNcc = ncc.MaNcc;
      n.TenNcc = ncc.TenNcc;
      n.Address = ncc.Address;
      n.Phone = ncc.Phone;
      n.Mail = ncc.Mail;
      n.Note = ncc.Note;
      context.NhaCungCap.Update(n);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    //xóa nhà cung cấp
    //[Route("/NhaCungCap/Delete/{id}")]
    public IActionResult Delete(int id)
    {
      NhaCungCap n = context.NhaCungCap.Find(id);
      n.Active = false;

      context.NhaCungCap.Update(n);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    [HttpPost("/loadTableNCC")]
    public IActionResult loadTableNCC(bool active)
    {
      if (active)
      {
        ViewBag.NCC = context.NhaCungCap.Where(x => x.Active == true).ToList();
      }
      else
      {
        ViewBag.NCC = context.NhaCungCap.ToList();
      }

      //TempData["Menu"] = context.Menu.FirstOrDefault(menu => EF.Functions.Like(menu.TenMenu, "%Nhà cung cấp%") && menu.Active == true).Id;
      return PartialView();
    }

    // [Route("/NhaCungCap/khoiphuc/{id}")]
    public IActionResult Restore(int id)
    {
      NhaCungCap ncc = context.NhaCungCap.Find(id);
      ncc.Active = true;

      context.NhaCungCap.Update(ncc);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }
  }
}
