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
  //Đooir lại datetime.now
  [Authorize(Roles = "NV")]

  public class VaiTroController : Controller
  {
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục vai trò";
      return View("TableRoles");
    }

    //Load table chức năng
    [HttpPost("/loadTableCN")]
    public IActionResult loadTableCN(int idvt)
    {
      MedicalShopContext context = new MedicalShopContext();
      List<ChucNang> listcn = context.ChucNang.Where(x => x.Idvt == idvt && x.Active == true).ToList();
      ViewBag.ListCN = listcn;
      ViewBag.IDVT = idvt;
      return PartialView();
    }

    //update list chucnang
    [HttpPost("/updateRoles")]
    public JsonResult updateRoles([FromBody] IEnumerable<FunctionModel> list)
    {
      MedicalShopContext context = new MedicalShopContext();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);

      foreach (var item in list)
      {
        ChucNang cn = context.ChucNang.FirstOrDefault(n => n.Idmenu == item.Idmenu && n.Idvt == item.Idvt && n.Active == true);
        if (cn == null)
        {
          ChucNang cnn = new ChucNang();
          cnn.Idmenu = item.Idmenu;
          cnn.Idvt = item.Idvt;
          cnn.Import = item.Import;
          cnn.Update = item.Update;
          cnn.Delete = item.Delete;
          cnn.Export = item.Export;
          cnn.Print = item.Print;
          cnn.CreatedBy = idUser;
          cnn.CreatedDate = DateTime.Now;
          cnn.ModifiedBy = idUser;
          cnn.ModifiedDate = DateTime.Now;
          cnn.Active = true;
          context.ChucNang.Add(cnn);
        }
        else
        {
          cn.Import = item.Import;
          cn.Update = item.Update;
          cn.Delete = item.Delete;
          cn.Export = item.Export;
          cn.Print = item.Print;
          cn.ModifiedBy = idUser;
          cn.ModifiedDate = DateTime.Now;
          context.ChucNang.Update(cn);
        }
      }
      context.SaveChanges();
      return Json(data: "Cập nhật thành công!");

    }

    //Tạo dòng enable-edit 
    [HttpPost("/addRowVT")]
    public IActionResult addRowVT(int idvt)
    {
      MedicalShopContext context = new MedicalShopContext();
      if (idvt == 0)
      {
        return PartialView();
      }
      else
      {
        ViewBag.VaiTro = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt);
        return PartialView();
      }
    }

    //load table vai trò
    [HttpPost("/loadTableVT")]
    public IActionResult loadTableVT()
    {
      MedicalShopContext context = new MedicalShopContext();
      ViewBag.loadTableVT = context.VaiTro.Where(x => x.Active == true).OrderBy(x => x.TenVt).ToList();
      return PartialView();
    }

    //thêm vai trò
    [HttpPost("/addVT")]
    public string addVT(string tenVT)
    {
      MedicalShopContext context = new MedicalShopContext();
      VaiTro vt = new VaiTro();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      vt.TenVt = tenVT;
      vt.CreatedBy = idUser;
      vt.CreatedDate = DateTime.Now;
      vt.ModifiedBy = idUser;
      vt.ModifiedDate = DateTime.Now;
      vt.Active = true;
      context.VaiTro.Add(vt);
      context.SaveChanges();
      return "Thêm thành công";
    }

    //khôi phục vai trò (chưa dùng)
    [Route("/Roles/khoiphuc/{id}")]
    public IActionResult khoiphucRoles(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      VaiTro vt = context.VaiTro.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      vt.ModifiedBy = idUser;
      vt.ModifiedDate = DateTime.Now;
      vt.Active = true;
      context.VaiTro.Update(vt);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }


    ///search vai trò
    [HttpPost("/searchTableVT")]
    public IActionResult searchTableVT(string key)
    {
      MedicalShopContext context = new MedicalShopContext();
      if (key == "" || key == null)
      {
        ViewBag.loadTableVT = context.VaiTro.Where(x => x.Active == true).ToList();
        return PartialView("LoadTableVT");
      }
      else
      {
        ViewBag.loadTableVT = context.VaiTro.FromSqlRaw("SELECT * FROM VaiTro WHERE TenVT LIKE N'%" + key + "%';").ToList();
        return PartialView("loadTableVT");
      }
    }


    //cập nhật vai trò
    [HttpPost("/updatevaitro")]
    public string updatevaitro(int idvt, string tenvt)
    {
      MedicalShopContext context = new MedicalShopContext();
      VaiTro vt = context.VaiTro.Find(idvt);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      vt.TenVt = tenvt;
      vt.ModifiedBy = idUser;
      vt.ModifiedDate = DateTime.Now;
      vt.Active = true;
      context.VaiTro.Update(vt);
      context.SaveChanges();
      return "Sửa thành công";
    }

    //Xóa (ẩn) vai trò
    [HttpPost("/removeVT")]
    public string removeVT(int idvt)
    {
      MedicalShopContext context = new MedicalShopContext();
      VaiTro vt = context.VaiTro.Find(idvt);
      vt.Active = false;
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      vt.ModifiedBy = idUser;
      vt.ModifiedDate = DateTime.Now;
      context.Update(vt);
      context.SaveChanges();
      return "Xoá thành công!";
    }
  }
}
