using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SE104_QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class HomeController : Controller
    {
        const string SessionKeyUser = "_User";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public string UpdateInfoStaff(IFormCollection form)
        {
            Database database = new Database();
            NhanVien nv = new NhanVien { 
                MaNhanVien = form["maNhanVien"].ToString(),
                HoTen = form["hoTen"].ToString(),
                CCCD = form["cccd"].ToString(),
                SoDienThoai = form["sdt"].ToString(),
                NgaySinh = Convert.ToDateTime(form["ngaySinh"]),
                Email = form["email"].ToString(),
                GioiTinh = Convert.ToByte(form["gioiTinh"]),
            };
            string result = database.UpdateInfoStaff(nv);
            if (result == "success")
            {
                NhanVien _curNhanVien = HttpContext.Session.Get<NhanVien>(SessionKeyUser);
                _curNhanVien.HoTen = nv.HoTen;
                _curNhanVien.CCCD = nv.CCCD;
                _curNhanVien.SoDienThoai = nv.SoDienThoai;
                _curNhanVien.NgaySinh = nv.NgaySinh;
                _curNhanVien.Email = nv.Email;
                _curNhanVien.GioiTinh = nv.GioiTinh;
                HttpContext.Session.Set<NhanVien>(SessionKeyUser, _curNhanVien);
            }
            return result;
        }
        public string ChangePassStaff(IFormCollection form)
        {
            Database database = new Database();
            string maNhanVien = form["maNhanVien"].ToString();
            string pass = form["pass"].ToString();
            return (database.ChangePassStaff(maNhanVien, pass));
        }
        public JsonResult GetStaffOnBoard()
        {
            return Json(HttpContext.Session.Get<NhanVien>(SessionKeyUser));
        }
        public string UploadImage(IFormFile file)
        {
            NhanVien nv = HttpContext.Session.Get<NhanVien>(SessionKeyUser);
            string maNhanVien = nv.MaNhanVien;
            Database database = new Database();
            string path = "/image/NhanVien/" + maNhanVien + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + file.FileName;
            string pathinsert = "wwwroot" + path;
            using (var filestream = new FileStream(Path.Combine(pathinsert), FileMode.Create))
            {
                file.CopyToAsync(filestream);
                database.UpdateImage(path, maNhanVien);
                nv.HinhAnh = path;
                HttpContext.Session.Set<NhanVien>(SessionKeyUser, nv);
                return path;
            }
        }
        public IActionResult LogOutNhanVien()
        {
            HttpContext.Session.Set<NhanVien>(SessionKeyUser, null);
            return Redirect("/Login/Login");
        }
        public IActionResult SoDoPhong()
        {
            return PartialView();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
