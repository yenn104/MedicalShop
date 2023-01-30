using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class TonKhoController : Controller
  {
    private readonly ILogger<TonKhoController> _logger;
    private readonly IWebHostEnvironment _webHostEnv;

    public TonKhoController(ILogger<TonKhoController> logger, IWebHostEnvironment webHostEnv)
    {
      _logger = logger;
      _webHostEnv = webHostEnv;
    }


    public IActionResult Table()
    {
      ViewData["Title"] = "Báo cáo tồn kho";
      return View("TableHangHoa");
    }

    public IActionResult LoadTonKho()
    {
      return View();
    }

    public IActionResult TonKhoList()
    {
      return View();
    }


  }
}
