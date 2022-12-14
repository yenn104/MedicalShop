using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class QuanLyController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    //[Authorize(Roles ="NV")]
    public IActionResult QuanLy()
    {
      return View();
    }


  }
}
