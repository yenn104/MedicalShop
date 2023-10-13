using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  [Authorize(Roles = "NV")]
  public class ImportController : Controller
  {
    
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục nhập dữ liệu";
      return View("TableImport");
    }

    public IActionResult ViewCreate()
    {
      ViewData["Title"] = "Thêm hàng hoá";
      return View();
    }
  }
}
