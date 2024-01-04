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
    public class ChiNhanhController : Controller
    {
        private MedicalShopContext context = new MedicalShopContext();
        private string _maChucNang = "ChiNhanh";
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục chi nhánh";
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            List<ChiNhanh> listChiNhanh = context.ChiNhanh.Where(x => x.Active == true && (type == 1 ? true : x.Id == idcn)).ToList();
            return View("TableChiNhanh", listChiNhanh);
        }

        //hiển thị view insert
        public IActionResult ViewCreate()
        {
            ViewData["Title"] = "Thêm chi nhánh";
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            return View();
        }


        [HttpPost]
        //thêm hãng sản xuất
        public IActionResult Insert(ChiNhanh cn)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            //int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            cn.CreatedBy = idUser;
            cn.ModifiedBy = idUser;
            cn.CreatedDate = DateTime.Now;
            cn.ModifiedDate = DateTime.Now;
            cn.Active = true;
            context.ChiNhanh.Add(cn);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }

        public IActionResult Delete(int id)
        {
            ChiNhanh cn = context.ChiNhanh.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            cn.ModifiedBy = idUser;
            cn.ModifiedDate = DateTime.Now;
            cn.Active = false;
            context.ChiNhanh.Update(cn);
            context.SaveChanges();
            return RedirectToAction("Table");
        }

        public IActionResult ViewUpdate(int id)
        {
            ChiNhanh cn = context.ChiNhanh.Find(id);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            ViewData["Title"] = "Cập nhật chi nhánh";
            return View(cn);
        }

        [HttpPost]
        public IActionResult Update(ChiNhanh cnn)
        {
            ChiNhanh cn = context.ChiNhanh.Find(cnn.Id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            cn.MaCn = cnn.MaCn;
            cn.TenCn = cnn.TenCn;
            cn.Address = cnn.Address;
            cn.Phone = cnn.Phone;
            cn.Mail = cnn.Mail;
            cn.Note = cnn.Note;
            cn.ModifiedBy = idUser;
            cn.ModifiedDate = DateTime.Now;
            context.ChiNhanh.Update(cn);
            context.SaveChanges();
            TempData["ThongBao"] = "Thành công!";
            return RedirectToAction("table");
        }


        [HttpPost("/loadTableChiNhanh")]
        public IActionResult LoadTableChiNhanh(bool active)
        {
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            ViewBag.ChiNhanh = context.ChiNhanh
              .Where(x => x.Active == active && (type == 1 ? true : x.Id == idcn))
              .OrderBy(x => x.TenCn)
              .ToList();

            return PartialView();
        }

        [HttpPost("/restoreChiNhanh")] 
        public IActionResult Restore(int id)
        {
            ChiNhanh cn = context.ChiNhanh.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            cn.ModifiedBy = idUser;
            cn.ModifiedDate = DateTime.Now;
            cn.Active = true;
            context.ChiNhanh.Update(cn);
            context.SaveChanges();
            return RedirectToAction("Table");
        }

        public IActionResult Details(int id)
        {
            MedicalShopContext context = new MedicalShopContext();
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            ChiNhanh hh = context.ChiNhanh.FirstOrDefault(x => x.Id == id);
            return View(hh);
        }

        [HttpPost("/loadDetailChiNhanh")]
        public IActionResult LoadDetail(int id)
        {
            ViewData["Title"] = "Chi tiết chi nhánh";
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            ChiNhanh hh = context.ChiNhanh.FirstOrDefault(x => x.Id == id);
            return View(hh);
        }

    }
}
