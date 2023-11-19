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
  public class NhomHangHoaController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục nhóm hàng hóa";
      TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "NhomHangHoa" && x.Active == true).FirstOrDefault().Id;
      return View("TableNHH");
    }

    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm nhóm hàng hóa";
      return View();
    }

    [HttpPost("/loadTableNHH")]
    public IActionResult loadTableNHH(bool active)
    {
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      int idvt = int.Parse(User.Claims.ElementAt(3).Value);
      var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;

      ViewBag.NHH = context.NhomHangHoa
        .Where(x => (active == false ? true : x.Active == true) && (type == true ? true : x.Idcn == idcn))
        .OrderBy(x => x.TenNhh)
        .ToList();
      return PartialView();
    }


    public IActionResult ViewUpdate(int id)
    {
      ViewData["Title"] = "Cập nhật nhóm hàng hóa";
      NhomHangHoa nhh = context.NhomHangHoa.Find(id);
      return View(nhh);
    }

    [HttpPost]
    public IActionResult Insert(NhomHangHoa nhh)
    {
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      nhh.CreatedBy = idUser;
      nhh.ModifiedBy = idUser;
      nhh.Idcn = idcn;
      nhh.Active = true;
      nhh.CreatedDate = DateTime.Now;
      nhh.ModifiedDate = DateTime.Now;
      context.NhomHangHoa.Add(nhh);
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
      TempData["ThongBao"] = "Cập nhật thành công!";
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
