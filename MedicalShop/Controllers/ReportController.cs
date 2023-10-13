using AspNetCore.Reporting;
using MedicalShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  [Authorize(Roles = "NV")]
  public class ReportController : Controller
  {

    private readonly IWebHostEnvironment _webHostEnv;


    public ReportController(IWebHostEnvironment webHostEnv)
    {
      _webHostEnv = webHostEnv;
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    public Report report = new Report();


    public IActionResult Index()
    {
      return View();
    }


    public IActionResult ReportTon()
    {
      var dt = new DataTable();
      //Report reportt = new Report();
    dt = report.getTonKho();

      string mimeType = "";
      int extension = 1;
      var path = $"{_webHostEnv.WebRootPath}\\Reports\\rptTonKho.rdlc";
      Dictionary<string, string> paramaters = new Dictionary<string, string>();
      paramaters.Add("prm4", "MEDICAL SHOP");
      paramaters.Add("prm1", DateTime.Now.ToString("MM-yyyy"));
      paramaters.Add("prm2", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
      paramaters.Add("prm3", "THỐNG KÊ TỒN KHO");

      LocalReport localReport = new LocalReport(path);
      localReport.AddDataSource("dsTonKho", dt);

      var res = localReport.Execute(RenderType.Pdf, extension, paramaters, mimeType);

      return File(res.MainStream, "application/pdf");
    }



    public IActionResult ReportHHN()
    {
      var dt = new DataTable();
      dt = report.getHangHoaNhap();

      string mimeType = "";
      int extension = 1;
      var path = $"{_webHostEnv.WebRootPath}\\Reports\\rptHHN.rdlc";
      Dictionary<string, string> paramaters = new Dictionary<string, string>();
      paramaters.Add("prm1", "MEDICAL SHOP");
      //paramaters.Add("prm1", DateTime.Now.ToString("MM-yyyy"));
      paramaters.Add("prm3", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
      paramaters.Add("prm2", "THỐNG KÊ HÀNG HÓA ĐÃ NHẬP");

      LocalReport localReport = new LocalReport(path);
      localReport.AddDataSource("dsHHN", dt);

      var res = localReport.Execute(RenderType.Pdf, extension, paramaters, mimeType);

      return File(res.MainStream, "application/pdf");
    }



    public IActionResult ReportHHX()
    {
      var dt = new DataTable();
      dt = report.getHangHoaXuat();

      string mimeType = "";
      int extension = 1;
      var path = $"{_webHostEnv.WebRootPath}\\Reports\\rptHHX.rdlc";
      Dictionary<string, string> paramaters = new Dictionary<string, string>();
      paramaters.Add("prm1", "MEDICAL SHOP");
      //paramaters.Add("prm1", DateTime.Now.ToString("MM-yyyy"));
      paramaters.Add("prm3", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
      paramaters.Add("prm2", "THỐNG KÊ HÀNG HÓA ĐÃ XUẤT");

      LocalReport localReport = new LocalReport(path);
      localReport.AddDataSource("dsHHX", dt);

      var res = localReport.Execute(RenderType.Pdf, extension, paramaters, mimeType);

      return File(res.MainStream, "application/pdf");
    }





  }
}
