using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using DocumentFormat.OpenXml.InkML;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public static ChucNang getVaiTroPhanQuyen(int? idVaiTro, string maMenu)
        {
            int idMenu = _context.Menu.Where(x => x.MaMenu == maMenu && x.Active == true).FirstOrDefault().Id;
            
            if (idVaiTro > 0 && idMenu > 0)
            {
                return _context.ChucNang
                    .Where(c => c.Idvt.Equals(idVaiTro) && c.Idmenu.Equals(idMenu))
                    .AsEnumerable()
                    .Select(x => new ChucNang
                    {
                        Import = x.Import ?? false,
                        Update = x.Update ?? false,
                        Delete = x.Delete ?? false,
                        Print = x.Print ?? false,
                        Export = x.Export ?? false
                    })
                    .FirstOrDefault();
            } 
            else
            {
                return new ChucNang()
                {
                    Import = false,
                    Update = false,
                    Delete = false,
                    Print = false,
                    Export = false
                };
            }
        }


        public static string getSoPhieuNhap(int? idChiNhanh, MedicalShopContext context)
        {
            QuyDinhMa qd = context.QuyDinhMa.Find(1);
            //ID chi nhánh
            //string cn = User.Claims.ElementAt(4).Value;

            DateTime d = DateTime.Now;
            string ngayThangNam = d.ToString("yyMMdd");
            string SoPhieu = idChiNhanh + "-" + qd.TiepDauNgu + ngayThangNam;
            var list = context.SoThuTu.FromSqlRaw("select * from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,date) and type = 'NhapKho'").FirstOrDefault();
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

        public static HhGia getGia(int? id)
        {
            HhGia giahh = _context.HhGia.FirstOrDefault(x => x.Idhh == id);
            return giahh != null ? giahh : new HhGia();
            
        }

        public static string getSoPhieuXuat(int? idChiNhanh, MedicalShopContext context)
        {

            QuyDinhMa qd = context.QuyDinhMa.Find(1);
            //ID chi nhánh
            //string cn = User.Claims.ElementAt(4).Value;

            DateTime d = DateTime.Now;
            string ngayThangNam = d.ToString("yyMMdd");
            string SoPhieu = idChiNhanh + "-" + qd.TiepDauNgu + ngayThangNam;
            var list = context.SoThuTu.FromSqlRaw("select * from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,Date) and Type = 'XuatKho'").FirstOrDefault();
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


    }
}
