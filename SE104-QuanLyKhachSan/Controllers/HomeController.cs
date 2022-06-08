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

        public IActionResult DTtheoLoaiPhong()
        {
            Database db = new Database();
            List<BaoCaoDoanhThuThang> list_bcdt = db.getAllBCDTThang();
            ViewData["list_bcdt"] = list_bcdt;
            return PartialView();
        }

        public IActionResult DotLuong()
        {
            Database db =  new Database(); 
            List<DotLuong> list_dotluong = db.getAllDotLuong();
            ViewData["list_dotluong"] = list_dotluong;
            return PartialView();
        }


        public IActionResult LuongChucVu(string Thang)
        {
            Database db = new Database();
            DateTime dt = DateTime.Parse(Thang);
            List<BaoCaoLuongChucVu> list_bccv = db.getAllBaoCaoLuongChucVu(dt);
            ViewData["Thang"] = dt.ToString("MM/yyyy"); 
            ViewData["list_bccv"] = list_bccv;
            return PartialView();
        }

        public IActionResult ThongKeDoanhThu(string ThangBaoCao)
        {
            Database db = new Database();
            DateTime dt = DateTime.Parse(ThangBaoCao);
            ThongKeDoanhThu thongke = db.GetThuChi(dt);
            ViewData["thongke"] = thongke;
            return PartialView();
        }

        public int UpdateLuongStaff(IFormCollection form)
        {
            Database db = new Database();
            ChiTietDotTraLuong info_Staf = new ChiTietDotTraLuong();
            info_Staf.MaDotTraLuong = Convert.ToInt32(form["MaDotTraLuong"]);
            info_Staf.MaNhanVien = (form["MaNhanVien"]).ToString();
            info_Staf.Thuong = Convert.ToInt32(form["Thuong"]);
            info_Staf.Phat = Convert.ToInt32(form["Phat"]);
            info_Staf.GhiChu = form["GhiChu"].ToString(); 
            info_Staf.SoTien = Convert.ToInt32(form["SoTien"]);
            db.UpdateLuongStaf(info_Staf);
            return db.UpdateLuongStaf(info_Staf);
        }

        public IActionResult ChiTietDotLuong(string MaBCDL, string ThangBaoCao, string NgayLap = "")
        {
            var mabc = System.Convert.ToInt32(MaBCDL);
            Database db = new Database();
            List<ChiTietDotTraLuong> list_dtl = null;
            if (NgayLap == "")
                list_dtl = db.getDetailDotTraLuongbyID(mabc);
            else
                list_dtl = db.getDetailDotTraLuongbyMonth(ThangBaoCao);
            ViewData["list_dtl"] = list_dtl;
            ViewData["ThangBaoCao"] = ThangBaoCao;
            return PartialView();
        }


        public IActionResult ChiTietDoanhThuPhong(string MaBCDoanhThu, string ThangBaoCao ,string Thang = "")
        {
            var mabc = System.Convert.ToInt32(MaBCDoanhThu);
            Database db = new Database();
            List<ChiTietBaoCaoDoanhThuThang> list_dt = new List<ChiTietBaoCaoDoanhThuThang>();
            
            if (Thang == "")
                list_dt = db.getDetailBCDTThangbyID(mabc);
            else
                list_dt = db.getDetailBCDTThangbyMonth(Thang); 
            ViewData["list_dt"] = list_dt;
           
            ViewData["ThangBaoCao"] = ThangBaoCao;
            
            return PartialView();
        }

        public int RemoveMonthReport(string MaBC)
        {

            int MaBCDoanhThu = System.Convert.ToInt32(MaBC);
            Database db = new Database();
            int isSucces = db.XoaBaoCaoDoanhThuThang(MaBCDoanhThu);
            return isSucces;
        }
        public int AddMonthReport(string ThangBC, string NgayLap)
        {
            Database db = new Database();
            DateTime thangbc = DateTime.ParseExact(ThangBC, "dd-MM-yyyy", null);
            DateTime ngaylap = DateTime.Parse(NgayLap);
            return db.UpdateCTBCDTThang(thangbc, ngaylap); 
        }


        public int AddDotLuongReport(string NgayTraLuong, string NgayLap)
        {
            Database db = new Database();
            DateTime ngaytraluong = DateTime.ParseExact(NgayTraLuong, "dd-MM-yyyy", null);
            DateTime ngaylap = DateTime.Parse(NgayLap);
            return db.UpdateCTDotLuong(ngaytraluong, ngaylap);
        }
        public int RemoveDotLuong(string MaBCDL)
        {
            int MaDotLuong = System.Convert.ToInt32(MaBCDL);
            Database db = new Database();
            int isSucces = db.XoaBaoCaoDotLuong(MaDotLuong);
            return isSucces;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public string addRoom(IFormCollection formAdd)
        {
            Phong newRoom = new Phong();
            Database database = new Database();

            newRoom.GhiChu = formAdd["GhiChu"].ToString();
            newRoom.MaPhong = formAdd["MaPhong"].ToString();
            newRoom.Tang = Convert.ToByte(formAdd["Tang"]);
            newRoom.SoPhong = Convert.ToByte(formAdd["SoPhong"]);
            newRoom.TrangThai = Convert.ToByte(formAdd["TrangThai"]);
            newRoom.MaLoaiPhong = Convert.ToByte(formAdd["MaLoaiPhong"]);

            return database.postNewRoom(newRoom);

        }
        public string addStaff(IFormCollection formAdd)
        {
            NhanVien newStaff = new NhanVien();
            Database database = new Database();

            /*  newStaff.MaNhanVien = ;
            newStaff.MatKhau =;*/
            newStaff.CCCD = formAdd["CCCD"].ToString();
            newStaff.HoTen = formAdd["HoTen"].ToString();
            newStaff.GioiTinh = Convert.ToByte(formAdd["GioiTinh"]);
            newStaff.NgaySinh = DateTime.Parse(formAdd["NgaySinh"]);
            newStaff.Email = formAdd["Email"].ToString();
            newStaff.SoDienThoai = formAdd["SoDienThoai"].ToString();
            newStaff.NgayVaoLam = DateTime.Parse(formAdd["NgayVaoLam"]);
            newStaff.MaChucVu = Convert.ToByte(formAdd["MaChucVu"]);
            newStaff.Luong = Convert.ToInt32(formAdd["Luong"]);

            return database.postNewStaff(newStaff);

        }

        public string Reset_Password(string email)
        {
            Database db = new Database();
            int check = db.Reset_Password(email);
            if(check ==  1)
            {
                return "success";
            }    
            else if(check == 2)
            {
                return "Địa chỉ mail bị lỗi";
            }    
            return "Đối mật khẩu không thành công.";
        }

       
        public JsonResult GetPhong()
        {
            Database db = new Database();
            return Json(db.getAllDetailRoom());
        }
        public JsonResult GetChosenStaff(string MaNhanVien)
        {
            Database db = new Database();
            return Json(db.getChosenStaff(MaNhanVien));
        }
        public JsonResult GetChosenRoom(string MaPhong)
        {
            Database db = new Database();
            return Json(db.getChosenRoom(MaPhong));
        }
        public string UpdateStaff(IFormCollection form)
        {
            Database db = new Database();
           NhanVien info_Staf = new NhanVien();
            info_Staf.CCCD = form["CCCD"].ToString();
            info_Staf.MaNhanVien = (form["MaNhanVien"]).ToString();
            info_Staf.MatKhau = (form["MatKhau"]).ToString();
            info_Staf.HoTen = (form["HoTen"]).ToString();
            info_Staf.SoDienThoai = (form["SoDienThoai"]).ToString();
            info_Staf.NgaySinh= Convert.ToDateTime(form["NgaySinh"]);
  
            info_Staf.Email = (form["Email"]).ToString();
            info_Staf.GioiTinh = Convert.ToByte(form["GioiTinh"]);

            info_Staf.NgayVaoLam = Convert.ToDateTime(form["NgayVaoLam"]);
            info_Staf.Luong = Convert.ToInt32(form["Luong"]);
            info_Staf.MaChucVu = Convert.ToByte(form["MaChucVu"]);

            return db.UpdateStaff(info_Staf);
        }
        public string UpdateRoom(IFormCollection form)
        {
            Database db = new Database();
            Phong info_Room = new Phong();
            info_Room.MaPhong = form["MaPhong"].ToString();
            info_Room.MaLoaiPhong = Convert.ToByte(form["MaLoaiPhong"]);
            info_Room.Tang = Convert.ToByte(form["Tang"]);
            info_Room.SoPhong = Convert.ToByte(form["SoPhong"]);
            info_Room.TrangThai = Convert.ToByte(form["TrangThai"]);
            info_Room.GhiChu = form["GhiChu"].ToString();
            return db.UpdateRoom(info_Room);
        }

        public string DeleteStaff(string MaNV)
        {
            Database db = new Database();
            return db.DeleteStaff(MaNV);
        }
        public string DeleteRoom(string MaP)
        {
            Database db = new Database();
            return db.DeleteRoom(MaP);
        }

    }

}
