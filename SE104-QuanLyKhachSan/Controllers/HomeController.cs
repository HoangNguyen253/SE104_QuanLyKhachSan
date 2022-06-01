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
            return PartialView();
        }

        public IActionResult ListDetail()
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

        public IActionResult ThongKeDoanhThu()
        {
            
            return PartialView();
        }

        public IActionResult ChiTietDotLuong(string MaBCDL, string ThangBaoCao, string NgayLap = "")
        {
            var mabc = System.Convert.ToInt32(MaBCDL);
            Database db = new Database();
            List<ChiTietDotTraLuong> list_dtl = null;
            if (NgayLap == "")
                list_dtl = db.getDetailDotTraLuongbyID(mabc);
            else
                list_dtl = db.getDetailDotTraLuongbyMonth(NgayLap);
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
    }
}
