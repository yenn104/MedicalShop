using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class PhieuXuatKhoController : Controller
  {

    [Route("/PhieuXuatKho")]
    public IActionResult Index()
    {
      return View("PhieuXuatKho");
    }




  }
}
