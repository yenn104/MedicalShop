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
    public class DVVCController : Controller
    {
        private MedicalShopContext context = new MedicalShopContext();
        private string _maChucNang = "DVVC";

        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục dịch vụ vận chuyển";
            //TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "DVVC" && x.Active == true).FirstOrDefault().Id;
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            return View("TableDVVC");
        }

        public IActionResult Details(int id)
        {
            ViewData["Title"] = "Chi tiết dịch vụ vận chuyển";
            Dvvc dvvc = context.Dvvc.FirstOrDefault(x => x.Id == id);
            return View(dvvc);
        }

        //hiển thị view insert
        public IActionResult ViewCreate()
        {
            ViewData["Title"] = "Thêm dịch vụ vận chuyển";
            return View();
        }

        //thêm
        public IActionResult Insert(Dvvc dvvc)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            dvvc.CreatedBy = idUser;
            dvvc.ModifiedBy = idUser;
            dvvc.Idcn = idcn;
            dvvc.CreatedDate = DateTime.Now;
            dvvc.ModifiedDate = DateTime.Now;
            dvvc.Active = true;
            context.Dvvc.Add(dvvc);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }

        //HIển thị view update
        // [Route("/NhaCungCap/ViewUpdate/{id}")]
        public IActionResult ViewUpdate(int id)

        {
            Dvvc dvvc = context.Dvvc.Find(id);
            ViewData["Title"] = "Cập nhật dịch vụ vận chuyển";
            return View(dvvc);
        }

        public IActionResult Update(Dvvc dvvc)
        {
            Dvvc n = context.Dvvc.Find(dvvc.Id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            n.ModifiedBy = idUser;
            n.ModifiedDate = DateTime.Now;
            n.MaDvvc = dvvc.MaDvvc;
            n.TenDvvc = dvvc.TenDvvc;
            n.Note = dvvc.Note;
            context.Dvvc.Update(n);
            context.SaveChanges();
            TempData["ThongBao"] = "Cập nhật thành công!";
            return RedirectToAction("Table");
        }

        //xóa nhà cung cấp
        //[Route("/NhaCungCap/Delete/{id}")] huhu 
        public IActionResult Delete(int id)
        {
            Dvvc dvvc = context.Dvvc.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvvc.Active = false;
            dvvc.ModifiedBy = idUser;
            dvvc.ModifiedDate = DateTime.Now;
            context.Dvvc.Update(dvvc);
            context.SaveChanges();
            return RedirectToAction("Table");
        }

        [HttpPost("/loadTableDVVC")]
        public IActionResult loadTableDVVC(bool active)
        {
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.DVVC = context.Dvvc
              .Where(x => (active == false ? true : x.Active == true) && (type == 1 ? true : x.Idcn == idcn))
              .OrderBy(x => x.TenDvvc)
              .ToList();
            return PartialView();
        }

        //[Route("/NhaCungCap/khoiphuc/{id}")]
        public IActionResult Restore(int id)
        {
            Dvvc dvvc = context.Dvvc.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvvc.Active = true;
            dvvc.ModifiedBy = idUser;
            dvvc.ModifiedDate = DateTime.Now;
            context.Dvvc.Update(dvvc);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }

        [HttpPost("/restoreDVVC")]
        public string Restoree(int id)
        {
            Dvvc dvvc = context.Dvvc.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvvc.Active = true;
            dvvc.ModifiedBy = idUser;
            dvvc.ModifiedDate = DateTime.Now;
            context.Dvvc.Update(dvvc);
            context.SaveChanges();
            return "Khôi phục thành công!";
        }

        [HttpPost("/loadDetailDVVC")]
        public IActionResult LoadDetail(int id)
        {
            ViewData["Title"] = "Chi tiết dịch vụ vận chuyển";
            Dvvc dvvc = context.Dvvc.FirstOrDefault(x => x.Id == id);
            return View(dvvc);
        }
    }
}
