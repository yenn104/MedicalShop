using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class HangHoaController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    

    [Route("/DSHH")]
    public IActionResult DSHH()
    {
      ViewData["Title"] = "Danh sách hàng hóa";
      List<HangHoa> list = context.HangHoa.ToList();
      return View(list);
    }


    public IActionResult Create()
    {
      if (Request.Form.Count > 0)
      {
        int Idcn = int.Parse(Request.Form["Idcn"]);
        int Idhsx = int.Parse(Request.Form["Idhsx"]);
        int Idnhh = int.Parse(Request.Form["Idnhh"]);
        int Idnsx = int.Parse(Request.Form["Idnsx"]);

        HangHoa hh = new HangHoa();
        hh.Idcn = Idcn;
        hh.Idhsx = Idhsx;
        hh.Idnhh = Idnhh;
        hh.Idnsx = Idnsx;
        hh.Image = "ahc";
        hh.Quantity = 0;
        hh.TenHh = "abc";

        context.HangHoa.Add(hh);
        context.SaveChanges();
        return RedirectToAction("Index");
      }
      return View();
    }


    //[Route("/detail")]
    public IActionResult Details(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == id);
      return View(hh);
    }

    //[Route("/detail")]
    public IActionResult Detail(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == id);
      return View(hh);
    }


  }
}
