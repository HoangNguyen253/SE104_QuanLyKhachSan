using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SE104_QuanLyKhachSan.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class BillController : Controller
    {
        const string SessionKeyUser = "_User";

        private readonly ILogger<BillController> _logger;

        public BillController(ILogger<BillController> logger)
        {
            _logger = logger;
        }

        public JsonResult GetAllBill()
        {
            Database db = new Database();
            return Json(db.GetAllBill());
        }
        public JsonResult GetDetailById(int maHoaDon)
        {
            Database db = new Database();
            return Json(db.GetDetailById(maHoaDon));
        }

        public void PendingDetail(string roomID)
        {
            Database db = new Database();
            db.PendingDetail(roomID);
        }

        public void CancelDetailBill()
        {
            Database db =  new Database();
            db.CancelDetailBill();
        }

        public bool IsPendingListNotNull()
        {
            Database db = new Database();
            return db.IsPendingListNotNull();
        }

        public JsonResult GetNullDetailByRoomID(string roomID)
        {
            Database db = new Database();
            return Json(db.GetNullDetailByRoomID(roomID));
        }

        public int SaveListDetailBill(string maNV, string doiTuong)
        {
            Database db = new Database();
            return db.SaveListDetailBill(maNV, doiTuong);
        }

        public JsonResult ConvertToSLKT(int maCTHD)
        {
            Database db = new Database();
            return Json(db.ConvertToSLKT(maCTHD));
        }

        public JsonResult CheckInsertDetailToBill(string roomID)
        {
            Database db = new Database();
            return Json(db.CheckInsertDetailToBill(roomID));
        }

        public JsonResult GetHoaDon(int MaHoaDon)
        {
            Database db = new Database();
            return Json(db.GetHoaDon(MaHoaDon));
        }
    }
}
