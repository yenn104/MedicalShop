using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
      Dvt dvt = context.Dvt.Find(hh.Iddvtc);

      return Json(new
      {
        dvt = hh.Iddvtc,
        tenDVT = dvt.TenDvt,
        //giaBan = GiaBan,
        slCon = getSLCon(idHH),
        //hinhAnh = hh.Image
      });

      //lấy giá theo khách hàng
      //var GiaTheoKH = context.GiaTheoKhachHang
      //    .Where(x => x.Idkh == idKH && x.Idhh == idHH && x.Iddvt == hh.Iddvtc)
      //    .FirstOrDefault();

      // lấy giá nhập của hàng hoá
      // SoLuongHhcon GiaHangTon;

      //    if (cachXuat.TheoTgnhap == true)
      //{
      //  GiaHangTon = context.SoLuongHhcon
      //      .Include(x => x.IdctpnNavigation)
      //      .Where(x => x.IdctpnNavigation.Idhh == idHH)
      //      .OrderBy(x => x.NgayNhap).
      //      FirstOrDefault();
      //}
      //else
      //{
      //  GiaHangTon = context.SoLuongHhcon
      //      .Include(x => x.IdctpnNavigation)
      //      .Where(x => x.IdctpnNavigation.Idhh == idHH)
      //      .OrderBy(x => x.IdctpnNavigation.Hsd)
      //      .FirstOrDefault();
      //}
      //if (GiaHangTon == null)
      //{
      //  return Json(new
      //  {
      //    options = options,
      //    giaBan = 0,
      //    slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //    hinhAnh = hh.Avatar
      //  });
      //}


      ////nếu giá theo khách hàng không tồn tại thì xét tiếp
      //if (GiaTheoKH == null || (GiaTheoKH.TiLeLe == 0 && GiaTheoKH.TiLeSi == 0 && GiaTheoKH.GiaBanSi == 0 && GiaTheoKH.GiaBanLe == 0))
      //{
      //  //Xét tiếp tới giá theo nhóm hàng hoá
      //  var listGTNHH = context.GiaTheoNhomHangHoa.Where(x => x.Idnhh == hh.Idnhh).ToList();

      //  //Nếu giá theo nhóm hàng hoá có tồn tại
      //  if (listGTNHH.Count > 0)
      //  {
      //    //xét nhiều khoản min max khác nhau
      //    foreach (var h in listGTNHH)
      //    {
      //      if (GiaHangTon.IdctpnNavigation.DonGia >= h.Min && GiaHangTon.IdctpnNavigation.DonGia <= h.Max)
      //      {
      //        if (kh.LoaiKh == true)
      //        {
      //          GiaBan = getTiLe(h.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
      //        }
      //        else // nếu loại khách hàng là sĩ
      //        {
      //          GiaBan = getTiLe(h.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
      //        }
      //        return Json(new
      //        {
      //          options = options,
      //          giaBan = GiaBan,
      //          slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //          hinhAnh = hh.Avatar
      //        });
      //      }
      //    }
      //    // nếu sau khi xét trong list Giá theo nhóm nhà hoá và vẫn kh return thì xét xuống giá theo hàng hoá
      //    if (kh.LoaiKh == true)
      //    {
      //      if (hh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
      //      {
      //        GiaBan = hh.GiaBanLe; // lấy giá bán
      //      }
      //      else // xét tỉ lẹ lẻ
      //      {
      //        GiaBan = getTiLe(hh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
      //      }
      //      return Json(new
      //      {
      //        options = options,
      //        giaBan = GiaBan,
      //        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //        hinhAnh = hh.Avatar
      //      });
      //    }
      //    else // nếu loại khách hàng là sĩ
      //    {
      //      if (hh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
      //      {
      //        GiaBan = hh.GiaBanSi;
      //      }
      //      else // xét tỉ lẹ lẻ
      //      {
      //        GiaBan = getTiLe(hh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
      //      }
      //      return Json(new
      //      {
      //        options = options,
      //        giaBan = GiaBan,
      //        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //        hinhAnh = hh.Avatar
      //      });
      //    }
      //  }
      //  else  // xét tiếp giá theo hàng hoá
      //  {
      //    //Nếu loại khách hàng là lẻ
      //    if (kh.LoaiKh == true)
      //    {
      //      if (hh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
      //      {
      //        GiaBan = hh.GiaBanLe; // lấy giá bán
      //      }
      //      else // xét tỉ lẹ lẻ
      //      {
      //        GiaBan = getTiLe(hh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
      //      }
      //      return Json(new
      //      {
      //        options = options,
      //        giaBan = GiaBan,
      //        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //        hinhAnh = hh.Avatar
      //      });
      //    }
      //    else // nếu loại khách hàng là sĩ
      //    {
      //      if (hh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
      //      {
      //        GiaBan = hh.GiaBanSi;
      //      }
      //      else // xét tỉ lẹ lẻ
      //      {
      //        GiaBan = getTiLe(hh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
      //      }
      //      return Json(new
      //      {
      //        options = options,
      //        giaBan = GiaBan,
      //        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //        hinhAnh = hh.Avatar
      //      });
      //    }
      //  }
      //}
      //else //Nếu giá theo khách hàng tồn tại
      //{
      //  //Nếu loại khách hàng là lẻ
      //  if (kh.LoaiKh == true)
      //  {
      //    if (GiaTheoKH.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
      //    {
      //      GiaBan = GiaTheoKH.GiaBanLe; // lấy giá bán
      //    }
      //    else // xét tỉ lẹ lẻ
      //    {
      //      GiaBan = getTiLe(GiaTheoKH.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
      //    }
      //    return Json(new
      //    {
      //      options = options,
      //      giaBan = GiaBan,
      //      slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //      hinhAnh = hh.Avatar
      //    });
      //  }
      //  else // nếu loại khách hàng là sĩ
      //  {
      //    if (GiaTheoKH.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
      //    {
      //      GiaBan = GiaTheoKH.GiaBanSi;
      //    }
      //    else // xét tỉ lẹ lẻ
      //    {
      //      GiaBan = getTiLe(GiaTheoKH.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
      //    }
      //    return Json(new
      //    {
      //      options = options,
      //      giaBan = GiaBan,
      //      slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
      //      hinhAnh = hh.Avatar
      //    });
      //  }
      //}
    }



    //////////////////////// huhu
    [HttpPost("/checkgia")]
    public JsonResult Checkgia(int idHH, double SL)
    {
      MedicalShopContext context = new MedicalShopContext();
      ////  TonKho tk = context.TonKho.OrderBy(x => x.NgayNhap).FirstOrDefault(x => x.Id == idHH);

      
      ////List<ChiTietPhieuNhap> tkk = context.ChiTietPhieuNhap.Where(x => x.Id == idHH).OrderBy(x => x.CreatedDate).ToList();
      
      //List<TonKho> tkk = context.TonKho.Where(x => x.Id == idHH).OrderBy(x => x.NgayNhap).ToList();
      List<TonKho> tkk = context.TonKho.Include(x => x.IdctpnNavigation).Where(x => x.IdctpnNavigation.Idhh == idHH).OrderBy(x => x.NgayNhap).ToList();


      int idCTPN = 0;
      double donGia = 0;
      double thanhTien = 0;
      int i = 1;
      double SLtemp = SL;
      foreach (TonKho tk in tkk)
      {
        idCTPN = (int)tk.Idctpn;
        ChiTietPhieuNhap ct = context.ChiTietPhieuNhap.FirstOrDefault(y => y.Id == idCTPN);
        //donGia = donGia + (double)ct.SalePrice;
      //  SLtemp = (int)(tk.SoLuong - SL);
       // thanhTien = thanhTien + SL * donGia;

        if (SLtemp <= tk.SoLuong)
        {
          donGia = (double)ct.SalePrice;
          //SLtemp = (int)(tk.SoLuong - SL);
          thanhTien = thanhTien + (SLtemp * donGia);
          break; 
         
        }
        else
        {
          donGia = (double)ct.SalePrice;
          thanhTien = thanhTien + ((double)tk.SoLuong * donGia);
          SLtemp = (double)(SL - tk.SoLuong);
          //break; 
        }     
      }
      return Json(
          new
          {
            donGia = thanhTien/SL,
            thanhTien = thanhTien
          });

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
                                              .OrderByDescending(x => x.Id)
                                              .ToList();
      return PartialView("TableLichSuXuat");
    }


    string getSoPhieu()
    {
      MedicalShopContext context = new MedicalShopContext();
      QuyDinhMa qd = context.QuyDinhMa.Find(2);
      //ID chi nhánh
      string cn = "01";

      DateTime d = DateTime.Now;
      string ngayThangNam = d.ToString("yyMMdd");
      string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam;
      var list = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'XuatKho'").FirstOrDefault();
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
