using DocumentFormat.OpenXml.InkML;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace MedicalShop.Services
{
    public class CommonServices
    {
        private static IWebHostEnvironment _hostingEnvironment;
        public static IHttpContextAccessor _httpContextAccessor;
        private static MedicalShopContext _context = new MedicalShopContext();
        //private static MedicalShopContext _context;

        public static void Configure(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, MedicalShopContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public static string formatDateTimeShort(DateTime? date)
        {
            if (date != null)
            {
                return date.Value.ToString("dd-MM-yy HH:mm");
            }
            else
            {
                return null;
            }
        }

        public static string formatDay(DateTime? date)
        {
            if (date != null)
            {
                return date.Value.ToString("dd-MM-yy");
            }
            else
            {
                return null;
            }
        }

        public static string FormatMoney(float amount)
        {
            // Nếu giá trị là 0.00, trả về chuỗi "0", ngược lại sử dụng định dạng tiền tệ
            return amount == 0 ? "0" : amount.ToString("#,##0.00");
        }

        public static string FormatMoneyWithZero(float amount)
        {
            return amount.ToString("0,###");
        }

        public static string getTenChiNhanh(int? id)
        {
            return _context.ChiNhanh.Where(x => x.Id == id).Select(x => x.TenCn).FirstOrDefault();
        }

        public static string getTenNhanVien(int? id)
        {
            return _context.NhanVien.Where(x => x.Id == id).Select(x => x.TenNv).FirstOrDefault();
        }

        public static string getTenNhomHangHoa(int? id)
        {
            return _context.NhomHangHoa.Where(x => x.Id == id).Select(x => x.TenNhh).FirstOrDefault();
        }

        public static string getTenDonViTinh(int? id)
        {
            return _context.NhomHangHoa.Where(x => x.Id == id).Select(x => x.TenNhh).FirstOrDefault();
        }

        public static string getTenHangSanXuat(int? id)
        {
            return _context.Hsx.Where(x => x.Id == id).Select(x => x.TenHsx).FirstOrDefault();
        }

        public static string getTenNuocSanXuat(int? id)
        {
            return _context.Nsx.Where(x => x.Id == id).Select(x => x.TenNsx).FirstOrDefault();
        }
     

    }
}
