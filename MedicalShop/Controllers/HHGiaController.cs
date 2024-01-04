using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using MedicalShop.Models;
using MedicalShop.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace MedicalShop.Controllers
{
    [Authorize(Roles = "NV")]

    public class HHGiaController : Controller
    {
        public IActionResult Table()
        {
            ViewData["Title"] = "Áp đặt giá bán";
            MedicalShopContext context = new MedicalShopContext();
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            List<ChiNhanh> listChiNhanh = context.ChiNhanh
            .Where(x => x.Active == true && (type == 1 ? true : x.Id == idcn))
            .ToList();
            return View("TableHHGia", listChiNhanh);
        }



        [HttpPost("/addRowGia")]
        public IActionResult addRowGia(int idHH)
        {
            MedicalShopContext context = new MedicalShopContext();
            if (idHH == 0)
            {
                return PartialView();
            }
            else
            {
                ViewBag.HangHoa = context.HangHoa.FirstOrDefault(x => x.Active == true && x.Id == idHH);
                return PartialView();
            }
        }



        [HttpPost("/loadDSNhap")]
        public IActionResult loadDSNhap(int idhh)
        {
            MedicalShopContext context = new MedicalShopContext();
            //List<ChiTietPhieuNhap> listctnk = context.ChiTietPhieuNhap.Where(x => x.Idhh == idhh).OrderBy(x => x.Hsd).ToList();

            List<ChiTietPhieuNhap> listctnk = context.TonKho.Include(x => x.IdctpnNavigation)
              .Where(x => x.IdctpnNavigation.Idhh == idhh).Select(x => x.IdctpnNavigation)
              .OrderBy(x => x.Hsd).ToList();


            ViewBag.ListCTNK = listctnk;
            ViewBag.IDHH = idhh;
            return PartialView();
        }

        

        [Route("/loadGiaLT")]
        public IActionResult loadGiaLT(int check, int idCN)
        {
            MedicalShopContext context = new MedicalShopContext();

            var results = context.HhGia
                      .Where(x => (check == 1 ? x.Price == null : true) || (check == 1 ? x.TiLe == null : true))
                      .ToList();

            //hàng hoá chưa set tỉ lệ/ giá bán
            var hh1 = context.HangHoa
                    .Where(hh => (check == 1 ? (!hh.HhGia.Any(x => x.Idcn == idCN)) : true) 
                    && hh.Active == true) // && hh.Idcn == idCN
                    .ToList();



            var cachTinhGia = context.CachTinhGia.Where(x => x.Idcn == idCN).FirstOrDefault();
           // var danhSachHangHoa = new List<TonKho>();
            var dsHHTon = new List<TonKho>();
       
                var group = context.TonKho
                 .Where(x => x.Idcn == idCN)
                 .GroupBy(x => x.Idhh)
                 .Select(group => new
                 {
                     Idhh = group.Key,
                     MaxGiaNhap = group.Max(x => x.GiaNhap),
                     MinGiaNhap = group.Min(x => x.GiaNhap),
                     MediumGiaNhap = group.Average(x => x.GiaNhap)
                 })
                 .ToList();

            List<dynamic> listReturn = new List<dynamic>();
            var hhGiaList = context.HhGia.Include(x => x.IdhhNavigation).Where(x => x.Idcn == idCN).ToList();

            foreach (var tonKho in group)
            {
                var hhgia = context.HhGia.Where(x => x.Idhh == tonKho.Idhh && tonKho.Idhh == idCN).FirstOrDefault();
                var gia = cachTinhGia.Max == true ? tonKho.MaxGiaNhap : cachTinhGia.Min == true ? tonKho.MinGiaNhap : tonKho.MediumGiaNhap;
                var giaLT = hhGiaList.Where(x => x.Idhh == tonKho.Idhh && (x.Price * 1.05) < gia).FirstOrDefault();
                if (giaLT != null)
                {
                    listReturn.Add(new HangHoa()
                    {
                        Id = (int)giaLT.Idhh,
                        TenHh = giaLT.IdhhNavigation.TenHh,
                        Idcn = giaLT.Idcn,
                    });
                }
            }

            ViewBag.Load = hh1.Union(listReturn).OrderBy(x => x.TenHh).ToList();
            ViewBag.IdChiNhanh = idCN;

            return PartialView("LoadGiaLT");
        }



        [HttpPost("/updatePrices")]
        public IActionResult updatePrices([FromBody] IEnumerable<PriceModel> list) //JsonResult
        {
            MedicalShopContext context = new MedicalShopContext();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            //int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            try
            {
                foreach (var item in list)
                {
                    HhGia gia = context.HhGia.FirstOrDefault(n => n.Idhh == item.Idhh && n.Idcn == item.Idcn);
                    if (gia == null)
                    {
                        HhGia newModel = new HhGia();
                        if (item.TiLe != null || item.Price != null)
                        {
                            if (item.TiLe != null)
                            {
                                newModel.TiLe = item.TiLe;
                                newModel.Price = null;
                            }
                            if (item.Price != null)
                            {
                                newModel.Price = item.Price;
                                newModel.TiLe = null;
                            }
                            newModel.Idhh = item.Idhh;
                            newModel.CreatedBy = idUser;
                            newModel.CreatedDate = DateTime.Now;
                            newModel.ModifiedBy = idUser;
                            newModel.ModifiedDate = DateTime.Now;
                            newModel.Idcn = item.Idcn;
                            newModel.Active = true;
                            context.HhGia.Add(newModel);
                        }
                    }
                    else
                    {
                        if (item.TiLe != null || item.Price != null)
                        {
                            if (item.TiLe != null)
                            {
                                gia.TiLe = item.TiLe;
                                gia.Price = null;
                            }
                            if (item.Price != null)
                            {
                                gia.Price = item.Price;
                                gia.TiLe = null;
                            }
                            gia.ModifiedBy = idUser;
                            gia.ModifiedDate = DateTime.Now;
                            context.HhGia.Update(gia);
                        }
                        else
                        {
                            context.HhGia.Remove(gia);
                        }
                    }
                }
                context.SaveChanges();
                var result = new
                {
                    statusCode = 200,
                    message = "Thành công!",
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    statusCode = 500,
                    message = "Thất bại!",
                };
                return Ok(result);
            }
        }


/// <summary>
/// CÁCH TÍNH GIÁ
/// </summary>
/// <returns></returns>
        public IActionResult CachTinhGia()
        {
            MedicalShopContext context = new MedicalShopContext();
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            int idvt = int.Parse(User.Claims.ElementAt(3).Value);
            var type = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt).Type;
            List<ChiNhanh> listChiNhanh = context.ChiNhanh
            .Where(x => x.Active == true && (type == 1 ? true : x.Id == idcn))
            .ToList();

            ViewData["Title"] = "Cách tính giá";
            return View("CachTinhGia", listChiNhanh);
        }


        [HttpPost("/updateCachTinhGia")]
        public IActionResult updateCachTinhGia([FromBody] CachTinhGia model) //JsonResult
        {
            MedicalShopContext context = new MedicalShopContext();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idcn = int.Parse(User.Claims.ElementAt(4).Value);
            try
            {
                CachTinhGia gia = context.CachTinhGia.FirstOrDefault(n => n.Id == model.Id);
                if (gia == null)
                {
                    CachTinhGia giaa = new CachTinhGia();
                   // giaa.Id = model.Id;
                   
                    giaa.Max = model.Max;
                    giaa.Medium = model.Medium;
                    giaa.Min = model.Min;
                    giaa.Idcn = idcn;
                    context.CachTinhGia.Add(giaa);
                }
                else
                {
                    gia.Max = model.Max;
                    gia.Medium = model.Medium;
                    gia.Min = model.Min;
                    context.CachTinhGia.Update(gia);
                }
                context.SaveChanges();
                var result = new
                {
                    statusCode = 200,
                    message = "Thành công!",
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    statusCode = 500,
                    message = "Thất bại!",
                };
                return Ok(result);
            }
        }



    }
}
