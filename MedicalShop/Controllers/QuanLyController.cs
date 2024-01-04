using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace MedicalShop.Controllers
{

  [Authorize(Roles ="NV")]
  public class QuanLyController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();
    public IActionResult TrucQuan()
    {
      var results = context.TonKho
                   .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
                   .Join(context.HangHoa, x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
                   .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh })
                   .Select(g => new TonKhoModel
                   {
                     MaHH = g.Key.MaHh,
                     TenHH = g.Key.TenHh,
                     SL = (double)g.Sum(tk => tk.tk.SoLuong),
                     Gia = (double)g.Sum(x => x.tk.SoLuong * (x.ctn.Price * (1 + x.ctn.Thue / 100)))
                   })
                   .OrderByDescending(r => r.Gia)
                   .Take(5)
                   .ToList();

      
      ViewBag.Results = results.OrderBy(r => r.Gia);     
      return View();
    }
    
    public IActionResult TestTQ()
    {
      var results = context.TonKho
                   .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
                   .Join(context.HangHoa, x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
                   .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh })
                   .Select(g => new TonKhoModel
                   {
                     MaHH = g.Key.MaHh,
                     TenHH = g.Key.TenHh,
                     SL = (double)g.Sum(tk => tk.tk.SoLuong),
                     Gia = (double)g.Sum(x => x.tk.SoLuong * (x.ctn.Price * (1 + x.ctn.Thue / 100)))
                   })
                   .OrderByDescending(r => r.Gia)
                   .Take(5)
                   .ToList();


      ViewBag.Results = results.OrderBy(r => r.Gia);
      return View();
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Test()
    {
      return View();
    }
    
    public IActionResult ConvertTableToData()
    {
      return View();
    }


   // [Route("/QuanLy")]
    public IActionResult QuanLy()
    {   
      return View();
    }

    
    public IActionResult HoSo(int id)
    {
      ViewBag.idUser = id;
      NhanVien nv = context.NhanVien.FirstOrDefault(a => a.Id.Equals(id));
      return View(nv);
    }


  }
}
