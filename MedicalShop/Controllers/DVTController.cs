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
    public class DVTController : Controller
    {
        private MedicalShopContext context = new MedicalShopContext();
        private string _maChucNang = "DVT";
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục đơn vị tính";
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            List<Dvt> listDVT = context.Dvt.Where(x => x.Active == true && (type == 1 ? true : x.Idcn == idcn)).ToList();
            return View("TableDVT", listDVT);
        }


        public IActionResult ViewCreate()
        {
            ViewData["Title"] = "Thêm đơn vị tính";
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            ViewBag.Quyen = CommonServices.getVaiTroPhanQuyen(idvt, _maChucNang);
            return View();
        }


        [HttpPost("/loadTableDVT")]
        public IActionResult loadTableDVT(bool active)
        {
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            ViewBag.DVT = context.Dvt
              .Where(x => (active == false ? true : x.Active == true) && (type == 1 ? true : x.Idcn == idcn))
              .OrderBy(x => x.TenDvt)
              .ToList();
            return PartialView();
        }

        //Trả về view sửa đơn vị tính
        //[Route("/DonViTinh/ViewUpdateDVT/{id}")]
        public IActionResult ViewUpdate(int id)
        {
            ViewData["Title"] = "Cập nhật đơn vị tính";
            Dvt dvt = context.Dvt.Find(id);
            return View(dvt);
        }

        //insert đơn vị tính
        [HttpPost]
        public IActionResult insertDVT(Dvt dvt)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            dvt.CreatedBy = idUser;
            dvt.ModifiedBy = idUser;
            dvt.Idcn = idcn;
            dvt.Active = true;
            dvt.CreatedDate = DateTime.Now;
            dvt.ModifiedDate = DateTime.Now;
            context.Dvt.Add(dvt);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }

        //update đơn vị tính
        [HttpPost]
        public IActionResult updateDVt(Dvt dvt)
        {
            Dvt dv = context.Dvt.Find(dvt.Id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dv.ModifiedBy = idUser;
            dv.ModifiedDate = DateTime.Now;
            dv.TenDvt = dvt.TenDvt;
            dv.MaDvt = dvt.MaDvt;

            context.Dvt.Update(dv);
            context.SaveChanges();
            TempData["ThongBao"] = "Cập nhật thành công!";
            return RedirectToAction("Table");
        }

        //Xoá đơn vị tính
        //[Route("/DonViTinh/xoa/{id}")]
        public IActionResult Delete(int id)
        {
            Dvt dvt = context.Dvt.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvt.ModifiedBy = idUser;
            dvt.ModifiedDate = DateTime.Now;
            dvt.Active = false;
            context.Dvt.Update(dvt);
            context.SaveChanges();
            return RedirectToAction("Table");
        }
        //[Route("/DonViTinh/khoiphuc/{id}")]
        public IActionResult Restore(int id)
        {
            Dvt dvt = context.Dvt.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvt.ModifiedBy = idUser;
            dvt.ModifiedDate = DateTime.Now;
            dvt.Active = true;
            context.Dvt.Update(dvt);
            context.SaveChanges();
            //TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }


    }
}
