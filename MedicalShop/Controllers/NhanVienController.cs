using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class NhanVienController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }


    private MedicalShopContext context = new MedicalShopContext();

    private readonly IWebHostEnvironment webHostEnvironment;

    public NhanVienController(IWebHostEnvironment hostEnvironment)
    {
      webHostEnvironment = hostEnvironment;
    }

    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục nhân viên";
      TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "NhanVien" && x.Active == true).FirstOrDefault().Id;
      return View("TableNhanVien");
    }


    //[HttpPost]
    //public IActionResult Create()
    //{
    //  if (Request.Form.Count > 0)
    //  {
    //    int Idcn = int.Parse(Request.Form["Idcn"]);
    //    int Idhsx = int.Parse(Request.Form["Idhsx"]);
    //    int Idnhh = int.Parse(Request.Form["Idnhh"]);
    //    int Idnsx = int.Parse(Request.Form["Idnsx"]);

    //    MedicalShopContext context = new MedicalShopContext();
    //    HangHoa hh = new HangHoa();

    //    hh.Idcn = Idcn;
    //    hh.Idhsx = Idhsx;
    //    hh.Idnhh = Idnhh;
    //    hh.Idnsx = Idnsx;
    //    hh.Image = "ahc";
    //    hh.Quantity = 0;
    //    hh.TenHh = "abc";

    //    context.HangHoa.Add(hh);
    //    context.SaveChanges();
    //    return RedirectToAction("Index");
    //  }
    //  return View();
    //}


    //[Route("/detail")]
    public IActionResult Details(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      NhanVien nv = context.NhanVien.FirstOrDefault(x => x.Id == id);
      return View(nv);
    }


    //[Route("/HangHoa/xoa/{id}")]
    public IActionResult Delete(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      NhanVien nv = context.NhanVien.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      nv.Active = false;
      nv.ModifiedBy = idUser;
      nv.ModifiedDate = DateTime.Now;
      context.NhanVien.Update(nv);
      context.SaveChanges();
      TempData["ThongBao"] = "Xoá thành công!";
      return RedirectToAction("Table");
    }


    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm nhân viên";
      return View();
    }


    [HttpPost]
    public IActionResult insertNhanVien(NhanVien nv, IFormFile Avt)
    {
      //MedicalShopContext context = new MedicalShopContext();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      nv.Idcn = idcn;
      nv.Image = UploadedFile(nv, Avt);
      nv.CreatedBy = idUser;
      nv.Active = true;
      nv.CreatedDate = DateTime.Now;
      nv.ModifiedBy = idUser;
      nv.ModifiedDate = DateTime.Now;
      context.NhanVien.Add(nv);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("ViewCreate");
    }


    private string UploadedFile(NhanVien model, IFormFile avt)
    {
      string uniqueFileName = null;

      if (avt != null)
      {
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "lib/imagesNV");
        uniqueFileName = model.MaNv + ".png";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          avt.CopyTo(fileStream);
        }
      }
      return uniqueFileName;
    }

    //Trả về view sửa hàng hoá
    //[Route("/HangHoa/ViewUpdateHangHoa/{id}")]
    public IActionResult ViewUpdate(int id)
    {
      ViewData["Title"] = "Sửa nhân viên";
      NhanVien nv = context.NhanVien.Find(id);
      return View(nv);
    }

    //update hh
    [HttpPost]
    public IActionResult updateNhanVien(NhanVien nv, IFormFile avt)
    {
      NhanVien dv = context.NhanVien.Find(nv.Id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      dv.ModifiedBy = idUser;
      dv.ModifiedDate = DateTime.Now;
      dv.TenNv = nv.TenNv;
      dv.MaNv = nv.MaNv;
      //sua cho nay///////////////////////////////////////////////////
      dv.Address = nv.Address;
      dv.Mail = nv.Mail;
      dv.Phone = nv.Phone;
      dv.HomeTown = nv.HomeTown;
      dv.Sex = nv.Sex;
      dv.DateOfBirth = nv.DateOfBirth;
      if (avt != null)
      {
        dv.Image = UploadedFile(nv, avt);
      }


      context.NhanVien.Update(dv);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }


    [HttpPost("/loadTableNV")]
    public IActionResult loadTable(bool active, int nhomNV)
    {
      if (active)
      {
        if (nhomNV != 0)
        {
          ViewBag.ListNV = context.NhanVien.Where(x => x.Active == active && x.Idnnv == nhomNV).OrderBy(x => x.TenNv).ToList();
        }
        else
        {
          ViewBag.ListNV = context.NhanVien.Where(x => x.Active == active).OrderBy(x => x.TenNv).ToList();
        }
      }
      else
      {
        if (nhomNV != 0)
        {
          ViewBag.ListNV = context.NhanVien.Where(x => x.Idnnv == nhomNV).OrderBy(x => x.TenNv).ToList();
        }
        else
        {
          ViewBag.ListNV = context.NhanVien.OrderBy(x => x.TenNv).ToList();
        }
      }
      return PartialView("LoadTableNV");
    }


    //[HttpPost("/loadMoreTableHH")]
    //public IActionResult loadMoreTableHH(bool active, int nhomHH, int SL)
    //{
    //  if (active)
    //  {
    //    if (nhomHH != 0)
    //    {
    //      ViewBag.ListHH = context.HangHoa.Where(x => x.Active == active && x.Idnhh == nhomHH).Take(SL + 9).ToList();
    //    }
    //    else
    //    {
    //      ViewBag.ListHH = context.HangHoa.Where(x => x.Active == active).Take(SL + 9).ToList();
    //    }
    //  }
    //  else
    //  {
    //    if (nhomHH != 0)
    //    {
    //      ViewBag.ListHH = context.HangHoa.Where(x => x.Idnhh == nhomHH).Take(SL + 9).ToList();
    //    }
    //    else
    //    {
    //      ViewBag.ListHH = context.HangHoa.Take(SL + 9).ToList();
    //    }
    //  }
    //  return PartialView("LoadTableHH");
    //}



    //[Route("/HangHoa/khoiphuc/{id}")]


    public IActionResult Restore(int id)
    {
      HangHoa hh = context.HangHoa.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      hh.ModifiedBy = idUser;
      hh.ModifiedDate = DateTime.Now;
      hh.Active = true;

      context.HangHoa.Update(hh);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }





  }
}
