using Microsoft.AspNetCore.Mvc;
using SE104_QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class PhieuThuePhongController : Controller
    {
        const string SessionKeyUser = "_User";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DanhSachThuePhong()
        {
            return PartialView();
        }
        public JsonResult LoadDataForDSTP()
        {
            Database database = new Database();
            return Json(database.LoadDataForDSTP());
        }        
        public JsonResult LoadDataForDSTPByFilter(string ngayDenO, string CCCD, string maPhong)
        {
            Database database = new Database();
            return Json(database.LoadDataForDSTPByFilter(ngayDenO, CCCD, maPhong));
        }
        public JsonResult GetPhieuThuePhong(int maCTHD, string trangThai)
        {
            Database database = new Database();
            if (trangThai == "dathanhtoan")
                return Json(database.GetPhieuThuePhongDaThanhToan(maCTHD));
            else if (trangThai == "dangthue")
            {
                return Json(database.GetPhieuThuePhongDangThue(maCTHD));
            }
            else return Json("");
        }
        public JsonResult ThanhToanPhieuThueBoPhong(int maCTHD, string doiTuongThanhToan)
        {
            NhanVien nhanVien = HttpContext.Session.Get<NhanVien>(SessionKeyUser);
            Database database = new Database();
            return Json(database.HoaDonChoBoPhong(maCTHD, doiTuongThanhToan, nhanVien.MaNhanVien, nhanVien.HoTen));
        }
        public JsonResult GetCTHDBoPhong(int maCTHD)
        {
            Database database = new Database();
            return Json(database.GetCTHDBoPhong(maCTHD));
        }
        public string XoaPhieuThuePhong(int maCTHD)
        {
            Database database = new Database();
            return database.XoaPhieuThuePhongByID(maCTHD);
        }
    }
}
