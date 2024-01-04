using AspNetCore.Reporting;
using ClosedXML.Excel;
using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "NV")]
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

            //lấy ngày nhập kho đầu tiên
            ViewBag.minCreateDay = (DateTime)context.TonKho
                                                  .Include(x => x.IdctpnNavigation)
                                                  .ThenInclude(x => x.IdpnNavigation)
                                                  .Min(x => x.IdctpnNavigation.IdpnNavigation.CreatedDate);

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
                NCC = g.Select(x => x.ncc.TenNcc).FirstOrDefault(),
                MaHH = g.Key.MaHh,
                TenHH = g.Key.TenHh,
                SoLo = g.Select(x => x.ctn.SoLo).FirstOrDefault(),
                HSD = (DateTime)g.Select(x => x.ctn.Hsd).FirstOrDefault(),
                SLNhap = (double)g.Select(x => x.ctn.Quantity).FirstOrDefault(),
                SLXuat = ((double)(g.Select(x => x.ctn.Quantity).FirstOrDefault() - g.Select(tk => tk.tk.SoLuong).FirstOrDefault())),
                SLTon = (double)g.Select(tk => tk.tk.SoLuong).FirstOrDefault(),
                DVT = g.Select(x => x.hh.IddvtcNavigation.TenDvt).FirstOrDefault(),
                DonGia = (double)g.Select(x => x.ctn.Price).FirstOrDefault(),
                ThanhTien = (double)(g.Select(x => x.ctn.Price).FirstOrDefault() * g.Select(tk => tk.tk.SoLuong).FirstOrDefault()),
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
            //DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
            DateTime minCreateDay = (DateTime)context.TonKho.Include(x => x.IdctpnNavigation).Min(x => x.IdctpnNavigation.CreatedDate);

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
            TtdoanhNghiep dn = context.TtdoanhNghiep.FirstOrDefault();
            ViewBag.fromDay = fromDay;
            ViewBag.toDay = toDay;
            ViewBag.nhomhh = idnhh == 0 ? "Tất cả" : context.NhomHangHoa.Find(idnhh).TenNhh;
            ViewBag.hanghoa = idhh == 0 ? "Tất cả" : context.HangHoa.Find(idhh).TenHh;
            int index = 0;


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
                worksheet.Cell(currentRow, 1).Value = dn.Name;
                currentRow++;
                worksheet.Range(currentRow, 1, currentRow, 5).Merge();
                worksheet.Cell(currentRow, 1).Value = dn.Address;
                currentRow++;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge();
                worksheet.Cell(currentRow, 1).Value = dn.Phone;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge();

                worksheet.Cell(currentRow, 2).Value = dn.Mail;
                currentRow++;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge();

                worksheet.Cell(currentRow, 1).Value = dn.TaxCode;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge();

                worksheet.Cell(currentRow, 2).Value = dn.Stk;
                currentRow++;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge();

                worksheet.Cell(currentRow, 1).Value = dn.Bank;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge();

                worksheet.Cell(currentRow, 2).Value = dn.Holder;



                worksheet.Range(currentRow, 1, currentRow, 5).Merge();

                //worksheet.Cell(currentRow, 1).Value = "Báo cáo tồn kho tổng hợp";
                var titleCell = worksheet.Cell(currentRow, 1);
                titleCell.Value = "BÁO CÁO TỒN KHO TỔNG HỢP";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontName = "Times New Roman";
                titleCell.Style.Font.FontSize = 18;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                currentRow++;

                worksheet.Range(currentRow, 1, currentRow, 5).Merge();
                var ngay = worksheet.Cell(currentRow, 1);
                ngay.Value = "(" + fromDay + " - " + toDay + ")";

                ngay.Style.Font.FontSize = 12;
                ngay.Style.Font.FontName = "Times New Roman";
                ngay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
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

                worksheet.Range(worksheet.Cell(9, 4), worksheet.Cell(currentRow, 4)).Style.NumberFormat.NumberFormatId = 4;
                worksheet.Range(worksheet.Cell(9, 5), worksheet.Cell(currentRow, 5)).Style.NumberFormat.NumberFormatId = 4;



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
        public IActionResult downloadPDFChiTiet(int idncc, int idnhh1, int idhh1, string fromDay1, string toDay1, string dateDay1)
        {
            var fullView = new HtmlToPdf();
            fullView.Options.WebPageWidth = 1280;
            fullView.Options.PdfPageSize = PdfPageSize.A4;
            fullView.Options.MarginTop = 20;
            fullView.Options.MarginBottom = 20;
            fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var url = Url.Action("viewCTPDF", "TonKho", new { fromDay = fromDay1, toDay = toDay1, idnhh = idnhh1, idhh = idhh1, idncc = idncc, dateDay = dateDay1 });

            var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + url;

            var pdf = fullView.ConvertUrl(currentUrl);

            var pdfBytes = pdf.Save();
            return File(pdfBytes, "application/pdf", "BaoCaoTonKho.pdf");
        }





        //[Route("/TonKhoChiTietPDF/{idnhh:int}/{idhh:int}/{fromDay}/{toDay}/{dateDay}/{idncc:int}")]
        public IActionResult viewCTPDF(int idncc, int idnhh, int idhh, string fromDay, string toDay, string dateDay)
        {
            MedicalShopContext context = new MedicalShopContext();
            int index = 0;
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
            DateTime DateDay = DateTime.ParseExact(dateDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);

            ViewBag.fromDay = fromDay;
            ViewBag.toDay = toDay;
            ViewBag.dateDay = dateDay;
            ViewBag.nhomhh = idnhh == 0 ? "Tất cả" : context.NhomHangHoa.Find(idnhh).TenNhh;
            ViewBag.nhacc = idncc == 0 ? "Tất cả" : context.NhaCungCap.Find(idncc).TenNcc;
            ViewBag.hanghoa = idhh == 0 ? "Tất cả" : context.HangHoa.Find(idhh).TenHh;


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
                NCC = g.Select(x => x.ncc.TenNcc).FirstOrDefault(),
                MaHH = g.Key.MaHh,
                TenHH = g.Key.TenHh,
                SoLo = g.Select(x => x.ctn.SoLo).FirstOrDefault(),
                HSD = (DateTime)g.Select(x => x.ctn.Hsd).FirstOrDefault(),
                SLNhap = (double)g.Select(x => x.ctn.Quantity).FirstOrDefault(),
                SLXuat = ((double)(g.Select(x => x.ctn.Quantity).FirstOrDefault() - g.Select(tk => tk.tk.SoLuong).FirstOrDefault())),
                SLTon = (double)g.Select(tk => tk.tk.SoLuong).FirstOrDefault(),
                DVT = g.Select(x => x.hh.IddvtcNavigation.TenDvt).FirstOrDefault(),
                DonGia = (double)g.Select(x => x.ctn.Price).FirstOrDefault(),
                ThanhTien = (double)(g.Select(x => x.ctn.Price).FirstOrDefault() * g.Select(tk => tk.tk.SoLuong).FirstOrDefault()),
            })
            .OrderBy(r => r.TenHH)
            .ToList();

            return View("TonKhoChiTietPDF", results);
        }


        [Route("/download/tonkhochitietexcel/")]
        public ActionResult ChiTietExportToExcel(int idncc, int idnhh1, int idhh1, string fromDay1, string toDay1, string dateDay1)
        {
            MedicalShopContext context = new MedicalShopContext();
            DateTime FromDay = DateTime.ParseExact(fromDay1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay1, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
            DateTime DateDay = DateTime.ParseExact(dateDay1, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99);
            int index = 0;
            ViewBag.fromDay = fromDay1;
            ViewBag.toDay = toDay1;
            ViewBag.dateDay = dateDay1;
            ViewBag.nhomhh = idnhh1 == 0 ? "Tất cả" : context.NhomHangHoa.Find(idnhh1).TenNhh;
            ViewBag.hanghoa = idhh1 == 0 ? "Tất cả" : context.HangHoa.Find(idhh1).TenHh;
            ViewBag.nhacc = idncc == 0 ? "Tất cả" : context.NhaCungCap.Find(idncc).TenNcc;


            var results = context.TonKho
            .Join(context.ChiTietPhieuNhap, tk => tk.Idctpn, ctn => ctn.Id, (tk, ctn) => new { tk, ctn })
            .Join(context.HangHoa.Include(hh => hh.IddvtcNavigation).Where(hh => (idnhh1 == 0 ? true : hh.Idnhh == idnhh1) && (idhh1 == 0 ? true : hh.Id == idhh1)), x => x.ctn.Idhh, hh => hh.Id, (x, hh) => new { x.tk, x.ctn, hh })
            .Join(context.PhieuNhap, x => x.ctn.Idpn, pn => pn.Id, (x, pn) => new { x.tk, x.ctn, x.hh, pn })
            .Join(context.NhaCungCap.Where(ncc => (idncc == 0 ? true : ncc.Id == idncc)), x => x.pn.Idncc, ncc => ncc.Id, (x, ncc) => new { x.tk, x.ctn, x.hh, x.pn, ncc })
            .Where(tk => tk.tk.NgayNhap >= FromDay && tk.tk.NgayNhap <= ToDay && tk.ctn.Hsd <= DateDay)
            .AsEnumerable()
            .GroupBy(x => new { x.hh.MaHh, x.hh.TenHh, x.ncc.TenNcc })
            .Select(g => new TonKhoChiTietModel
            {
                STT = index + 1,
                NgayNhap = (DateTime)g.Select(x => x.ctn.CreatedDate).FirstOrDefault(),
                NCC = g.Select(x => x.ncc.TenNcc).FirstOrDefault(),
                MaHH = g.Key.MaHh,
                TenHH = g.Key.TenHh,
                SoLo = g.Select(x => x.ctn.SoLo).FirstOrDefault(),
                HSD = (DateTime)g.Select(x => x.ctn.Hsd).FirstOrDefault(),
                SLNhap = (double)g.Select(x => x.ctn.Quantity).FirstOrDefault(),
                SLXuat = ((double)(g.Select(x => x.ctn.Quantity).FirstOrDefault() - g.Select(tk => tk.tk.SoLuong).FirstOrDefault())),
                SLTon = (double)g.Select(tk => tk.tk.SoLuong).FirstOrDefault(),
                DVT = g.Select(x => x.hh.IddvtcNavigation.TenDvt).FirstOrDefault(),
                DonGia = (double)g.Select(x => x.ctn.Price).FirstOrDefault(),
                ThanhTien = (double)(g.Select(x => x.ctn.Price).FirstOrDefault() * g.Select(tk => tk.tk.SoLuong).FirstOrDefault()),
            })
            .OrderBy(r => r.TenHH)
            .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Báo cáo tồn kho tổng hợp");

                var currentRow = 1;
                int stt = 1;
                worksheet.Range(currentRow, 1, currentRow, 13).Merge();

                //worksheet.Cell(currentRow, 1).Value = "Báo cáo tồn kho tổng hợp";
                var titleCell = worksheet.Cell(currentRow, 1);
                titleCell.Value = "BÁO CÁO TỒN KHO CHI TIẾT";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontName = "Times New Roman";
                titleCell.Style.Font.FontSize = 20;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                currentRow++;

                worksheet.Range(currentRow, 1, currentRow, 13).Merge();
                var ngay = worksheet.Cell(currentRow, 1);
                ngay.Value = "(" + fromDay1 + " - " + toDay1 + ")";
                ngay.Style.Font.FontSize = 12;
                ngay.Style.Font.FontName = "Times New Roman";
                ngay.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                currentRow += 2;

                worksheet.Cell(currentRow, 1).Value = "STT";
                worksheet.Cell(currentRow, 2).Value = "Ngày nhập";
                worksheet.Cell(currentRow, 3).Value = "Nhà cung cấp";
                worksheet.Cell(currentRow, 4).Value = "Mã HH";
                worksheet.Cell(currentRow, 5).Value = "Tên HH";
                worksheet.Cell(currentRow, 6).Value = "Số lô";
                worksheet.Cell(currentRow, 7).Value = "HSĐ";
                worksheet.Cell(currentRow, 8).Value = "SL nhập";

                worksheet.Cell(currentRow, 9).Value = "SL xuất";
                worksheet.Cell(currentRow, 10).Value = "SL tồn";
                worksheet.Cell(currentRow, 11).Value = "ĐVT";
                worksheet.Cell(currentRow, 12).Value = "Đơn giá";
                worksheet.Cell(currentRow, 13).Value = "Thành tiền";
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                currentRow++;
                foreach (var tk in results)
                {

                    worksheet.Cell(currentRow, 1).Value = stt;
                    worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 2).Value = tk.NgayNhap;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(currentRow, 3).Value = tk.NCC;
                    worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(currentRow, 4).Value = tk.MaHH;
                    worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(currentRow, 5).Value = tk.TenHH;
                    worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(currentRow, 6).Value = tk.SoLo;
                    worksheet.Cell(currentRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 7).Value = tk.HSD;
                    worksheet.Cell(currentRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    worksheet.Cell(currentRow, 8).Value = tk.SLNhap;
                    worksheet.Cell(currentRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 9).Value = tk.SLXuat;
                    worksheet.Cell(currentRow, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 10).Value = tk.SLTon;
                    worksheet.Cell(currentRow, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 11).Value = tk.DVT;
                    worksheet.Cell(currentRow, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(currentRow, 12).Value = tk.DonGia;
                    worksheet.Cell(currentRow, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 13).Value = tk.ThanhTien;
                    worksheet.Cell(currentRow, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    currentRow++;
                    stt++;
                }

                worksheet.Range(currentRow, 1, currentRow, 7).Merge();

                //worksheet.Cell(currentRow, 1).Value = "Báo cáo tồn kho tổng hợp";
                titleCell = worksheet.Cell(currentRow, 1);
                titleCell.Value = "Tổng";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontName = "Times New Roman";
                titleCell.Style.Font.FontSize = 12;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                worksheet.Cell(currentRow, 8).FormulaA1 = "=SUM(H5:H" + (currentRow - 1) + ")";
                worksheet.Cell(currentRow, 9).FormulaA1 = "=SUM(I5:I" + (currentRow - 1) + ")";
                worksheet.Cell(currentRow, 10).FormulaA1 = "=SUM(J5:J" + (currentRow - 1) + ")";
                worksheet.Cell(currentRow, 12).FormulaA1 = "=SUM(L5:L" + (currentRow - 1) + ")";
                worksheet.Cell(currentRow, 13).FormulaA1 = "=SUM(M5:M" + (currentRow - 1) + ")";


                worksheet.Range(worksheet.Cell(5, 8), worksheet.Cell(currentRow, 8)).Style.NumberFormat.NumberFormatId = 4;
                worksheet.Range(worksheet.Cell(5, 9), worksheet.Cell(currentRow, 9)).Style.NumberFormat.NumberFormatId = 4;
                worksheet.Range(worksheet.Cell(5, 10), worksheet.Cell(currentRow, 10)).Style.NumberFormat.NumberFormatId = 4;
                worksheet.Range(worksheet.Cell(5, 12), worksheet.Cell(currentRow, 12)).Style.NumberFormat.NumberFormatId = 4;
                worksheet.Range(worksheet.Cell(5, 13), worksheet.Cell(currentRow, 13)).Style.NumberFormat.NumberFormatId = 4;



                worksheet.Row(currentRow).Style.Font.Bold = true;

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TonKhoChiTiet.xlsx");
                }
            }

        }









        [Route("/canhbaotonkho")]
        public IActionResult CanhBaoTonKho()
        {
            MedicalShopContext context = new MedicalShopContext();

            var results = context.TonKho.Select(x => x.Idhh).Distinct().ToList(); //.Where(x => x.IdhhNavigation.Active == true)

            var listHangHoa = context.HangHoa.Where(hh => hh.Active == true).ToList();

            // tìm ra list tồn dưới mức cho phép
            var listTon = context.TonKho.GroupBy(x => x.Idhh)
               .Select(group => new
               {
                   Idhh = group.Key,
                   SoLuong = group.Sum(x => x.SoLuong)
               })
               .ToList();

            //tìm ra list không tồn
            var listIDHangHoa = listHangHoa.Select(x => x.Id).ToList();
            var listTonIds = listTon.Select(x => x.Idhh).Distinct().ToList();

            var listIDHangHoaKhongTon = listIDHangHoa.Where(id => !listTonIds.Contains(id)).ToList();
            var listIDHangHoaTon = listIDHangHoa.Where(id => !listIDHangHoaKhongTon.Contains(id)).ToList();

            // Lọc ra những Idhh có tổng SoLuong trong listTon bé hơn SoLuongCanhBao trong HangHoa
            var listIdhhCanhBao = listTon
                .Join(listHangHoa,
                      ton => ton.Idhh,
                      hangHoa => hangHoa.Id,
                      (ton, hangHoa) => new
                      {
                          Idhh = ton.Idhh,
                          SoLuong = ton.SoLuong,
                          SoLuongCanhBao = hangHoa.SoLuongCanhBao
                      })
                .Where(x => x.SoLuong < x.SoLuongCanhBao)
                .Select(x => x.Idhh)
                .ToList();


            var listIDHangHoaCanhBao = listIdhhCanhBao
                                        .Select(id => (int?)id) // Chuyển đổi từ int sang int?
                                        .Concat(listIDHangHoaKhongTon.Select(id => (int?)id))
                                        .Distinct()
                                        .ToList();


            var listHangHoaCanhBao = listHangHoa.Where(x => listIDHangHoaCanhBao.Contains(x.Id)).ToList();

            //var result = listIDHangHoaCanhBao
            //                .Select(id =>
            //                {
            //                    var tonInfo = listTon.FirstOrDefault(ton => ton.Idhh == id);
            //                    var hangHoa = listHangHoa.FirstOrDefault(hh => hh.Id == id);
            //                    return new
            //                    {
            //                        Idhh = id,
            //                        MaHh = hangHoa.MaHh,
            //                        TenHh = hangHoa.TenHh,
            //                        SoLuong = tonInfo == null ? 0 : tonInfo.SoLuong,
            //                        SoLuongCanhBao = hangHoa.SoLuongCanhBao == null ? 0 : hangHoa.SoLuongCanhBao
            //                    };
            //                })
            //                .ToList();

            List<CanhBaoTonKho> result = listIDHangHoaCanhBao
                                        .Select(id =>
                                        {
                                            var tonInfo = listTon.FirstOrDefault(ton => ton.Idhh == id);
                                            var hangHoa = listHangHoa.FirstOrDefault(hh => hh.Id == id);
                                            return new CanhBaoTonKho
                                            {
                                                Idhh = id,
                                                MaHh = hangHoa?.MaHh,
                                                TenHh = hangHoa?.TenHh,
                                                SoLuong = tonInfo?.SoLuong ?? 0,
                                                SoLuongCanhBao = hangHoa?.SoLuongCanhBao ?? 0,
                                                Iddvtc = hangHoa?.Iddvtc,
                                            };
                                        })
                                        .ToList();



            ViewBag.CanhBaoTonKho = result;
            return View();
        }
    }
    public class CanhBaoTonKho
    {
        public int? Idhh { get; set; }
        public string MaHh { get; set; }
        public string TenHh { get; set; }
        public double? SoLuong { get; set; }
        public double? SoLuongCanhBao { get; set; }
        public int? Iddvtc { get; set; }
    };

    public class CanhBaoTonKhoHSD
    {
        public int? Idhh { get; set; }
        public string MaHh { get; set; }
        public string TenHh { get; set; }
        public DateTime NgayNhap { get; set; }
        public DateTime HanSuDung { get; set; }
        public double? SoLuong { get; set; }
        public int? Iddvtc { get; set; }
    };
}
