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


    [Authorize(Roles = "NV")]
    public IActionResult ViewSelector(string returnUrl)
    {

      returnUrl = (string)TempData["returnUrl"];
      TempData["ReturnUrl"] = returnUrl;
      //  ViewBag.PhanQuyen = TempData["PhanQuyen"];
      // string tk = ViewBag.TaiKhoan;
      string user = User.Claims.ElementAt(0).Value;
      ViewBag.TaiKhoan = user;

      MedicalShopContext context = new MedicalShopContext();
      int idtk = context.TaiKhoan.FirstOrDefault(k => k.Active == true && k.UserName == user).Id;
      ViewBag.ChiNhanh = context.PhanQuyen.Where(aa => aa.Idtk.Equals(idtk) && aa.Active == true).Select(aa => aa.Idcn).Distinct().ToList();

      return View();
    }



    [HttpPost]
    [Authorize(Roles = "NV")]
    public async Task<IActionResult> SelectorAsync(PhanQuyen pq, string returnUrl)
    {
      returnUrl = (string)TempData["returnUrl"];

      var identity = new ClaimsIdentity(User.Identity);

      var vaiTroClaim = identity.FindFirst("VaiTro");
      var chiNhanhClaim = identity.FindFirst("ChiNhanh");

      if (vaiTroClaim != null && chiNhanhClaim != null)
      {
        identity.RemoveClaim(identity.FindFirst("VaiTro"));
        identity.RemoveClaim(identity.FindFirst("ChiNhanh"));
      }

      identity.AddClaim(new Claim("VaiTro", pq.Idvt.ToString()));
      identity.AddClaim(new Claim("ChiNhanh", pq.Idcn.ToString()));
     

      var claims = identity.Claims.ToList();

      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction("QuanLy", "QuanLy");
      }

    }



    [HttpPost("/loadQuyen")]
    [Authorize(Roles = "NV")]
    public IActionResult loadVaiTro(string tk, int idcn)
    {
      MedicalShopContext context = new MedicalShopContext();
      int idtk = context.TaiKhoan.FirstOrDefault(k => k.Active == true && k.UserName == tk).Id;

      if (idcn != 0)
      {
        var listvt = context.PhanQuyen.Where(a => a.Idcn == idcn && a.Idtk == idtk && a.Active == true).Select(j => j.Idvt).Distinct().ToList();
        ViewBag.ListPQCN = listvt;
      }
      else
      {
        var listvt = context.PhanQuyen.Where(a => a.Idtk.Equals(idtk) && a.Active == true).Select(j => j.Idvt).Distinct().ToList();
        ViewBag.ListPQCN = listvt;
      }
      return PartialView();
    }



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
          //PhanQuyen vaitro = context.PhanQuyen.FirstOrDefault(c => c.Idtk.Equals(acc.Id));

        //    var listpq = context.PhanQuyen.Where(aa => aa.Idtk.Equals(acc.Id) && aa.Active == true).Select(aa => aa.Id).Distinct().ToList();

         // var listcn = context.PhanQuyen.Where(k => k.Idtk.Equals(acc.Id) && k.Active == true).Select(k => k.Idcn).Distinct().ToList();

          TempData["TaiKhoan"] = user;
        //  TempData["PhanQuyen"] = listpq;
       //   TempData["ChiNhanh"] = listcn;


          if (acc.Staff == true)
          {
            NhanVien nv = context.NhanVien.FirstOrDefault(n => n.UserName.Equals(acc.UserName));
            PhanQuyen vaitro = context.PhanQuyen.FirstOrDefault(c => c.Idtk.Equals(acc.Id));

            var claims = new List<Claim>
              {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, "NV"),
                new Claim(nv.Id.ToString(), nv.TenNv),
                new Claim("VaiTro", vaitro.Idvt.ToString()),
                // new Claim("VaiTro", vaitro.Idvt.ToString()),
                //new Claim(acc.Staff.ToString(), "Nhân viên"),
                // new Claim("ChiNhanh", vaitro.Idcn.ToString()),
              };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
              //return Redirect(returnUrl);
              TempData["ReturnUrl"] = returnUrl;
              return RedirectToAction("ViewSelector");



            }
            else
            {
              return RedirectToAction("ViewSelector");
              //return RedirectToAction("Index", "Home");
            }
          }
          else
          {
            KhachHang kh = context.KhachHang.FirstOrDefault(s => s.UserName.Equals(acc.UserName));
            var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, account.UserName),
                  new Claim(ClaimTypes.Role, "KH"),
                  new Claim(kh.Id.ToString(), kh.TenKh),
                  //new Claim("VaiTro", vaitro.Idvt.ToString()),
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
    //  ViewBag.TaiKhoan = "";
     // ViewBag.ChiNhanh = "";
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
