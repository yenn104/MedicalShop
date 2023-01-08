using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  [Authorize(Roles ="NV")]
  public class QuanLyController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();

    [Route("/QuanLy")]
    public IActionResult QuanLy()
    {
      
      return View();
    }

    public IActionResult HoSo(int id)
    {
      ViewBag.idUser = id;
      NhanVien nv = context.NhanVien.FirstOrDefault(a => a.Id.Equals(id));
      return View(nv);
    }


  }
}
