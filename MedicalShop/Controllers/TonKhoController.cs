using AspNetCore.Reporting;
using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MedicalShop.Controllers
{
  public class TonKhoController : Controller
  {
    private readonly ILogger<TonKhoController> _logger;
    private readonly IWebHostEnvironment _webHostEnv;


    public TonKhoController(ILogger<TonKhoController> logger)
    {
      _logger = logger;
    }

    //[Route("/BaoCaoTonKho")]
    [Route("/BaoCaoTonKho")]
    public IActionResult TableTonKho(/*int id*/)
    {
      ViewData["Title"] = "Báo cáo tồn kho";
      MedicalShopContext context = new MedicalShopContext();

      //var phieu = context.PhieuXuat
      //    .Include(x => x.IdkhNavigation)
      //    .Include(x => x.ChiTietPhieuXuat)
      //    .Where(x => x.Id == id).FirstOrDefault();
      //return View(phieu);
      //var results = context.TonKho
      //      .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
      //      .Join(context.HangHoa, x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
      //      .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh })
      //      .Select(g => new
      //      {
      //        MaHH = g.Key.MaHh,
      //        TenHH = g.Key.TenHh,
      //        SL = g.Sum(tk => tk.tk.SoLuong),
      //        Gia = g.Sum(x => x.tk.SoLuong * (x.ctn.Price * (1 + x.ctn.Thue / 100)))
      //      })
      //      .OrderBy(r => r.TenHH)
      //      .ToList();
      //var objectList = results.Select(x => (object)x).ToList();
      //return View(objectList);

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
                    .OrderBy(r => r.TenHH)
                    .ToList();




      ViewBag.getListNHH1 = context.TonKho
        .Include(x=>x.IdctpnNavigation)
        .Include(x=>x.IdctpnNavigation.IdhhNavigation)
        .Include(x=>x.IdctpnNavigation.IdhhNavigation.IdnhhNavigation)
        .AsEnumerable()
        .GroupBy(x=> x.IdctpnNavigation.IdhhNavigation.IdnhhNavigation)
        .Select(x=>x.Key)
        .ToList();


      ViewBag.getListHH = context.TonKho
        .Include(x => x.IdctpnNavigation)
        .Include(x => x.IdctpnNavigation.IdhhNavigation)
        .AsEnumerable()
        .GroupBy(x => x.IdctpnNavigation.IdhhNavigation)
        .Select(x => x.Key)
        .ToList();


      return View(results);

    }

    //ok  return Json(results);
    [Route("/loadTonKhoTH")]
    public IActionResult LoadTonKho(int idnhh, int idhh, string fromDay, string toDay)
    {
      MedicalShopContext context = new MedicalShopContext();
      int index = 0;
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

      //var hangTons = await _dACNPMContext.HangTonKhos
      //              .Include(x => x.IdhhNavigation)
      //              .Include(x => x.IdhhNavigation.IdnhhNavigation)
      //              .Where(x => (idNhh == 0 ? true : x.IdhhNavigation.Idnhh == idNhh)
      //              && (idHh == 0 ? true : x.Idhh == idHh)
      //              && x.NgayNhap.Value.Date >= FromDay
      //              && x.NgayNhap.Value.Date <= ToDay
      //              && x.Idcn == idCn)
      //              .ToListAsync();
      //hh.Idnhh == idnhh


      var results = context.TonKho
          .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
          .Join(context.HangHoa.Where(hh => (idnhh == 0 ? true : hh.Idnhh == idnhh) && (idhh == 0 ? true : hh.Id == idhh)), x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
           .Where(tk => tk.tk.NgayNhap >= FromDay && tk.tk.NgayNhap <=ToDay)
          .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh })
          .Select(g => new TonKhoModel
          {
            STT = index + 1,
            MaHH = g.Key.MaHh,
            TenHH = g.Key.TenHh,
            SL = (double)g.Sum(tk => tk.tk.SoLuong),
            Gia = (double)g.Sum(x => x.tk.SoLuong * (x.ctn.Price * (1 + x.ctn.Thue / 100)))
          })
          .OrderBy(r => r.TenHH)
          .ToList();




      return Ok(results);
 

    }



    [Route("/download/tonkhotonghop/")]
    public IActionResult downloadPhieuXuat(int idnhh, int idhh, string fromDay, string toDay)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var url = Url.Action("viewPDF", "TonKho", new { fromDay = fromDay, toDay = toDay, idnhh = idnhh, idhh = idhh});

      var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + url;

      var pdf = fullView.ConvertUrl(currentUrl);

      var pdfBytes = pdf.Save();
      return File(pdfBytes, "application/pdf", "BCaoTonKho.pdf");
    }





    [Route("/TonKhoPDF/{idnhh:int}/{idhh:int}/{fromDay}/{toDay}")]
    public IActionResult viewPDF(int idnhh, int idhh, string fromDay, string toDay)
    {
      MedicalShopContext context = new MedicalShopContext();
      int index = 0;
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      ViewBag.fromDay = fromDay;
      ViewBag.toDay = toDay;
      ViewBag.nhomhh = idnhh == 0 ? "Tất cả" : context.NhomHangHoa.Find(idnhh).TenNhh;
      ViewBag.hanghoa = idhh == 0 ? "Tất cả" : context.HangHoa.Find(idhh).TenHh;

      

      var results = context.TonKho
          .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
          .Join(context.HangHoa.Where(hh => (idnhh == 0 ? true : hh.Idnhh == idnhh) && (idhh == 0 ? true : hh.Id == idhh)), x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
           .Where(tk => tk.tk.NgayNhap >= FromDay && tk.tk.NgayNhap <= ToDay)
          .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh })
          .Select(g => new TonKhoModel
          {
            STT = index + 1,
            MaHH = g.Key.MaHh,
            TenHH = g.Key.TenHh,
            SL = (double)g.Sum(tk => tk.tk.SoLuong),
            Gia = (double)g.Sum(x => x.tk.SoLuong * (x.ctn.Price * (1 + x.ctn.Thue / 100)))
          })
          .OrderBy(r => r.TenHH)
          .ToList();

      return View("TonKhoTHPDF", results);
    }


  }
}
