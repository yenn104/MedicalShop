using DocumentFormat.OpenXml.InkML;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
    [Authorize(Roles = "NV")]
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
            .ThenInclude(x => x.IdnccNavigation)
            .Where(x => x.IdpnNavigation.ModifiedDate.Value.Date >= tuNgay.Date && x.IdpnNavigation.ModifiedDate.Value.Date <= denNgay.Date && x.Idhh == idHH).ToList();

            var ncc = data.GroupBy(x => x.IdpnNavigation.Idncc)
            .Select(x => new
            {
                label = x.Key,
                value = x.Min(x => x.GiaVon)
            })
            .ToList().Take(3);

            List<DateTime> dateTimes = new List<DateTime>();
            foreach (ChiTietPhieuNhap ct in data)
            {
                if (ncc.Any(x => x.label == ct.IdpnNavigation.Idncc))
                {
                    dateTimes.Add((DateTime)ct.ModifiedDate.Value.Date);
                }
            }
            dateTimes = dateTimes.Distinct().ToList();
            var listData = new List<dynamic>();
            foreach (var n in ncc)
            {
                List<dataNCC> dates = new List<dataNCC>();
                foreach (var date in dateTimes)
                {
                    dataNCC dataNCC = new dataNCC();
                    dataNCC.TenNCC = getTenNCC(n.label);
                    var a = data.FirstOrDefault(x => x.IdpnNavigation.Idncc == n.label && x.IdpnNavigation.CreatedDate.Value.Date == date.Date);
                    if (a != null)
                    {
                        dataNCC.Gia = (double)a.GiaVon;
                    }
                    dates.Add(dataNCC);
                }
                listData.Add(dates);
            }
            return new
            {
                labels = dateTimes,
                value = listData
            };
        }

        private class dataNCC
        {
            public string? TenNCC { get; set; }
            public double Gia { get; set; }
        }


        public string getTenNCC(int? idNCC)
        {
            return context.NhaCungCap.Find(idNCC).TenNcc;
        }
        [HttpGet("ViewBieuDoKHTT")]
        public IActionResult ViewBieuDoKHTT()
        {
            return View("BieuDoKHTT");
        }
        [HttpPost("/BieuDoKHTT")]
        public dynamic BieuDoKHTT(string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var data = context.PhieuXuat
                .Include(x => x.IdkhNavigation)
                .Where(x => x.CreatedDate.Value.Date >= tuNgay.Date && x.CreatedDate.Value.Date <= denNgay.Date)
                .GroupBy(x => x.IdkhNavigation.TenKh)
                .Select(x => new
                {
                    label = x.Key,
                    value = x.Count()
                })
                .ToList();
            return data.OrderBy(x => x.value).Take(3);
        }
    }
}
