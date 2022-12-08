using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }

    [Authorize]
    public IActionResult About()
    {
      return View();
    }

    public IActionResult Login()
    {
      return View();
    }

    public IActionResult News()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(TaiKhoan account, string returnUrl)
    {
      MedicalShopContext context = new MedicalShopContext();
      //if (ModelState.IsValid)
      //{
      string user = account.UserName;
      string pass = account.Password;
      var acc = context.TaiKhoan.FirstOrDefault(s => s.UserName.Equals(user) && s.Password.Equals(pass));
      if (acc != null)
      {
        if (acc.Roles == true)
        {
          //FormsAuthentication.SetAuthCookie(acc.UserName, false);
          if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
          {
            return Redirect(returnUrl);
          }
          else
          {
            //FormsAuthentication.SetAuthCookie(account.UserName, true);
            //Session["TaiKhoan"] = acc;
            return RedirectToAction("Index", "Home");
          }
        }
        else
        {
          if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
          {
            return Redirect(returnUrl);
          }
          else
          {
            //FormsAuthentication.SetAuthCookie(account.Username, true);
            //Session["TaiKhoan"] = acc;
            //return RedirectToAction("Index", "Home");
          }
          //return RedirectToAction("Index", "Home");
        }
      }
      //else
      //{
      //  ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
      //}
      //}
      return View();
    }

    //public IActionResult Privacy()
    //{
    //  return View();
    //}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
