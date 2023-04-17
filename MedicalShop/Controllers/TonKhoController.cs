using AspNetCore.Reporting;
using MedicalShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalShop.Controllers
{
  public class TonKhoController : Controller
  {
    private readonly ILogger<TonKhoController> _logger;
    private readonly IWebHostEnvironment _webHostEnv;


    public TonKhoController(ILogger<TonKhoController> logger)
    {
      _logger = logger;
    }


    public IActionResult Table()
    {
      ViewData["Title"] = "Báo cáo tồn kho";
      return View("TableTonKho");
    }

    public IActionResult LoadTonKho()
    {
      return View();
    }


  }
}
