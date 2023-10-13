using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  [Authorize(Roles = "NV")]
  public class NhomNhanVienController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục nhóm nhân viên";
      TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "NhomNhanVien" && x.Active == true).FirstOrDefault().Id;
      return View("TableNNV");
    }

    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm nhóm nhân viên";
      return View();
    }

    [HttpPost("/loadTableNNV")]
    public IActionResult loadTableNNV(bool active)
    {
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);

      int idvt = int.Parse(User.Claims.ElementAt(3).Value);

      var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;


      ViewBag.NNV = context.NhomNhanVien
        .Where(x => (active == false ? true : x.Active == true) && (type == true ? true : x.Idcn == idcn))
        .OrderBy(x => x.TenNnv)
        .ToList();
      return PartialView();
    }


    public IActionResult ViewUpdate(int id)
    {
      ViewData["Title"] = "Sửa nhóm nhân viên";
      NhomNhanVien nnv = context.NhomNhanVien.Find(id);
      return View(nnv);
    }

    [HttpPost]
    public IActionResult Insert(NhomNhanVien nnv)
    {
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      nnv.CreatedBy = idUser;
      nnv.ModifiedBy = idUser;
      nnv.Idcn = idcn;
      nnv.Active = true;
      nnv.CreatedDate = DateTime.Now;
      nnv.ModifiedDate = DateTime.Now;
      context.NhomNhanVien.Add(nnv);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }


    [HttpPost]
    public IActionResult Update(NhomNhanVien nnv)
    {
      NhomNhanVien nv = context.NhomNhanVien.Find(nnv.Id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nv.ModifiedBy = idUser;
      nv.ModifiedDate = DateTime.Now;
      nv.TenNnv = nnv.TenNnv;
      nv.MaNnv = nnv.MaNnv;

      context.NhomNhanVien.Update(nv);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    public IActionResult Delete(int id)
    {
      NhomNhanVien nnv = context.NhomNhanVien.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nnv.ModifiedBy = idUser;
      nnv.ModifiedDate = DateTime.Now;
      nnv.Active = false;
      context.NhomNhanVien.Update(nnv);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    public IActionResult Restore(int id)
    {
      NhomNhanVien nnv = context.NhomNhanVien.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nnv.ModifiedBy = idUser;
      nnv.ModifiedDate = DateTime.Now;
      nnv.Active = true;
      context.NhomNhanVien.Update(nnv);
      context.SaveChanges();
      //TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }
  }
}
