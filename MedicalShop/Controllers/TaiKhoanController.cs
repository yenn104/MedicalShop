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

    public IActionResult Login(string returnUrl)
    {
      TempData["returnUrl"] = returnUrl;
      return View();
    }

    [HttpPost]
    //[Route("/Login")]
    public ActionResult Login(TaiKhoan account, string returnUrl)
    {     
      if (ModelState.IsValid)
      {
        returnUrl = (string)TempData["returnUrl"];
        string user = account.UserName;
        string pass = account.Password;
        var acc = context.TaiKhoan.FirstOrDefault(s => s.UserName.Equals(user) && s.Password.Equals(pass));
        if (acc != null)
        {
          if (acc.Roles == true)
          {
            NhanVien nv = context.NhanVien.FirstOrDefault(n => n.Id.Equals(acc.Idnv));
            var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, account.UserName),
                  new Claim(ClaimTypes.Role, "NV"),
                  new Claim("IDNV", account.Idnv.ToString()),
                  new Claim("NV", nv.TenNv),
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
            KhachHang kh = context.KhachHang.FirstOrDefault(s => s.Id.Equals(acc.Idkh));
            var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, account.UserName),
                  new Claim(ClaimTypes.Role, "KH"),
                  new Claim("IDKH", account.Idkh.ToString()),
                  new Claim("KH", kh.TenKh),
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
    public ActionResult Logout()
    {
      HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Login");
    }

    [Authorize]
    [Route("/User")]
    public ActionResult ThongTin(string username)
    {
      TaiKhoan acc = context.TaiKhoan.FirstOrDefault(s => s.UserName.Equals(username));
      if(acc.Idkh != null && acc.Idkh != 0)
      {
        KhachHang kh = context.KhachHang.FirstOrDefault(s => s.Id.Equals(acc.Idkh));
        ViewBag.User = kh;
      }
      if (acc.Idkh != null && acc.Idkh != 0)
      {
        NhanVien nv = context.NhanVien.FirstOrDefault(n => n.Id.Equals(acc.Idnv));
        ViewBag.User = nv;
      }
      return View(acc);
    }




  }
}
