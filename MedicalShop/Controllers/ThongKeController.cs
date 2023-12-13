using DocumentFormat.OpenXml.InkML;
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
    public class ThongKeController : Controller
    {
        private MedicalShopContext context = new MedicalShopContext();

        public IActionResult ThongKe()
        {
            return View();
        }
        [HttpPost("/baoCaoLoiNhuan")]
        public async Task<dynamic> baoCaoLoiNhuan(string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //var doanhThu = await context.HoaDons
            //  .Where(x => x.Tgxuat.Value.Date >= tuNgay.Date && x.Tgxuat.Value.Date <= denNgay.Date)
            //  .OrderBy(x => x.Tgxuat)
            //  .GroupBy(x => x.Tgxuat.Value.Date)
            //  .Select(x => new
            //  {
            //      label = x.Key,
            //      doanhthu = x.Sum(x => x.TongTien)
            //  })
            //  .ToListAsync();
            var doanhThu = context.PhieuXuat
              .Include(x => x.ChiTietPhieuXuat)
              .ThenInclude(x => x.IdctpnNavigation)
              .Where(x => x.CreatedDate.Value.Date >= tuNgay.Date && x.CreatedDate.Value.Date <= denNgay.Date)
              .ToList()
              .OrderBy(x => x.CreatedDate)
              .GroupBy(x => x.CreatedDate.Value.Date)
              .Select(x => new
              {
                  label = x.Key,
                  doanhthu = tongPhieuNhap(x.ToList()),
              })
              .ToList();
            var giaVon = context.PhieuXuat
              .Include(x => x.ChiTietPhieuXuat)
              .ThenInclude(x => x.IdctpnNavigation)
              .Where(x => x.CreatedDate.Value.Date >= tuNgay.Date && x.CreatedDate.Value.Date <= denNgay.Date)
              .ToList()
              .OrderBy(x => x.CreatedDate)
              .GroupBy(x => x.CreatedDate.Value.Date)
              .Select(x => new
              {
                  label = x.Key,
                  doanhthu = GiaVon(x.ToList()),
              })
              .ToList();
            var listLoiNhuan = new List<dynamic>();
            foreach (var d in doanhThu)
            {
                var data = new
                {
                    Ngay = d.label,
                    DoanhThu = (double)d.doanhthu,
                    GiaVon = (double)0,
                };
                listLoiNhuan.Add(data);
            }
            foreach (var d in giaVon)
            {
                var data = new
                {
                    Ngay = d.label,
                    DoanhThu = (double)0,
                    GiaVon = (double)d.doanhthu,
                };
                listLoiNhuan.Add(data);
            }
            List<LoiNhuan> newlist = listLoiNhuan.GroupBy(x => x.Ngay)
            .Select(x => new LoiNhuan()
            {
                Ngay = x.Key,
                DoanhThu = (double)x.Sum(item => (double)item.DoanhThu),
                GiaVon = (double)x.Sum(item => (double)item.GiaVon)
            }).ToList();
            return new
            {
                doanhThu = doanhThu,
                giaVon = giaVon,
                listLoiNhuan = newlist,
            };
        }
        public class LoiNhuan
        {
            public DateTime? Ngay { get; set; }
            public double? DoanhThu { get; set; }
            public double? GiaVon { get; set; }
        }
        public double tongPhieuNhap(List<PhieuXuat> phieuNhaps)
        {
            double tong = (double)phieuNhaps.Sum(x => x.ChiTietPhieuXuat.Sum(ct => ct.Price));
            return tong;
        }
        public double GiaVon(List<PhieuXuat> phieuNhaps)
        {
            double tong = (double)phieuNhaps.Sum(x => x.ChiTietPhieuXuat.Sum(ct => ct.IdctpnNavigation.Price ?? 0));
            return tong;
        }
        public double tongChiTietPhieuNhap(List<ChiTietPhieuNhap> chiTietPhieuNhaps)
        {
            double tong = (double)chiTietPhieuNhaps.Sum(ct => ct.Price);
            return tong;
        }
        [HttpGet("ViewBieuDoGiaNhap")]
        public IActionResult ViewBieuDoGiaNhap()
        {
            return View("BieuDoGiaNhap");
        }
        //viết đây nè nha
        [HttpPost("BieuDoGiaNhap")]
        public dynamic BieuDoGiaNhap(int idHH, string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var data = context.ChiTietPhieuNhap
    .Include(x => x.IdpnNavigation)
    .Where(x => x.IdpnNavigation.ModifiedDate.Value.Date >= tuNgay.Date && x.IdpnNavigation.ModifiedDate.Value.Date <= denNgay.Date && x.Idhh == idHH)
    .GroupBy(x => new { x.IdpnNavigation.ModifiedDate, x.IdpnNavigation.Idncc })
    .Select(x => new
    {
        label = x.Key.ModifiedDate,
        ncc = x.Key.Idncc,
        gia = x.Min(y => y.GiaVon)
    })
    .ToList()
    .GroupBy(x => x.label)
    .Select(x => new
    {
        label = x.Key,
        nccs = x.OrderBy(y => y.gia).Take(3).Select(y => new { y.ncc, y.gia })
    })
    .ToList();
            var a = new
            {
                label = new List<DateTime>(),
                nccs =  new List<object>(),
            };
            foreach (var b in data)
            {
                a.label.Add((DateTime)b.label);
                a.nccs.Add(b.nccs);
            }

            return a;

        }
        public string getTenNCC(int? idNCC)
        {
            return context.NhaCungCap.Find(idNCC).TenNcc;
        }
    }
}
