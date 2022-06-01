using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SE104_QuanLyKhachSan.Models;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class BillDetailController : Controller
    {
        const string SessionKeyUser = "_User";

        private readonly ILogger<BillDetailController> _logger;

        public BillDetailController(ILogger<BillDetailController> logger)
        {
            _logger = logger;
        }

        public JsonResult GetAllDetailBills()
        {
            Database db = new Database();
            return Json(db.GetAllDetailBills());
        }

        public JsonResult GetDetailByDetailID(int maCTHD)
        {
            Database db = new Database();
            return Json(db.GetDetailByDetailID(maCTHD));
        }

        public void DeleteDetailById(int maCTHD)
        {
            Database db = new Database();
            db.DeleteDetailById(maCTHD);
        }

        public void UpdateCancelStatusDetail(int maCTHD)
        {
            Database db = new Database();
            db.UpdateCancelStatusDetail(maCTHD);
        }
    }
}
