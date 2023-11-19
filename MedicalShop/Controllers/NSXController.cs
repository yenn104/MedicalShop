using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
    [Authorize(Roles = "NV")]
    public class NSXController : Controller
    {
        private MedicalShopContext _context = new MedicalShopContext();

        //[Route("/NuocSanXuat")]
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục nước sản xuất";
            TempData["Menu"] = _context.Menu.Where(x => x.MaMenu == "NSX" && x.Active == true).FirstOrDefault().Id;
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = _context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            List<Nsx> listNSX = _context.Nsx.Where(x => x.Active == true && (type == true ? true : x.Idcn == idcn)).ToList();
            return View("TableNSX", listNSX);
        }

        //hiển thị view thêm nhà sản xuất
        public IActionResult ViewCreate()
        {
            ViewData["Title"] = "Thêm danh mục nước sản xuất";
            return View();
        }

        // thêm nhà sản xuất
        [HttpPost]
        public IActionResult Insert(Nsx nsx)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            nsx.CreatedBy = idUser;
            nsx.ModifiedBy = idUser;
            nsx.Idcn = idcn;
            nsx.CreatedDate = DateTime.Now;
            nsx.ModifiedDate = DateTime.Now;
            nsx.Active = true;
            _context.Add(nsx);
            _context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");

        }

        //xóa nhà sản xuất
        //[Route("/NuocSanXuat/delete/{id}")]
        public IActionResult Delete(int id)
        {
            Nsx nsx = _context.Nsx.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            nsx.ModifiedBy = idUser;
            nsx.ModifiedDate = DateTime.Now;
            nsx.Active = false;
            _context.Nsx.Update(nsx);
            _context.SaveChanges();
            TempData["ThongBao"] = "Thành công!";

            return RedirectToAction("Table");
        }

        // dẫn tới view update
        // [Route("/NuocSanXuat/Update/{id}")]
        public IActionResult ViewUpdate(int id)

        {
            ViewData["Title"] = "Sửa nước sản xuất";
            Nsx nsx = _context.Nsx.Find(id);
            return View(nsx);
        }

        //update nhà sản xuất
        public IActionResult Update(Nsx nsx)
        {
            Nsx n = _context.Nsx.Find(nsx.Id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            n.ModifiedBy = idUser;
            n.ModifiedDate = DateTime.Now;
            n.MaNsx = nsx.MaNsx;
            n.TenNsx = nsx.TenNsx;
            _context.Nsx.Update(n);
            _context.SaveChanges();
            TempData["ThongBao"] = "Cập nhật thành công!";
            return RedirectToAction("Table");
        }

        [HttpPost("/loadTableNSX")]
        public IActionResult loadTableNSX(bool active)
        {
            if (active)
            {
                ViewBag.NSX = _context.Nsx.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.NSX = _context.Nsx.ToList();
            }
            return PartialView();
        }

        //[Route("/NuocSanXuat/khoiphuc/{id}")]
        public IActionResult Restore(int id)
        {
            Nsx n = _context.Nsx.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            n.ModifiedBy = idUser;
            n.ModifiedDate = DateTime.Now;
            n.Active = true;

            _context.Nsx.Update(n);
            _context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }


        [HttpPost("/loadDetailHSX")]
        public IActionResult LoadDetail(int id)
        {
            ViewData["Title"] = "Chi tiết hãng sản xuất";
            Hsx h = _context.Hsx.Find(id);
            return View(h);
        }

    }
}
