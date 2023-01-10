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
  public class HangHoaController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    
    private readonly IWebHostEnvironment webHostEnvironment;


    public HangHoaController(IWebHostEnvironment hostEnvironment)
    {
      webHostEnvironment = hostEnvironment;
    }

    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục hàng hoá";
      return View("TableHangHoa");
    }


    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm hàng hoá";
      return View();
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
      HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == id);
      return View(hh);
    }


    //[Route("/HangHoa/xoa/{id}")]
    public IActionResult Delete(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      HangHoa dvt = context.HangHoa.Find(id);
      dvt.Active = false;

      context.HangHoa.Update(dvt);
      context.SaveChanges();
      TempData["ThongBao"] = "Xoá thành công!";
      return RedirectToAction("Table");
    }




    [HttpPost]
    public IActionResult insertHangHoa(HangHoa hh, IFormFile Avt)
    {
      //MedicalShopContext context = new MedicalShopContext();
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      int idcn = int.Parse(User.Claims.ElementAt(5).Value);
      hh.Idcn = idcn;
      hh.Image = UploadedFile(hh, Avt);
      hh.CreatedBy = idUser;
      hh.Active = true;
      hh.CreatedDate = DateTime.Now;
      hh.ModifiedBy = idUser;
      hh.ModifiedDate = DateTime.Now;
      context.HangHoa.Add(hh);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return View("ViewCreate");
    }


    private string UploadedFile(HangHoa model, IFormFile avt)
    {
      string uniqueFileName = null;

      if (avt != null)
      {
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "lib/imagesHH");
        uniqueFileName = model.MaHh + ".png";
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
      ViewData["Title"] = "Sửa hàng hoá";
      HangHoa dvt = context.HangHoa.Find(id);
      return View(dvt);
    }

    //update hh
    [HttpPost]
    public IActionResult updateHangHoa(HangHoa hh, IFormFile avt)
    {
      HangHoa dv = context.HangHoa.Find(hh.Id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      dv.ModifiedBy = idUser;
      dv.ModifiedDate = DateTime.Now;
      dv.TenHh = hh.TenHh;
      dv.MaHh = hh.MaHh;
      dv.BarCode = hh.BarCode;
      dv.Detail = hh.Detail;
      dv.Idnhh = hh.Idnhh;
      dv.Iddvtc = hh.Iddvtc;
      dv.Idhsx = hh.Idhsx;
      dv.Idnsx = hh.Idnsx;
      if (avt != null)
      {
        dv.Image = UploadedFile(hh, avt);
      }
      

      context.HangHoa.Update(dv);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }


    [HttpPost("/loadTableHH")]
    public IActionResult loadTable(bool active, int nhomHH, int SL)
    {
      if (active)
      {
        if (nhomHH != 0)
        {
          ViewBag.ListHH = context.HangHoa.Where(x => x.Active == active && x.Idnhh == nhomHH).ToList();
        }
        else
        {
          ViewBag.ListHH = context.HangHoa.Where(x => x.Active == active).ToList();
        }
      }
      else
      {
        if (nhomHH != 0)
        {
          ViewBag.ListHH = context.HangHoa.Where(x => x.Idnhh == nhomHH).ToList();
        }
        else
        {
          ViewBag.ListHH = context.HangHoa.ToList();
        }
      }
      return PartialView("LoadTableHH");
    }


    [HttpPost("/loadMoreTableHH")]
    public IActionResult loadMoreTableHH(bool active, int nhomHH, int SL)
    {
      if (active)
      {
        if (nhomHH != 0)
        {
          ViewBag.ListHH = context.HangHoa.Where(x => x.Active == active && x.Idnhh == nhomHH).Take(SL + 9).ToList();
        }
        else
        {
          ViewBag.ListHH = context.HangHoa.Where(x => x.Active == active).Take(SL + 9).ToList();
        }
      }
      else
      {
        if (nhomHH != 0)
        {
          ViewBag.ListHH = context.HangHoa.Where(x => x.Idnhh == nhomHH).Take(SL + 9).ToList();
        }
        else
        {
          ViewBag.ListHH = context.HangHoa.Take(SL + 9).ToList();
        }
      }
      return PartialView("LoadTableHH");
    }



    //[Route("/HangHoa/khoiphuc/{id}")]
    public IActionResult Restore(int id)
    {
      HangHoa hh = context.HangHoa.Find(id);
      hh.Active = true;

      context.HangHoa.Update(hh);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }




  }
}
