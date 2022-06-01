using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SE104_QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            NhanVien nhanVien_OnBoard = HttpContext.Session.Get<NhanVien>(SessionKeyUser);
            ViewData["nhanVien_OnBoard"] = nhanVien_OnBoard;
            return View();
        }

        public IActionResult SoDoPhong()
        {
            return PartialView();
        }

        public IActionResult ListBill()
        {

            NhanVien nhanVien_OnBoard = HttpContext.Session.Get<NhanVien>(SessionKeyUser);
            ViewData["nhanVien_OnBoard"] = nhanVien_OnBoard;
            Database db = new Database();
            List<HoaDon> listBill = db.GetAllBill();
            ViewData["listBill"] = listBill;
            return PartialView();
        }

        public IActionResult ListDetail()
        {
            Database db = new Database();
            List<ChiTietHoaDon> listDetails = db.GetAllDetailBills();
            ViewData["listDetails"] = listDetails;
            return PartialView();
        }
        public IActionResult ListStaff()
        {
            return PartialView();
        }
        public IActionResult ListRoom()
        {
            return PartialView();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public JsonResult addRoom(IFormCollection formAdd)
        {
            Phong newRoom = new Phong();
            Database database = new Database();

            newRoom.GhiChu = formAdd["GhiChu"].ToString();
            newRoom.MaPhong = formAdd["MaPhong"].ToString();
            newRoom.Tang = Convert.ToByte(formAdd["Tang"]);
            newRoom.TrangThai = Convert.ToByte(formAdd["TrangThai"]);
            newRoom.MaLoaiPhong = int.Parse(formAdd["LoaiPhong"]);
            return Json(database.postNewRoom(newRoom));

        }
        public JsonResult addStaff(IFormCollection formAdd)
        {
            NhanVien newStaff = new NhanVien();
            Database database = new Database();

            newStaff.MaNhanVien = formAdd["MaNhanVien"].ToString();
            newStaff.MatKhau = formAdd["MatKhau"].ToString();
            newStaff.CCCD = formAdd["CCCD"].ToString();
            newStaff.HoTen = formAdd["HoTen"].ToString();
            newStaff.GioiTinh = Convert.ToByte(formAdd["GioiTinh"]);
            newStaff.NgaySinh = DateTime.Parse(formAdd["NgaySinh"]);
            newStaff.Email = formAdd["Email"].ToString();
            newStaff.SoDienThoai = formAdd["SoDienThoai"].ToString();
            newStaff.NgayVaoLam = DateTime.Parse(formAdd["NgayVaoLam"]);
            newStaff.MaChucVu = Convert.ToByte(formAdd["MaChucVu"]);
            newStaff.Luong = Convert.ToInt32(formAdd["Luong"]);

            newStaff.HinhAnh = (formAdd["NgaySinh"]).ToString();
            return Json(database.postNewStaff(newStaff));

        }
        /* public JsonResult getStaff(IFormCollection formAdd)
         {
             NhanVien getStaff = new NhanVien();
             Database database = new Database();

             getStaff.MaNhanVien = formAdd["MaNhanVien"].ToString();

             return Json(database.getChosenStaff(getStaff.MaNhanVien));

         }*/

       /* public JsonResult delPhong(IFormCollection formAdd)
        {
            Phong phong = new Phong();
            Database database = new Database();

            phong.MaPhong = formAdd["MaPhong"].ToString();

            return Json(database.deleteRoom(phong.MaPhong));

        }*/
    }
}
