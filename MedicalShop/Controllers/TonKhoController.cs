using AspNetCore.Reporting;
using ClosedXML.Excel;
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
using System.IO;
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
        .Include(x => x.IdctpnNavigation)
        .Include(x => x.IdctpnNavigation.IdhhNavigation)
        .Include(x => x.IdctpnNavigation.IdhhNavigation.IdnhhNavigation)
        .AsEnumerable()
        .GroupBy(x => x.IdctpnNavigation.IdhhNavigation.IdnhhNavigation)
        .Select(x => x.Key)
        .ToList();


      ViewBag.getListNCC1 = context.TonKho
        .Include(x => x.IdctpnNavigation)
        .Include(x => x.IdctpnNavigation.IdpnNavigation)
        .Include(x => x.IdctpnNavigation.IdpnNavigation.IdnccNavigation)
        .AsEnumerable()
        .GroupBy(x => x.IdctpnNavigation.IdpnNavigation.IdnccNavigation)
        .Select(x => x.Key)
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
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);

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




      return Ok(results);


    }


    [Route("/loadTonKhoCT")]
    public IActionResult LoadTonKhoCT(int idncc, int idnhh, int idhh, string fromDay, string toDay, string dateDay)
    {
      MedicalShopContext context = new MedicalShopContext();
      int index = 0;
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
      DateTime DateDay = DateTime.ParseExact(dateDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);


      //var results = context.TonKho
      //    .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
      //    .Join(context.HangHoa.Where(hh => (idnhh == 0 ? true : hh.Idnhh == idnhh) && (idhh == 0 ? true : hh.Id == idhh)), x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
      //    .Join(context.PhieuNhap, x => x.ctn.Idpn, pn => pn.Id, (x, pn) => new { x.tk, x.ctn, x.hh, pn })
      //    .Join(context.NhaCungCap.Where(ncc => (idncc == 0 ? true : ncc.Id == idncc)), x => x.pn.Idncc, ncc => ncc.Id, (x, ncc) => new { x.tk, x.ctn, x.hh, x.pn, ncc })
      //    .Where(tk => tk.tk.NgayNhap >= FromDay && tk.tk.NgayNhap <= ToDay)
      //    .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh, x.ncc.TenNcc })
      //    .Select(g => new TonKhoChiTietModel
      //    {
      //      STT = index + 1,
      //      NgayNhap = (DateTime)g.Select(x => x.ctn.CreatedDate).FirstOrDefault(),
      //      MaHH = g.Key.MaHh,
      //      TenHH = g.Key.TenHh,
      //      SLNhap = (double)g.Select(x => x.ctn.Quantity).FirstOrDefault(),

      //      SLTon = (double)g.Select(tk => tk.tk.SoLuong).FirstOrDefault(),
      //      SLXuat = (double)(g.Select(x => x.ctn.Quantity).FirstOrDefault() - g.Select(tk => tk.tk.SoLuong).FirstOrDefault()),
      //      //DVT = g.Select(x => x.ctn.d).FirstOrDefault(),
      //      DonGia = (double)g.Select(x => x.ctn.Price).FirstOrDefault(),
      //    })
      //    .OrderBy(r => r.TenHH)
      //    .ToList();

      var results = context.TonKho
      .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
      .Join(context.HangHoa.Include(hh => hh.IddvtcNavigation).Where(hh => (idnhh == 0 ? true : hh.Idnhh == idnhh) && (idhh == 0 ? true : hh.Id == idhh)), x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
      .Join(context.PhieuNhap, x => x.ctn.Idpn, pn => pn.Id, (x, pn) => new { x.tk, x.ctn, x.hh, pn })
      .Join(context.NhaCungCap.Where(ncc => (idncc == 0 ? true : ncc.Id == idncc)), x => x.pn.Idncc, ncc => ncc.Id, (x, ncc) => new { x.tk, x.ctn, x.hh, x.pn, ncc })
      .Where(tk => tk.tk.NgayNhap >= FromDay && tk.tk.NgayNhap <= ToDay && tk.ctn.Hsd <= DateDay)
      .AsEnumerable()
      .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh, x.ncc.TenNcc })
      .Select(g => new TonKhoChiTietModel
      {
        STT = index + 1,
        NgayNhap = (DateTime)g.Select(x => x.ctn.CreatedDate).FirstOrDefault(),
        NCC = g.Select(x=> x.ncc.TenNcc).FirstOrDefault(),
        MaHH = g.Key.MaHh,
        TenHH = g.Key.TenHh,
        SoLo = g.Select(x => x.ctn.SoLo).FirstOrDefault(),
        HSD = (DateTime)g.Select(x => x.ctn.Hsd).FirstOrDefault(),
        SLNhap = (double)g.Select(x => x.ctn.Quantity).FirstOrDefault(),
        SLXuat = ((double)(g.Select(x => x.ctn.Quantity).FirstOrDefault() - g.Select(tk => tk.tk.SoLuong).FirstOrDefault())),
        SLTon = (double)g.Select(tk => tk.tk.SoLuong).FirstOrDefault(),
        DVT = g.Select(x => x.hh.IddvtcNavigation.TenDvt).FirstOrDefault(),
        DonGia = (double)g.Select(x => x.ctn.Price).FirstOrDefault(),
        ThanhTien = (double)(g.Select(x => x.ctn.Price).FirstOrDefault()*g.Select(tk => tk.tk.SoLuong).FirstOrDefault()),
      })
      .OrderBy(r => r.TenHH)
      .ToList();


      ViewBag.TonKhoCT = results;

      return Ok(results);


    }



    [Route("/download/tonkhotonghop/")]
    public IActionResult downloadPDFTongHop(int idnhh, int idhh, string fromDay, string toDay)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var url = Url.Action("viewPDF", "TonKho", new { fromDay = fromDay, toDay = toDay, idnhh = idnhh, idhh = idhh });

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
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
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


    [Route("/download/tonkhotonghopexcel/")]
    public ActionResult ExportToExcel(int idnhh, int idhh, string fromDay, string toDay)
    {
      MedicalShopContext context = new MedicalShopContext();
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
      int index = 0;
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

      using (var workbook = new XLWorkbook())
      {
        var worksheet = workbook.Worksheets.Add("Báo cáo tồn kho tổng hợp");
        
        var currentRow = 1;
        int stt = 1;
        worksheet.Range(currentRow, 1, currentRow, 5).Merge();

        //worksheet.Cell(currentRow, 1).Value = "Báo cáo tồn kho tổng hợp";
        var titleCell = worksheet.Cell(currentRow, 1);
        titleCell.Value = "BÁO CÁO TỒN KHO TỔNG HỢP";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.FontName = "Times New Roman";
        titleCell.Style.Font.FontSize = 20;
        titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        currentRow +=2;

        worksheet.Range(currentRow, 1, currentRow, 5).Merge();
        var ngay = worksheet.Cell(currentRow, 1);
        ngay.Value = "Từ:" + fromDay + "   Đến: " + toDay;
        ngay.Style.Font.FontSize = 12;
        ngay.Style.Font.FontName = "Times New Roman";
        ngay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        currentRow += 2;

        worksheet.Cell(currentRow, 1).Value = "STT";
        worksheet.Cell(currentRow, 2).Value = "Mã HH";
        worksheet.Cell(currentRow, 3).Value = "Tên HH";
        worksheet.Cell(currentRow, 4).Value = "Tổng tồn";
        worksheet.Cell(currentRow, 5).Value = "Trị giá tồn";
        worksheet.Row(currentRow).Style.Font.Bold = true;
        worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        currentRow ++;
        foreach (var tk in results)
        {
          
          worksheet.Cell(currentRow, 1).Value = stt;
          worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
          worksheet.Cell(currentRow, 2).Value = tk.MaHH;
          worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
          worksheet.Cell(currentRow, 3).Value = tk.TenHH;
          worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
          worksheet.Cell(currentRow, 4).Value = tk.SL;
          worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
          worksheet.Cell(currentRow, 5).Value = tk.Gia;
          worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
          currentRow++;
          stt++;
        }

        worksheet.Cell(currentRow, 4).FormulaA1 = "=SUM(D6:D" + (currentRow - 1) + ")";
        worksheet.Cell(currentRow, 5).FormulaA1 = "=SUM(E6:E" + (currentRow - 1) + ")";
        
        worksheet.Range(worksheet.Cell(6, 4), worksheet.Cell(currentRow, 4)).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(worksheet.Cell(6, 5), worksheet.Cell(currentRow, 5)).Style.NumberFormat.NumberFormatId = 4;


        
        worksheet.Row(currentRow).Style.Font.Bold = true;

        using (var stream = new MemoryStream())
        {
          worksheet.Columns().AdjustToContents();
          workbook.SaveAs(stream);
          var content = stream.ToArray();
          return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeTonKho.xlsx");
        }
      }

    }




    [Route("/download/tonkhochitiet/")]
    public IActionResult downloadPDFChiTiet(int idncc, int idnhh, int idhh, string fromDay, string toDay, string dateDay)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var url = Url.Action("viewCTPDF", "TonKho", new { fromDay = fromDay, toDay = toDay, idnhh = idnhh, idhh = idhh, idncc = idncc, dateDay = dateDay });

      var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + url;

      var pdf = fullView.ConvertUrl(currentUrl);

      var pdfBytes = pdf.Save();
      return File(pdfBytes, "application/pdf", "BCaoTonKho.pdf");
    }





    [Route("/TonKhoCTPDF/{idnhh:int}/{idhh:int}/{fromDay}/{toDay}/{dateDay}/{idncc:int}")]
    public IActionResult viewCTPDF(int idncc, int idnhh, int idhh, string fromDay, string toDay, string dateDay)
    {
      MedicalShopContext context = new MedicalShopContext();
      int index = 0;
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
      ViewBag.fromDay = fromDay;
      ViewBag.toDay = toDay;
      ViewBag.dateDay = dateDay;
      ViewBag.nhomhh = idnhh == 0 ? "Tất cả" : context.NhomHangHoa.Find(idnhh).TenNhh;
      ViewBag.nhacc = idncc == 0 ? "Tất cả" : context.NhaCungCap.Find(idncc).TenNcc;
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


    [Route("/download/tonkhochitietexcel/")]
    public ActionResult ChiTietExportToExcel(int idnhh, int idhh, string fromDay, string toDay)
    {
      MedicalShopContext context = new MedicalShopContext();
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
      int index = 0;
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

      using (var workbook = new XLWorkbook())
      {
        var worksheet = workbook.Worksheets.Add("Báo cáo tồn kho tổng hợp");

        var currentRow = 1;
        int stt = 1;
        worksheet.Range(currentRow, 1, currentRow, 5).Merge();

        //worksheet.Cell(currentRow, 1).Value = "Báo cáo tồn kho tổng hợp";
        var titleCell = worksheet.Cell(currentRow, 1);
        titleCell.Value = "BÁO CÁO TỒN KHO TỔNG HỢP";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.FontName = "Times New Roman";
        titleCell.Style.Font.FontSize = 20;
        titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        currentRow += 2;

        worksheet.Range(currentRow, 1, currentRow, 5).Merge();
        var ngay = worksheet.Cell(currentRow, 1);
        ngay.Value = "Từ:" + fromDay + "   Đến: " + toDay;
        ngay.Style.Font.FontSize = 12;
        ngay.Style.Font.FontName = "Times New Roman";
        ngay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        currentRow += 2;

        worksheet.Cell(currentRow, 1).Value = "STT";
        worksheet.Cell(currentRow, 2).Value = "Mã HH";
        worksheet.Cell(currentRow, 3).Value = "Tên HH";
        worksheet.Cell(currentRow, 4).Value = "Tổng tồn";
        worksheet.Cell(currentRow, 5).Value = "Trị giá tồn";
        worksheet.Row(currentRow).Style.Font.Bold = true;
        worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        currentRow++;
        foreach (var tk in results)
        {

          worksheet.Cell(currentRow, 1).Value = stt;
          worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
          worksheet.Cell(currentRow, 2).Value = tk.MaHH;
          worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
          worksheet.Cell(currentRow, 3).Value = tk.TenHH;
          worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
          worksheet.Cell(currentRow, 4).Value = tk.SL;
          worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
          worksheet.Cell(currentRow, 5).Value = tk.Gia;
          worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
          currentRow++;
          stt++;
        }

        worksheet.Cell(currentRow, 4).FormulaA1 = "=SUM(D6:D" + (currentRow - 1) + ")";
        worksheet.Cell(currentRow, 5).FormulaA1 = "=SUM(E6:E" + (currentRow - 1) + ")";

        worksheet.Range(worksheet.Cell(6, 4), worksheet.Cell(currentRow, 4)).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(worksheet.Cell(6, 5), worksheet.Cell(currentRow, 5)).Style.NumberFormat.NumberFormatId = 4;



        worksheet.Row(currentRow).Style.Font.Bold = true;

        using (var stream = new MemoryStream())
        {
          worksheet.Columns().AdjustToContents();
          workbook.SaveAs(stream);
          var content = stream.ToArray();
          return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeTonKho.xlsx");
        }
      }

    }







  }
}
