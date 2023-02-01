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

    public TonKho tonkho = new TonKho();

    public TonKhoController(ILogger<TonKhoController> logger, IWebHostEnvironment webHostEnv)
    {
      _logger = logger;
      _webHostEnv = webHostEnv;
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
      var dt = new DataTable();
      dt = tonkho.getTonKho();

      string mimeType = "";
      int extension = 1;
      var path = $"{_webHostEnv.WebRootPath}\\Reports\\rptTonKho.rdlc";
      Dictionary<string, string> paramaters = new Dictionary<string, string>();
      paramaters.Add("prm4", "MEDICAL SHOP");
      paramaters.Add("prm1", DateTime.Now.ToString("MM/yyyy"));
      paramaters.Add("prm2", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
      paramaters.Add("prm3", "THỐNG KÊ TỒN KHO");

      LocalReport localReport = new LocalReport(path);
      localReport.AddDataSource("dsTonKho", dt);

      var res = localReport.Execute(RenderType.Pdf, extension, paramaters, mimeType);

      return File(res.MainStream, "application/pdf");
    }




  }
}
