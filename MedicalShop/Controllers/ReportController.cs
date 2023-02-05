using AspNetCore.Reporting;
using MedicalShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class ReportController : Controller
  {

    private readonly IWebHostEnvironment _webHostEnv;


    public ReportController(IWebHostEnvironment webHostEnv)
    {
      _webHostEnv = webHostEnv;
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    public HHNList hhn = new HHNList();


    public IActionResult Index()
    {
      return View();
    }


    public IActionResult Report()
    {
      var dt = new DataTable();
      dt = hhn.getHangHoa();

      string mimeType = "";
      int extension = 1;
      var path = $"{_webHostEnv.WebRootPath}\\Reports\\rptTonKho.rdlc";
      Dictionary<string, string> paramaters = new Dictionary<string, string>();
      paramaters.Add("prm4", "MEDICAL SHOP");
      paramaters.Add("prm1", DateTime.Now.ToString("MM/yyyy"));
      paramaters.Add("prm2", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
      paramaters.Add("prm3", "THỐNG KÊ HÀNG HÓA ĐÃ NHẬP");

      LocalReport localReport = new LocalReport(path);
      localReport.AddDataSource("dsTonKho", dt);

      var res = localReport.Execute(RenderType.Pdf, extension, paramaters, mimeType);

      return File(res.MainStream, "application/pdf");
    }

  }
}
