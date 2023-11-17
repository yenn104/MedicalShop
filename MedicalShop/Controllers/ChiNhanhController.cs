using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
    public class ChiNhanhController : Controller
    {
        private MedicalShopContext context = new MedicalShopContext();
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục chi nhánh";
            TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "ChiNhanh" && x.Active == true).FirstOrDefault().Id;

            int idcn = int.Parse(User.Claims.ElementAt(4).Value);

            int idvt = int.Parse(User.Claims.ElementAt(3).Value);

            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;

            List<ChiNhanh> listChiNhanh = context.ChiNhanh.Where(x => x.Active == true && (type == true ? true : x.Id == idcn)).ToList();

            return View("TableChiNhanh", listChiNhanh);
        }

        //hiển thị view insert
        public IActionResult ViewCreate()
        {
            ViewData["Title"] = "Thêm chi nhánh";
            return View();
        }


        [HttpPost]
        //thêm hãng sản xuất
        public IActionResult Insert(ChiNhanh cn)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
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
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("table");
        }


        [HttpPost("/loadTableChiNhanh")]
        public IActionResult LoadTableChiNhanh(bool active)
        {
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;

            ViewBag.ChiNhanh = context.ChiNhanh
              .Where(x => (active == false ? true : x.Active == true) && (type == true ? true : x.Id == idcn))
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
            ChiNhanh hh = context.ChiNhanh.FirstOrDefault(x => x.Id == id);
            return View(hh);
        }

        [HttpPost("/loadDetailChiNhanh")]
        public IActionResult LoadDetail(int id)
        {
            ViewData["Title"] = "Chi tiết chi nhánh";
            ChiNhanh hh = context.ChiNhanh.FirstOrDefault(x => x.Id == id);
            return View(hh);
        }

    }
}
