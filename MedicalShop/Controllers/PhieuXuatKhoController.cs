using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class PhieuXuatKhoController : Controller
  {

    [Route("/PhieuXuatKho")]
    public IActionResult Index()
    {
      return View("PhieuXuatKho");
    }

    //private ICompositeViewEngine _viewEngine;
    //public PhieuXuatKhoController(ICompositeViewEngine viewEngine)
    //{
    //  _viewEngine = viewEngine;
    //}


    ///////////////////////////////////////////huhu

    [HttpPost("/getDonViTinhPX")]
    public IActionResult getDonViTinhPX(int idHH)
    {
      int idcn = int.Parse(User.Claims.ElementAt(4).Value);
      MedicalShopContext context = new MedicalShopContext();
      // double? GiaBan;
      //KhachHang kh = context.KhachHang.Find(idKH);

      //HangHoa hh = context.HangHoa.Include(x => x.IddvtcNavigation).FirstOrDefault(x => x.Id == idHH);

      // var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

      //Lấy các đơn vị tính của Hàng hoá
      //string options = "<option selected value = '" + hh.Iddvtc + "'>" + hh.IddvtcNavigation.TenDvt + "</option>";
      //foreach (HhDvt d in dvt)
      //{
      //  options += "<option value = '" + d.Iddvt + "'>" + d.IddvtNavigation.TenDvt + "</option>";
      //}

      HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == idHH);

      //int ListCTPNT = (int)context.ChiTietPhieuNhap.Where(x => x.Idhh == idHH).Select(x => x.Quantity).Sum();
      // var slCon = ListCTPNT.Sum(x => x.Slg * x.DonGia);
      string message = "";
      Dvt dvt = context.Dvt.Find(hh.Iddvtc);
      HhGia gia = context.HhGia.FirstOrDefault(y => y.Idhh == idHH);

      //var giaa = context.TonKho
      //                  .Include(x => x.IdctpnNavigation)
      //                  .Include(x => x.IdctpnNavigation.IdhhNavigation)
      //                  .Join(
      //                      context.HhGia,
      //                      tk => tk.IdctpnNavigation.IdhhNavigation.Id,
      //                      hhgia => hhgia.Idhh,
      //                      (tk, hhgia) => new
      //                      {
      //                        TonKho = tk,
      //                        HhGia = hhgia
      //                      })
      //                  .Where(x => x.HhGia.Price > (x.TonKho.IdctpnNavigation.Price*1.05) && x.TonKho.IdctpnNavigation.IdhhNavigation.Id == idHH)
      //                  .Select(x => x.TonKho.IdctpnNavigation.IdhhNavigation)
      //                  /*.ToList()*/;
      







      return Json(new
      {
        dvt = hh.Iddvtc == null ? 0 : hh.Iddvtc,
        tenDVT = dvt == null ? "" : dvt.TenDvt,
        slCon = getSLCon(idHH),
        setgia = gia == null ? false : true,
      });

    
    }



    //////////////////////// huhu
    [HttpPost("/checkgia")]
    public JsonResult Checkgia(int idHH, double SL)
    {
      MedicalShopContext context = new MedicalShopContext();

      ////List<ChiTietPhieuNhap> tkk = context.ChiTietPhieuNhap.Where(x => x.Id == idHH).OrderBy(x => x.CreatedDate).ToList();

      //List<TonKho> tkk = context.TonKho.Where(x => x.Id == idHH).OrderBy(x => x.NgayNhap).ToList();
      List<TonKho> tkk = context.TonKho.Include(x => x.IdctpnNavigation).Where(x => x.IdctpnNavigation.Idhh == idHH).OrderBy(x => x.NgayNhap).ToList();


      double donGia = 0;
      double thanhTien = 0;
      //double SLtemp = 0;
      //double SLT = 0;


      HhGia gia = context.HhGia.FirstOrDefault(y => y.Idhh == idHH);

      if (gia.Price != 0 && gia.Price != null)
      {
        donGia = (double)gia.Price;
        thanhTien = donGia * SL;
      }
      if (gia.TiLe != 0 && gia.TiLe != null)
      {
        var giaa = context.TonKho.Include(x => x.IdctpnNavigation)
                                  .Where(x => x.IdctpnNavigation.Idhh == idHH)
                                  .Max(x => x.IdctpnNavigation.Price);

        var thuee = context.TonKho.Include(x => x.IdctpnNavigation)
                                    .Where(x => x.IdctpnNavigation.Idhh == idHH)
                                    .Max(x => x.IdctpnNavigation.Thue);

        donGia = (double)(giaa * (1 + (gia.TiLe / 100) + (thuee / 100)));
        thanhTien = donGia * SL;
        //if (SL < tkk[0].SoLuong)
        //{
        //  donGia = (double)(tkk[0].IdctpnNavigation.Price * (1 + (gia.TiLe / 100) + ((tkk[0].IdctpnNavigation.Thue) / 100)));
        //  thanhTien = donGia * SL;
        //}
        //else
        //{
        //  foreach (TonKho tk in tkk)
        //  {
        //    if (SLT < SL)
        //    {
        //      if ((SL - SLT) > tk.SoLuong)
        //      {
        //        SLT = (double)(SLT + tk.SoLuong);
        //        SLtemp = (double)tk.SoLuong;
        //      }
        //      else
        //      {
        //        SLtemp = SL - SLT;
        //        SLT = SL;
        //      }

        //      donGia = (double)(tk.IdctpnNavigation.Price * (1 + (gia.TiLe / 100) + ((tkk[0].IdctpnNavigation.Thue) / 100)));
        //      thanhTien = (double)(thanhTien + (donGia * SLtemp));
        //    }
        //    // thành tiền chia đều cho số lượng -> đơn giá
        //    donGia = thanhTien / SL;
        //  }
        //}

      }


      return Json(
          new
          {
            donGia = donGia,
            thanhTien = thanhTien
          });

    }




    [HttpPost("/listCTPXT")]
    public JsonResult ListCTPXT([FromBody] /*IEnumerable<ChiTietPhieuNhapTam> list,  string NgayHd*/ PhieuXuatModel px)
    {
      MedicalShopContext context = new MedicalShopContext();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idCN = int.Parse(User.Claims.ElementAt(4).Value);
      var tran = context.Database.BeginTransaction();
      try
      {
        PhieuXuat phieuXuat = new PhieuXuat();
        phieuXuat.Note = px.Note;
        phieuXuat.SoHd = px.SoHd;
        phieuXuat.Idkh = px.KH;
        phieuXuat.NgayHd = DateTime.ParseExact(px.NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        phieuXuat.CreatedDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        phieuXuat.Active = true;
        phieuXuat.Idcn = idCN;
        phieuXuat.Idnv = idUser;
        phieuXuat.CreatedBy = idUser;
        phieuXuat.ModifiedBy = idUser;
        phieuXuat.SoPx = getSoPhieu();
        context.PhieuXuat.Add(phieuXuat);
        context.SaveChanges();

        foreach (ChiTietPhieuXuatTam t in px.listOfCTPXT)
        {

          //ChiTietPhieuXuat ct = new ChiTietPhieuXuat();
          //ct.Idhh = t.Idhh;
          //ct.Idpx = phieuXuat.Id;
          //ct.Thue = t.Thue;
          //ct.Quantity = t.Slg;
          //ct.Price = t.DonGia;
          //ct.Cktm = t.Cktm;
          //ct.Active = true;
          //ct.CreatedBy = idUser;
          //ct.CreatedDate = DateTime.Now;
          //ct.ModifiedBy = idUser;
          //ct.ModifiedDate = DateTime.Now;
          //trên ok


         // ct.Idctpn  ct.Iddvt px.SoHd
          // ct.CreatedDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
          

          List<TonKho> tkk = context.TonKho.Include(x => x.IdctpnNavigation).Where(x => x.IdctpnNavigation.Idhh == t.Idhh).OrderBy(x => x.NgayNhap).ToList();
          double SLT = (double)t.Slg;
          foreach (TonKho tk in tkk)
          {
            if (SLT < tk.SoLuong)
            {
              //context.TonKho.Remove(tk);
              tk.SoLuong = tk.SoLuong - SLT;
              context.TonKho.Update(tk);
              // var iddvt = context.HangHoa.FirstOrDefault(x => x.Id == t.Idhh).Iddvtc;
              ChiTietPhieuXuat ct = new ChiTietPhieuXuat();
              ct.Idhh = t.Idhh;
              ct.Idpx = phieuXuat.Id;
              ct.Thue = t.Thue;
              ct.Quantity = SLT;
              ct.Price = t.DonGia;
              ct.Iddvt = t.IdDvt;
              ct.Cktm = t.Cktm;
              ct.Active = true;
              ct.CreatedBy = idUser;
              ct.CreatedDate = DateTime.Now;
              ct.ModifiedBy = idUser;
              ct.ModifiedDate = DateTime.Now;
              ct.Idctpn = tk.Idctpn;
              context.ChiTietPhieuXuat.Add(ct);
              context.SaveChanges();
              break;
            }
            else
            {
              SLT = (double)(SLT - tk.SoLuong);
              context.TonKho.Remove(tk);

              ChiTietPhieuXuat ct = new ChiTietPhieuXuat();
              ct.Idhh = t.Idhh;
              ct.Idpx = phieuXuat.Id;
              ct.Thue = t.Thue;
              ct.Quantity = tk.SoLuong;
              ct.Price = t.DonGia;
              ct.Iddvt = t.IdDvt;
              ct.Cktm = t.Cktm;
              ct.Active = true;
              ct.CreatedBy = idUser;
              ct.CreatedDate = DateTime.Now;
              ct.ModifiedBy = idUser;
              ct.ModifiedDate = DateTime.Now;
              ct.Idctpn = tk.Idctpn;
              context.ChiTietPhieuXuat.Add(ct);
              context.SaveChanges();
            }
          }




          //TonKho sl = new TonKho();
          //sl.Idctpn = ct.Id;
          //sl.SoLuong = Math.Round((double)ct.Quantity, 2);
          //sl.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
          //sl.NgayNhap = phieuXuat.CreatedDate;
          //context.TonKho.Add(sl);
          // context.SaveChanges();
        }
        var stt = context.SoThuTu.FromSqlRaw("SELECT * FROM SoThuTu WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,Date) and Type = 'XuatKho'").FirstOrDefault();
        stt.Stt += 1;
        context.SoThuTu.Update(stt);
        context.SaveChanges();
        tran.Commit();
      }
      catch (Exception e)
      {
        tran.Rollback();
        return Json(data: e);
      }
      return Json(data: "Cập nhật thành công!");
    }






    [HttpPost("/ViewThongTinPhieuXuat")]
    public IActionResult ViewThongTinPhieuXuat(int idPX)
    {
      MedicalShopContext context = new MedicalShopContext();
      var phieu = context.PhieuXuat.Include(x => x.ChiTietPhieuXuat).Include(x => x.IdkhNavigation).Include(x => x.IdnvNavigation).FirstOrDefault(x => x.Id == idPX);
      return PartialView(phieu);
      //var phieu = context.PhieuXuat.Include(x => x.ChiTietPhieuXuat).Include(x => x.IdkhNavigation).Include(x => x.IdnvNavigation).FirstOrDefault(x => x.Id == idPX);
      //return PartialView(phieu);
    }



    [HttpPost("/loadTableLichSuXuat")]
    public IActionResult loadTableLichSuXuat(string fromDay, string toDay)
    {
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

      MedicalShopContext context = new MedicalShopContext();
      ViewBag.ListPhieuXuat = context.PhieuXuat
                                              .FromSqlRaw("select*from PhieuXuat where CONVERT(date,CreatedDate) >= '" + FromDay.ToString("yyyy-MM-dd") + "' and CONVERT(date,CreatedDate) <= '" + ToDay.ToString("yyyy-MM-dd") + "' and Active = 1")
                                              .Include(x => x.IdkhNavigation)
                                              .Include(x => x.IdnvNavigation)
                                              .OrderByDescending(x => x.CreatedDate)
                                              .ToList();
      return PartialView("TableLichSuXuat");
    }




    [Route("/download/phieuxuat/{id:int}")]
    public IActionResult downloadPhieuXuat(int id)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

      var pdf = fullView.ConvertUrl(currentUrl + "/PhieuXuatPDF/" + id);

      var pdfBytes = pdf.Save();
      return File(pdfBytes, "application/pdf", "PhieuXuat.pdf");
    }


    [Route("/PhieuXuatPDF/{id:int}")]
    public IActionResult viewPDF(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      var phieu = context.PhieuXuat
          .Include(x => x.IdkhNavigation)
          .Include(x => x.ChiTietPhieuXuat)
          .Where(x => x.Id == id).FirstOrDefault();
      return View("PhieuXuatPDF", phieu);
    }




    [HttpPost("/download/BaoCaoPhieuXuat")]
    public IActionResult downloadBaoCaoPhieuXuat(string fromDay, string toDay, string soPhieuLS, string soHDLS, int khLS, int hhLS)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var url = Url.Action("viewBaoCaoPhieuXuatPDF", "PhieuXuatKho", new { fromDay = fromDay, toDay = toDay, soPhieuLS = soPhieuLS, soHDLS = soHDLS, khLS = khLS, hhLS = hhLS });

      var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + url;

      var pdf = fullView.ConvertUrl(currentUrl);

      var pdfBytes = pdf.Save();
      return File(pdfBytes, "application/pdf", "BaoCaoPhieuXuat.pdf");
    }


    public IActionResult viewBaoCaoPhieuXuatPDF(string fromDay, string toDay, string soPhieuLS, string soHDLS, int khLS, int hhLS)
    {
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      ViewBag.fromDay = fromDay;
      ViewBag.toDay = toDay;
      MedicalShopContext context = new MedicalShopContext();
      List<PhieuXuat> listPhieu = context.PhieuXuat.Where(x => x.CreatedDate.Value.Date >= FromDay
                                          && x.CreatedDate.Value.Date <= ToDay
                                          && x.Active == true)
                                          .Include(x => x.IdkhNavigation)
                                          .Include(x => x.IdnvNavigation)
                                          .Include(x => x.ChiTietPhieuXuat)
                                          .OrderByDescending(x => x.Id)
                                          .ToList();
      if (khLS == 0 && hhLS == 0)
      {

        return View("BaoCaoPhieuXuatPDF", listPhieu.Where(x => (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS) ?? false))
        && (soPhieuLS == null ? true : x.SoPx.Contains(soPhieuLS.ToUpper()))).ToList());
      }
      else
      {
        return View("BaoCaoPhieuXuatPDF", listPhieu.Where(x => (hhLS == 0 ? true : (x.ChiTietPhieuXuat.Where(y => y.Idhh == hhLS).Count() > 0 ? true : false))
        && (khLS == 0 ? true : x.Idkh == khLS)
        && (soPhieuLS == null ? true : x.SoPx.Contains(soPhieuLS.ToUpper()))
        && (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS.ToUpper()) ?? false))).ToList());
      }

    }



    //public ActionResult ExportToExcel()
    //{
    //  MedicalShopContext context = new MedicalShopContext();

    //  List<SP_TonKhoResult> list = context.SP_TonKho().ToList();

    //  using (var workbook = new XLWorkbook())
    //  {
    //    var worksheet = workbook.Worksheets.Add("Danh sách sản phẩm tồn kho");
    //    var currentRow = 1;
    //    worksheet.Cell(currentRow, 1).Value = "Mã sản phẩm";
    //    worksheet.Cell(currentRow, 2).Value = "Tên sản phẩm";
    //    worksheet.Cell(currentRow, 3).Value = "Số lượng nhập";
    //    worksheet.Cell(currentRow, 4).Value = "Số lượng xuất";
    //    worksheet.Cell(currentRow, 5).Value = "Số lượng tồn kho";



    //    foreach (var nv in list)
    //    {
    //      currentRow++;
    //      worksheet.Cell(currentRow, 1).Value = nv.MaSP;
    //      worksheet.Cell(currentRow, 2).Value = nv.TenSP;
    //      worksheet.Cell(currentRow, 3).Value = nv._Số_lượng_xuất;
    //      worksheet.Cell(currentRow, 4).Value = nv._Số_lượng_xuất;
    //      worksheet.Cell(currentRow, 5).Value = nv.Số_lượng_tồn;




    //    }
    //    using (var stream = new MemoryStream())
    //    {
    //      workbook.SaveAs(stream);
    //      var content = stream.ToArray();
    //      return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danhsachtonkho.xlsx");
    //    }
    //  }


    string getSoPhieu()
    {
      MedicalShopContext context = new MedicalShopContext();
      QuyDinhMa qd = context.QuyDinhMa.Find(1);
      string cn = User.Claims.ElementAt(4).Value;

      DateTime d = DateTime.Now;
      string ngayThangNam = d.ToString("yyMMdd");
      string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam;
      var list = context.SoThuTu.FromSqlRaw("SELECT * FROM SoThuTu WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,Date) and Type = 'XuatKho'").FirstOrDefault();
      int stt;
      if (list == null)
      {
        SoThuTu sttt = new SoThuTu();
        sttt.Date = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
        sttt.Stt = 0;
        sttt.Type = "XuatKho";
        context.SoThuTu.Add(sttt);
        context.SaveChanges();
        stt = 1;
      }
      else
      {
        stt = (Int32)list.Stt + 1;
      }
      SoPhieu += stt;
      while (true)
      {
        if (qd.DoDai == SoPhieu.Length) break;
        SoPhieu = SoPhieu.Insert(SoPhieu.Length - stt.ToString().Length, "0");
      }
      return SoPhieu;
    }

    double? getSLCon(int idHH)
    {
      MedicalShopContext context = new MedicalShopContext();
      HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == idHH);

      double? a = context.TonKho.Include(x => x.IdctpnNavigation)
                .Where(x => x.IdctpnNavigation.Idhh == idHH)
                .Sum(x => x.SoLuong);


      //double? a = context.ChiTietPhieuNhap
      //    .Where(x => x.Idhh == idHH)
      //    .Sum(x => x.Quantity);
      return a;
      //if (hh.Iddvtc == idDVT)
      //{
      //  return a;
      //}
      //else
      //{
      //  var sl = (double)context.HhDvt.Where(x => x.Idhh == idHH && x.Iddvt == idDVT).FirstOrDefault().SlquyDoi;
      //  return a * sl;
      //}
    }


  }
}
