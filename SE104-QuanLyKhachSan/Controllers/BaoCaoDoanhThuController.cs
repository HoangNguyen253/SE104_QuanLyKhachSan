using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SE104_QuanLyKhachSan.Models;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class BaoCaoDoanhThuController : Controller
    {
        const string SessionKeyUser = "_User";

        private readonly ILogger<BillController> _logger;
        public IActionResult Index()
        {
            return View();
        }


        public JsonResult RemoveMonthReport(string MaBC)
        {

            int MaBCDoanhThu = System.Convert.ToInt32(MaBC);
            Database db = new Database();
            bool isSucces = System.Convert.ToBoolean(db.XoaBaoCaoDoanhThuThang(MaBCDoanhThu));
            if (isSucces)
                return Json(true);
            else
                return Json(false);
        }


    }
}
