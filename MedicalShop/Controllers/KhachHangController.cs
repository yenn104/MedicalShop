using MedicalShop.Models.Entities;
using MedicalShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
    [Authorize(Roles = "NV")]
    public class KhachHangController : Controller
    {
        private MedicalShopContext context = new MedicalShopContext();
        private string _maChucNang = "KhachHang";

        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục khách hàng";
            TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "KhachHang" && x.Active == true).FirstOrDefault().Id;
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            List<KhachHang> listKH = context.KhachHang.Where(x => x.Active == true && (type == 1 ? true : x.Idcn == idcn)).ToList();
            //TempData["Menu"] = context.Menu.Where( menu => EF.Functions.Like( menu.TenMenu, "%Nhà cung cấp%") && menu.Active == true).Select(menu => menu.Id);
            // EF.Functions.Like(c.Name, "a%")     menu.TenMenu.Contains("/Nhà cung cấp/")
            return View("TableKH", listKH);
        }

        public IActionResult Details(int id)
        {
            ViewData["Title"] = "Chi tiết khách hàng";
            KhachHang kh = context.KhachHang.FirstOrDefault(x => x.Id == id);
            return View(kh);
        }



        //hiển thị view insert
        public IActionResult ViewCreate()
        {
            ViewData["Title"] = "Thêm khách hàng";
            return View();
        }

        //thêm nhà cung cấp
        public IActionResult Insert(KhachHang kh)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            kh.CreatedBy = idUser;
            kh.ModifiedBy = idUser;
            kh.Idcn = idcn;
            kh.CreatedDate = DateTime.Now;
            kh.ModifiedDate = DateTime.Now;
            kh.Active = true;
            context.KhachHang.Add(kh);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }

        //HIển thị view update
        // [Route("/KhachHang/ViewUpdate/{id}")]
        public IActionResult ViewUpdate(int id)

        {
            KhachHang kh = context.KhachHang.Find(id);
            ViewData["Title"] = "Cập nhật khách hàng";
            return View(kh);
        }

        public IActionResult Update(KhachHang kh)
        {
            KhachHang n = context.KhachHang.Find(kh.Id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            n.ModifiedBy = idUser;
            n.ModifiedDate = DateTime.Now;
            n.MaKh = kh.MaKh;
            n.TenKh = kh.TenKh;
            n.Address = kh.Address;
            n.Phone = kh.Phone;
            n.Mail = kh.Mail;
            n.Note = kh.Note;
            context.KhachHang.Update(n);
            context.SaveChanges();
            TempData["ThongBao"] = "Cập nhật thành công!";
            return RedirectToAction("Table");
        }

        //xóa nhà cung cấp
        //[Route("/KhachHang/Delete/{id}")] huhu 
        public IActionResult Delete(int id)
        {
            KhachHang kh = context.KhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            kh.Active = false;
            kh.ModifiedBy = idUser;
            kh.ModifiedDate = DateTime.Now;
            context.KhachHang.Update(kh);
            context.SaveChanges();
            return RedirectToAction("Table");
        }

        [HttpPost("/loadTableKH")]
        public IActionResult loadTableKH(bool active)
        {
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;

            ViewBag.KH = context.KhachHang
              .Where(x => (active == false ? true : x.Active == true) && (type == 1 ? true : x.Idcn == idcn))
              .OrderBy(x => x.TenKh)
              .ToList();
            return PartialView();
        }

        //[Route("/KhachHang/khoiphuc/{id}")]
        public IActionResult Restore(int id)
        {
            KhachHang kh = context.KhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            kh.Active = true;
            kh.ModifiedBy = idUser;
            kh.ModifiedDate = DateTime.Now;
            context.KhachHang.Update(kh);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }

        [HttpPost("/restoreKH")]
        public string Restoree(int id)
        {
            KhachHang kh = context.KhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            kh.Active = true;
            kh.ModifiedBy = idUser;
            kh.ModifiedDate = DateTime.Now;
            context.KhachHang.Update(kh);
            context.SaveChanges();
            return "Khôi phục thành công!";
        }

        [HttpPost("/loadDetailKH")]
        public IActionResult LoadDetailKH(int id)
        {
            ViewData["Title"] = "Chi tiết khách hàng";
            KhachHang kh = context.KhachHang.FirstOrDefault(x => x.Id == id);
            return View(kh);
        }
    }
}
