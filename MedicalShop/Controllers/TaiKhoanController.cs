using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
  public class TaiKhoanController : Controller
  {
    private MedicalShopContext context = new MedicalShopContext();

    public IActionResult Index()
    {
      return View();
    }

    //[Route("/Login")]
    public IActionResult LogIn(string returnUrl)
    {
      TempData["returnUrl"] = returnUrl;
      return View();
    }

    [HttpPost]
    
    public ActionResult LogIn(TaiKhoan account, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        returnUrl = (string)TempData["returnUrl"];
        string user = account.UserName;
        string pass = account.Password;
        var acc = context.TaiKhoan.FirstOrDefault(s => s.UserName.Equals(user) && s.Password.Equals(pass));
        if (acc != null)
        {
          if (acc.Staff == true)
          {
            NhanVien nv = context.NhanVien.FirstOrDefault(n => n.UserName.Equals(acc.UserName));
            var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, account.UserName),
                  new Claim(ClaimTypes.Role, "NV"),
                  new Claim("UserID", nv.Id.ToString()),
                  new Claim("Staff", acc.Staff.ToString()),
                  new Claim("NVV", nv.TenNv),
              };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
              return Redirect(returnUrl);
            }
            else
            {
              return RedirectToAction("Index", "Home");
            }
          }
          else
          {
            KhachHang kh = context.KhachHang.FirstOrDefault(s => s.UserName.Equals(acc.UserName));
            var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, account.UserName),
                  new Claim(ClaimTypes.Role, "KH"),
                  new Claim("UserID", kh.Id.ToString()),
                  new Claim("Staff", acc.Staff.ToString()),
                  new Claim("KHH", kh.TenKh),
              };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
              return Redirect(returnUrl);
            }
            else
            {
              return RedirectToAction("Index", "Home");
            }
          }
        }
        else
        {
          ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
        }
      }
      return View();
    }


    [Authorize]
    public ActionResult LogOut()
    {
      HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Login");
    }

    [Authorize]
    [Route("/User")]
    public IActionResult ThongTin(string username)
    {    
      TaiKhoan acc = context.TaiKhoan.FirstOrDefault(s => s.UserName.Equals(username));
      if (acc.Iduser != null && acc.Iduser != 0)
      {
        if (acc.Staff == true)
        {
          NhanVien nv = context.NhanVien.FirstOrDefault(n => n.Id.Equals(acc.Iduser));
          ViewBag.User = nv;
        }
        else
        {
          KhachHang kh = context.KhachHang.FirstOrDefault(s => s.Id.Equals(acc.Iduser));
          ViewBag.User = kh;
        }
      }    
      return View(acc);
    }


    [HttpPost]
    public ActionResult SignUp(TaiKhoan account)
    {
      if (ModelState.IsValid)
      {

      }
      return View();
    }
  }
}
