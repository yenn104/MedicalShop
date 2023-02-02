using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class PhieuNhapKhoController : Controller
  {

    [Route("/PhieuNhapKho")]
    public IActionResult Index()
    {
      return View("PhieuNhapKho");
    }
    [HttpPost("/them-phieu-nhap")]
    public IActionResult ThemPhieuNhap(PhieuNhap phieuNhap, string NgayHd, string NgayTao)
    {

      MedicalShopContext context = new MedicalShopContext();
      TonKho TonKho = new TonKho();
      int idUser = int.Parse(User.Claims.ElementAt(2).Type);
    //  string h = GetLocalIPAddress();
      // List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == h).OrderByDescending(x => x.Id).ToList();
      var tran = context.Database.BeginTransaction();
      try
      {
        phieuNhap.NgayHd = DateTime.ParseExact(NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        phieuNhap.CreatedDate = DateTime.ParseExact(NgayTao, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
        phieuNhap.Active = true;
        phieuNhap.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
        phieuNhap.Idnv = idUser;
        phieuNhap.SoPn = getSoPhieu();
        context.PhieuNhap.Add(phieuNhap);
        context.SaveChanges();
        //foreach (ChiTietPhieuNhapTam t in ListCTPNT)
        //{
        //  ChiTietPhieuNhap ct = new ChiTietPhieuNhap();
        //  ct.Idhh = t.Idhh;
        //  ct.Idbh = t.Idbh;
        //  ct.Thue = t.Thue;
        //  ct.Idpn = phieuNhap.Id;
        //  ct.Slg = t.Slg;
        //  ct.Tgbh = t.Tgbh;
        //  ct.DonGia = t.DonGia;
        //  ct.Cktm = t.Cktm;
        //  ct.Nsx = t.Nsx;
        //  ct.Hsd = t.Hsd;
        //  ct.SoLo = t.SoLo;
        //  ct.GhiChu = t.GhiChu;
        //  ct.Active = true;
        //  ct.Nvtao = idUser;
        //  ct.NgayTao = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
        //  context.ChiTietPhieuNhap.Add(ct);
        //  context.ChiTietPhieuNhapTam.Remove(t);
        //  context.SaveChanges();

        //  TonKho sl = new TonKho();
        //  sl.Idctpn = ct.Id;
        //  sl.Slcon = Math.Round((double)ct.Slg, 2);
        //  sl.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
        //  sl.NgayNhap = phieuNhap.NgayTao;
        //  context.TonKho.Add(sl);
        //  context.SaveChanges();
        //}
        var stt = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'NhapKho'").FirstOrDefault();
        stt.Stt += 1;
        context.SoThuTu.Update(stt);
        context.SaveChanges();
        tran.Commit();
      }
      catch (Exception e)
      {
        tran.Rollback();
        TempData["ThongBao"] = e.Message;
        return RedirectToAction("Index");
      }
      TempData["ThongBao"] = "Thêm thành công";
      return RedirectToAction("Index");
    }

    //[HttpPost("/getDonViTinh")]
    //public JsonResult getTenDVT(int idHH)
    //{
    //  MedicalShopContext context = new MedicalShopContext();
    //  HangHoa hh = context.HangHoa.Find(idHH);

    //  Dvt dvt = context.Dvt.Find(hh.Iddvtc);
    //  return Json(
    //      new
    //      {
    //        tenDVT = dvt.TenDvt,
    //        GiaBan = hh.GiaBanSi
    //      });
    //}

    //[HttpPost("/add-Chi-Tiet-Phieu")]
    //public JsonResult addChiTietPhieu(int idHH, string SoLo, float ThueXuat, float SL, float DonGia,
    //    float ChietKhau, string HanDung, string NgaySX)
    //{
    //  int idUser = int.Parse(User.Claims.ElementAt(2).Type);
    //  MedicalShopContext context = new MedicalShopContext();
    //  ChiTietPhieuNhapTam ct = new ChiTietPhieuNhapTam();
    //  ct.Active = true;
    //  ct.Idhh = idHH;
    //  ct.SoLo = SoLo;
    //  ct.Thue = Math.Round(ThueXuat, 2);
    //  ct.Slg = Math.Round(SL, 2);
    //  ct.DonGia = Math.Round(DonGia, 2);
    //  ct.Cktm = Math.Round(ChietKhau, 2);
    //  ct.Hsd = DateTime.ParseExact(HanDung, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    //  ct.Nsx = DateTime.ParseExact(NgaySX, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    //  ct.Host = GetLocalIPAddress();
    //  ct.Nvtao = idUser;
    //  ct.NgayTao = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
    //  context.ChiTietPhieuNhapTam.Add(ct);
    //  context.SaveChanges();

    //  string ht = GetLocalIPAddress();
    //  List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
    //  var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
    //  var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
    //  var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
    //  var TienThanhToan = TienHang - TienCK + TienThue;

    //  PartialViewResult partialViewResult = PartialView("TableChiTietPhieuNhap");

    //  string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

    //  return Json(new
    //  {
    //    table = viewContent,
    //    tienHang = TienHang,
    //    tienCK = TienCK,
    //    tienThue = TienThue,
    //    tienThanhToan = TienThanhToan
    //  });
    //}

    //[HttpPost("/edit-Chi-Tiet-Phieu")]
    //public IActionResult editChiTietPhieu(int idHH, string SoLo, float ThueXuat, int SL, float DonGia,
    //    float ThanhTien, float ChietKhau, string HanDung, string NgaySX, int id)
    //{
    //  MedicalShopContext context = new MedicalShopContext();
    //  int idUser = int.Parse(User.Claims.ElementAt(2).Type);
    //  ChiTietPhieuNhapTam ct = context.ChiTietPhieuNhapTam.Find(id);
    //  ct.Active = true;
    //  ct.Idhh = idHH;
    //  ct.SoLo = SoLo;
    //  ct.Thue = ThueXuat;
    //  ct.Slg = SL;
    //  ct.DonGia = DonGia;
    //  ct.Cktm = ChietKhau;
    //  ct.Hsd = DateTime.ParseExact(HanDung, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    //  ct.Nsx = DateTime.ParseExact(NgaySX, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    //  ct.Host = GetLocalIPAddress();
    //  ct.Nvtao = idUser;
    //  context.ChiTietPhieuNhapTam.Update(ct);
    //  context.SaveChanges();

    //  string ht = GetLocalIPAddress();
    //  List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
    //  var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
    //  var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
    //  var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
    //  var TienThanhToan = TienHang - TienCK + TienThue;

    //  PartialViewResult partialViewResult = PartialView("TableChiTietPhieuNhap");

    //  string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

    //  return Json(new
    //  {
    //    table = viewContent,
    //    tienHang = TienHang,
    //    tienCK = TienCK,
    //    tienThue = TienThue,
    //    tienThanhToan = TienThanhToan
    //  });
    //}

    //[HttpPost("/delete-Chi-Tiet-Phieu")]
    //public IActionResult deletePhieuNhapTam(int id)
    //{
    //  MedicalShopContext context = new MedicalShopContext();
    //  ChiTietPhieuNhapTam ch = context.ChiTietPhieuNhapTam.Find(id);
    //  context.ChiTietPhieuNhapTam.Remove(ch);
    //  context.SaveChanges();

    //  string ht = GetLocalIPAddress();
    //  List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
    //  var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
    //  var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
    //  var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
    //  var TienThanhToan = TienHang - TienCK + TienThue;

    //  PartialViewResult partialViewResult = PartialView("TableChiTietPhieuNhap");

    //  string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

    //  return Json(new
    //  {
    //    table = viewContent,
    //    tienHang = TienHang,
    //    tienCK = TienCK,
    //    tienThue = TienThue,
    //    tienThanhToan = TienThanhToan
    //  });
    //}

    [HttpPost("/load-table-chitiet")]
    public IActionResult loadTableChitiet()
    {
      return PartialView("TableChiTietPhieuNhap");
    }

    //[HttpPost("/editChitietPhieuNhapTam")]
    //public IActionResult editChitietPhieuNhapTam(int id)
    //{
    //  MedicalShopContext context = new MedicalShopContext();
    //  if (id > 0)
    //  {
    //    return PartialView("GroupChitietPhieuNhapTam", context.ChiTietPhieuNhapTam.Find(id));
    //  }
    //  else
    //  {
    //    return PartialView("GroupChitietPhieuNhapTam");
    //  }
    //}

    [HttpPost("/ViewThongTinPhieuNhap")]
    public IActionResult ViewThongTinPhieuNhap(int idPN)
    {
      MedicalShopContext context = new MedicalShopContext();
      var phieu = context.PhieuNhap.Include(x => x.ChiTietPhieuNhap).Include(x => x.IdnccNavigation).Include(x => x.IdnvNavigation).FirstOrDefault(x => x.Id == idPN);
      return PartialView(phieu);
    }

    [HttpPost("/loadTableLichSuNhap")]
    public IActionResult loadTableLichSuNhap(string fromDay, string toDay)
    {
      DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
      DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

      MedicalShopContext context = new MedicalShopContext();
      ViewBag.ListPhieuNhap = context.PhieuNhap
                                              .FromSqlRaw("select*from PhieuNhap where CONVERT(date,NgayTao) >= '" + FromDay.ToString("yyyy-MM-dd") + "' and CONVERT(date,NgayTao) <= '" + ToDay.ToString("yyyy-MM-dd") + "' and Active = 1")
                                              .Include(x => x.IdnccNavigation)
                                              .Include(x => x.IdnvNavigation)
                                              .OrderByDescending(x => x.Id)
                                              .ToList();
      return PartialView("TableLichSuNhap");
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
    //string GetLocalIPAddress()
    //{
    //  var host = Dns.GetHostEntry(Dns.GetHostName());
    //  foreach (var ip in host.AddressList)
    //  {
    //    if (ip.AddressFamily == AddressFamily.InterNetwork)
    //    {
    //      return ip.ToString();
    //    }
    //  }
    //  throw new Exception("No network adapters with an IPv4 address in the system!");
    //}

    string getSoPhieu()
    {
      MedicalShopContext context = new MedicalShopContext();
      QuyDinhMa qd = context.QuyDinhMa.Find(1);
      //ID chi nhánh
      string cn = "1";

      DateTime d = DateTime.Now;
      string ngayThangNam = d.ToString("yyMMdd");
      string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam;
      var list = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'NhapKho'").FirstOrDefault();
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
