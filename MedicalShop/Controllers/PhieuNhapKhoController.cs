using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  [Authorize(Roles = "NV")]
  public class PhieuNhapKhoController : Controller
  {

    [Route("/PhieuNhapKho")]
    public IActionResult Index()
    {
      return View("PhieuNhapKho");
    }


    [HttpPost("/listCTPNT")]
    public JsonResult ListCTPNT([FromBody] /*IEnumerable<ChiTietPhieuNhapTam> list,  string NgayHd*/ PhieuNhapModel pn)
    {
      MedicalShopContext context = new MedicalShopContext();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
      int idCN = int.Parse(User.Claims.ElementAt(4).Value);
      var tran = context.Database.BeginTransaction();
      try
      {
        PhieuNhap phieuNhap = new PhieuNhap();
        phieuNhap.Note = pn.Note;
        phieuNhap.SoHd = pn.SoHd;
        phieuNhap.Idncc = pn.NCC;
        phieuNhap.NgayHd = DateTime.ParseExact(pn.NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        phieuNhap.CreatedDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        phieuNhap.Active = true;
        phieuNhap.Idcn = idCN;
        phieuNhap.Idnv = idUser;
        phieuNhap.CreatedBy = idUser;
        phieuNhap.ModifiedBy = idUser;
        phieuNhap.SoPn = getSoPhieu();
        context.PhieuNhap.Add(phieuNhap);
        context.SaveChanges();
          
        foreach (ChiTietPhieuNhapTam t in pn.listOfCTPNT)
        {
          ChiTietPhieuNhap ct = new ChiTietPhieuNhap();
          ct.Idhh = t.Idhh;
          ct.Idpn = phieuNhap.Id;
          ct.Thue = t.Thue;
          ct.Quantity = t.Slg;
          ct.Price = t.DonGia;
          //ct.SalePrice = t.DonGia + (t.DonGia * 0.5);
          ct.Cktm = t.Cktm;
          ct.Nsx = DateTime.ParseExact(t.Nsx, "dd-MM-yyyy", CultureInfo.InvariantCulture); 
          ct.Hsd = DateTime.ParseExact(t.Hsd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
          ct.SoLo = t.SoLo;
          ct.Active = true;
          ct.CreatedBy = idUser;
          ct.CreatedDate = DateTime.Now;
          ct.ModifiedBy = idUser;
          ct.ModifiedDate = DateTime.Now;
          // ct.CreatedDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
          context.ChiTietPhieuNhap.Add(ct);
          context.SaveChanges();

          TonKho sl = new TonKho();
          sl.Idctpn = ct.Id;
          sl.SoLuong = Math.Round((double)ct.Quantity, 2);
          sl.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
          sl.NgayNhap = phieuNhap.CreatedDate;
          context.TonKho.Add(sl);
          context.SaveChanges();
        }
        var stt = context.SoThuTu.FromSqlRaw("SELECT * FROM SoThuTu WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,Date) and Type = 'NhapKho'").FirstOrDefault();
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





    [HttpPost("/getDonViTinh")]
    public JsonResult getTenDVT(int idHH)
    {
      MedicalShopContext context = new MedicalShopContext();
      HangHoa hh = context.HangHoa.Find(idHH);

      Dvt dvt = context.Dvt.Find(hh.Iddvtc);
      return Json(
          new
          {
            tenDVT = dvt.TenDvt,
          });
    }


    //chi tiết phiếu nhập (lịch sử)
    [HttpPost("/ViewThongTinPhieuNhap")]
    public IActionResult ViewThongTinPhieuNhap(int idPN)
    {
      MedicalShopContext context = new MedicalShopContext();
      var phieu = context.PhieuNhap.Include(x => x.ChiTietPhieuNhap).Include(x => x.IdnccNavigation).Include(x => x.IdnvNavigation).FirstOrDefault(x => x.Id == idPN);
      return PartialView(phieu);
    }


    //lịch sử nhập ok
    [HttpPost("/loadTableLichSuNhap")]
    public IActionResult loadTableLichSuNhap(string fromDay, string toDay)
    {
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

      MedicalShopContext context = new MedicalShopContext();
      ViewBag.ListPhieuNhap = context.PhieuNhap
                                              .FromSqlRaw("SELECT * FROM PhieuNhap WHERE CONVERT(date,CreatedDate) >= '" + FromDay.ToString("yyyy-MM-dd") + "' and CONVERT(date,CreatedDate) <= '" + ToDay.ToString("yyyy-MM-dd") + "' and Active = 1")
                                              .Include(x => x.IdnccNavigation)
                                              .Include(x => x.IdnvNavigation)
                                              .OrderByDescending(x => x.Id)
                                              .ToList();
      return PartialView("TableLichSuNhap");
    }







    [Route("/download/phieunhap/{id:int}")]
    public IActionResult downloadPhieuNhap(int id)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

      var pdf = fullView.ConvertUrl(currentUrl + "/PhieuNhapPDF/" + id);

      var pdfBytes = pdf.Save();
      return File(pdfBytes, "application/pdf", "PhieuNhap.pdf");
    }


    [Route("/PhieuNhapPDF/{id:int}")]
    public IActionResult viewPDF(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      var phieu = context.PhieuNhap
          .Include(x => x.IdnccNavigation)
          .Include(x => x.ChiTietPhieuNhap)
          .Where(x => x.Id == id).FirstOrDefault();
      return View("PhieuNhapPDF", phieu);
    }




    [HttpPost("/download/BaoCaoPhieuNhap")]
    public IActionResult downloadBaoCaoPhieuNhap(string fromDay, string toDay, string soPhieuLS, string soHDLS, int nhaCC, int hhLS)
    {
      var fullView = new HtmlToPdf();
      fullView.Options.WebPageWidth = 1280;
      fullView.Options.PdfPageSize = PdfPageSize.A4;
      fullView.Options.MarginTop = 20;
      fullView.Options.MarginBottom = 20;
      fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

      var url = Url.Action("viewBaoCaoPhieuNhapPDF", "PhieuNhapKho", new { fromDay = fromDay, toDay = toDay, soPhieuLS = soPhieuLS, soHDLS = soHDLS, nhaCC = nhaCC, hhLS = hhLS });

      var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + url;

      var pdf = fullView.ConvertUrl(currentUrl);

      var pdfBytes = pdf.Save();
      return File(pdfBytes, "application/pdf", "BaoCaoPhieuNhap.pdf");
    }
    public IActionResult viewBaoCaoPhieuNhapPDF(string fromDay, string toDay, string soPhieuLS, string soHDLS, int nhaCC, int hhLS)
    {
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      ViewBag.fromDay = fromDay;
      ViewBag.toDay = toDay;
      MedicalShopContext context = new MedicalShopContext();
      List<PhieuNhap> listPhieu = context.PhieuNhap
      .Where(x => x.CreatedDate.Value.Date >= FromDay
      && x.CreatedDate.Value.Date <= ToDay
      && x.Active == true)
      .Include(x => x.IdnccNavigation)
      .Include(x => x.IdnvNavigation)
      .Include(x => x.ChiTietPhieuNhap)
      .OrderByDescending(x => x.Id)
      .ToList();
      if (nhaCC == 0 && hhLS == 0)
      {

        return View("BaoCaoPhieuNhapPDF", listPhieu.Where(x => (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS) ?? false))
        && (soPhieuLS == null ? true : x.SoPn.Contains(soPhieuLS.ToUpper()))).ToList());
      }
      else
      {
        return View("BaoCaoPhieuNhapPDF", listPhieu.Where(x => (hhLS == 0 ? true : (x.ChiTietPhieuNhap.Where(y => y.Idhh == hhLS).Count() > 0 ? true : false))
        && (nhaCC == 0 ? true : x.Idncc == nhaCC)
        && (soPhieuLS == null ? true : x.SoPn.Contains(soPhieuLS.ToUpper()))
        && (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS.ToUpper()) ?? false))).ToList());
      }

    }





    public string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
    {
      using (StringWriter writer = new StringWriter())
      {
        ViewEngineResult vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
        ViewContext viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());

        vResult.View.RenderAsync(viewContext);

        return writer.GetStringBuilder().ToString();
      }
    }


    string getSoPhieu()
    {
      MedicalShopContext context = new MedicalShopContext();
      QuyDinhMa qd = context.QuyDinhMa.Find(1);
      string cn = User.Claims.ElementAt(4).Value;

      DateTime d = DateTime.Now;
      string ngayThangNam = d.ToString("yyMMdd");
      string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam;
      var list = context.SoThuTu.FromSqlRaw("SELECT * FROM SoThuTu WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,Date) and Type = 'NhapKho'").FirstOrDefault();
      int stt;
      if (list == null)
      {
        SoThuTu sttt = new SoThuTu();
        sttt.Date = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
        sttt.Stt = 0;
        sttt.Type = "NhapKho";
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



  }
}
