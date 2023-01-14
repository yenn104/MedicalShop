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


    //[Authorize(Roles = "NV")]
    //public IActionResult ViewSelector()
    //{
    //  ViewBag.TaiKhoan = TempData["TaiKhoan"];
    //  ViewBag.ChiNhanh = TempData["ChiNhanh"];
    //  return View();
    //}


    [HttpPost]
    
    public ActionResult LogIn(TaiKhoan account, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        MedicalShopContext context = new MedicalShopContext();
        returnUrl = (string)TempData["returnUrl"];
        string user = account.UserName;
        string pass = account.Password;
        var acc = context.TaiKhoan.FirstOrDefault(s => s.UserName.Equals(user) && s.Password.Equals(pass));
        if (acc != null)
        {
          PhanQuyen vaitro = context.PhanQuyen.FirstOrDefault(c => c.Idtk.Equals(acc.Id));
          if (acc.Staff == true)
          {
            NhanVien nv = context.NhanVien.FirstOrDefault(n => n.UserName.Equals(acc.UserName));
            var claims = new List<Claim>
              {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, "NV"),
                new Claim("VaiTro", vaitro.Idvt.ToString()),
                new Claim(nv.Id.ToString(), nv.TenNv),
                new Claim(acc.Staff.ToString(), "Nhân viên"),
                new Claim("ChiNhanh", vaitro.Idcn.ToString()),
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
                  new Claim("VaiTro", vaitro.Idvt.ToString()),
                  new Claim(kh.Id.ToString(), kh.TenKh),
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
    public IActionResult ThongTin(int id)
    {
      MedicalShopContext context = new MedicalShopContext();
      TaiKhoan acc = context.TaiKhoan.FirstOrDefault(s => s.Id.Equals(id));
      if (acc.Staff != null || acc.Staff == true)
      {
        NhanVien nv = context.NhanVien.FirstOrDefault(a => a.Id.Equals(id));
        ViewBag.User = nv;
      }
      else
      {
        KhachHang kh = context.KhachHang.FirstOrDefault(b => b.Id.Equals(id));
        ViewBag.User = kh;
      }    
      return View(acc);
    }


    [HttpPost]
    public ActionResult SignUp(TaiKhoan account)
    {
      MedicalShopContext context = new MedicalShopContext();
      if (ModelState.IsValid)
      {

      }
      return View();
    }



    [HttpPost("/change-pass")]
    public string ChangePassword(string passWord, string newPassWord)
    {
      MedicalShopContext context = new MedicalShopContext();
      TaiKhoan tk = context.TaiKhoan.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
      if (tk.Password != passWord)
      {
        return "<div class='invalid-feedback trangThai' style='display:block;'>Mật khẩu không đúng</div>";
      }
      else
      {
        tk.Password = newPassWord;
        context.TaiKhoan.Update(tk);
        context.SaveChanges();
        return "<div class='valid-feedback trangThai' style='display:block;'>Đổi mật khẩu thành công</div>";
      }
    }




  }
}
