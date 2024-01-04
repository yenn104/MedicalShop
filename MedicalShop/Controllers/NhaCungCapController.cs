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
  public class NhaCungCapController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();

    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục nhà cung cấp";
      TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "NhaCungCap" && x.Active == true).FirstOrDefault().Id;


      //TempData["Menu"] = context.Menu.Where( menu => EF.Functions.Like( menu.TenMenu, "%Nhà cung cấp%") && menu.Active == true).Select(menu => menu.Id);
      // EF.Functions.Like(c.Name, "a%")     menu.TenMenu.Contains("/Nhà cung cấp/")
      return View("TableNCC");
    }

    public IActionResult Details(int id)
    {
      ViewData["Title"] = "Chi tiết nhà cung cấp";
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
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
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
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
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
    //[Route("/NhaCungCap/Delete/{id}")] huhu 
    public IActionResult Delete(int id)
    {
      NhaCungCap ncc = context.NhaCungCap.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      ncc.Active = false;
      ncc.ModifiedBy = idUser;
      ncc.ModifiedDate = DateTime.Now;
      context.NhaCungCap.Update(ncc);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    [HttpPost("/loadTableNCC")]
    public IActionResult loadTableNCC(bool active)
    {
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);

      int idvt = int.Parse(User.Claims.ElementAt(3).Value);

      var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;


      ViewBag.NCC = context.NhaCungCap
        .Where(x => (active == false ? true : x.Active == true) && (type == 1 ? true : x.Idcn == idcn))
        .OrderBy(x => x.TenNcc)
        .ToList();
      return PartialView();
    }

    //[Route("/NhaCungCap/khoiphuc/{id}")]
    public IActionResult Restore(int id)
    {
      NhaCungCap ncc = context.NhaCungCap.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      ncc.Active = true;
      ncc.ModifiedBy = idUser;
      ncc.ModifiedDate = DateTime.Now;
      context.NhaCungCap.Update(ncc);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }

    [HttpPost("/restore")]
    public string Restoree(int id)
    {
      NhaCungCap ncc = context.NhaCungCap.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      ncc.Active = true;
      ncc.ModifiedBy = idUser;
      ncc.ModifiedDate = DateTime.Now;
      context.NhaCungCap.Update(ncc);
      context.SaveChanges();
      return "Khôi phục thành công!";
    }

    [HttpPost("/loadDetailNCC")]
    public IActionResult LoadDetail(int id)
    {
      ViewData["Title"] = "Chi tiết nhà cung cấp";
      NhaCungCap ncc = context.NhaCungCap.FirstOrDefault(x => x.Id == id);
      return View(ncc);
    }

  }
}
