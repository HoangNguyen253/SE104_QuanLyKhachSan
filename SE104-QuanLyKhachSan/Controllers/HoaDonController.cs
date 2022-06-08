using Microsoft.AspNetCore.Mvc;
using SE104_QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class HoaDonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DanhSachHoaDon()
        {
            return PartialView();
        }
        public JsonResult LoadDataForDSHD()
        {
            Database database = new Database();
            return Json(database.LoadDataForDSHD());
        }
        public JsonResult GetCTHDPayRoom(string maPhong)
        {
            Database database = new Database();
            return Json(database.GetCTHDPayRoom(maPhong));
        }
        public JsonResult CreateNewReceipt(string strPhongs, string doiTuongThanhToan, string maNhanVien)
        {
            Database database = new Database();
            return Json(database.CreateNewReceipt(strPhongs, doiTuongThanhToan, maNhanVien));
        }        
        public JsonResult GetDetailOldReceipt(string maHoaDon)
        {
            Database database = new Database();
            return Json(database.GetDetailOldReceipt(maHoaDon));
        }
        public JsonResult LoadDataForDanhSachHoaDonByFilter(DateTime ngayHoaDon, int? maHoaDon)
        {
            Database database = new Database();
            return Json(database.LoadDataForDanhSachHoaDonByFilter(ngayHoaDon, maHoaDon));
        }

    }
}
