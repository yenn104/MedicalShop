using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
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
    public IActionResult loadTableNHH(bool active)
    {
      if (active)
      {
        ViewBag.NNV = context.NhomNhanVien.Where(x => x.Active == true).ToList();
      }
      else
      {
        ViewBag.NNV = context.NhomNhanVien.ToList();
      }
      return PartialView();
    }


    public IActionResult ViewUpdate(int id)
    {
      ViewData["Title"] = "Sửa đơn vị tính";
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
     // nnv.Idcn = idcn;
      nnv.Active = true;
      nnv.CreatedDate = DateTime.Now;
      nnv.ModifiedDate = DateTime.Now;
      context.NhomNhanVien.Add(nnv);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }


    [HttpPost]
    public IActionResult Update(NhomHangHoa nhh)
    {
      NhomHangHoa nh = context.NhomHangHoa.Find(nhh.Id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nh.ModifiedBy = idUser;
      nh.ModifiedDate = DateTime.Now;
      nh.TenNhh = nhh.TenNhh;
      nh.MaNhh = nhh.MaNhh;

      context.NhomHangHoa.Update(nh);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    public IActionResult Delete(int id)
    {
      NhomHangHoa nhh = context.NhomHangHoa.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nhh.ModifiedBy = idUser;
      nhh.ModifiedDate = DateTime.Now;
      nhh.Active = false;
      context.NhomHangHoa.Update(nhh);
      context.SaveChanges();
      return RedirectToAction("Table");
    }

    public IActionResult Restore(int id)
    {
      NhomHangHoa nhh = context.NhomHangHoa.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nhh.ModifiedBy = idUser;
      nhh.ModifiedDate = DateTime.Now;
      nhh.Active = true;
      context.NhomHangHoa.Update(nhh);
      context.SaveChanges();
      //TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }
  }
}
