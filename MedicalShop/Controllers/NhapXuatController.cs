using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class NhapXuatController : Controller
  {
    public IActionResult Table()
    {
      ViewData["Title"] = "Nhập - Xuất dữ liệu";
      return View("TableData");
    }

    public IActionResult Xuat()
    {
      ViewData["Title"] = "Xuất dữ liệu";
      return View("TableExport");
    }

    public IActionResult Nhap()
    {
      ViewData["Title"] = "Nhập dữ liệu";
      return View("TableImport");
    }


  }
}
