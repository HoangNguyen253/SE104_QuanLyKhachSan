using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SE104_QuanLyKhachSan.Models;
using Microsoft.AspNetCore.Http;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class SoDoPhongController : Controller
    {
        public JsonResult LoadDataForSoDoPhong()
        {
            Database database = new Database();
            return Json(database.LoadDataForSoDoPhong());
        }
        public JsonResult LoadDataForSoDoPhongByFilter(int tang, int trangThai, string soPhong)
        {
            Database database = new Database();
            return Json(database.LoadDataForSoDoPhongByFilter(tang, trangThai, soPhong));
        }
        public string UpdateStatusForRoom(string maPhong, string trangThai)
        {
            Database database = new Database();
            bool isSuccess = false;
            if (trangThai == "available")
            {
                isSuccess = database.UpdateStatusForRoom(maPhong, 0);
            }
            else if (trangThai == "fixed")
            {
                isSuccess = database.UpdateStatusForRoom(maPhong, 2);
            }
            else if (trangThai == "outed")
            {
                isSuccess = database.UpdateStatusForRoom(maPhong, 3);
            }
            else if (trangThai == "occupied")
            {
                isSuccess = database.UpdateStatusForRoom(maPhong, 1);
            }
            return (isSuccess == true) ? "Success" : "Fail";
        }
        public string UpdateStatusForCTHD(string maPhong, string trangThai)
        {
            Database database = new Database();
            bool isSuccess = false;
            if (trangThai == "khachbophong")
            {
                isSuccess = database.UpdateStatusForCTHD(maPhong, 2);
            }
            return (isSuccess == true) ? "Success" : "Fail";
        }
        public JsonResult GetInfoRoom(string maPhong, string mucDich)
        {
            Database database = new Database();
            if (mucDich == "datphong")
            {
                return Json(database.GetInfoRoomToOrder(maPhong));
            }
            else if (mucDich == "thaydoikhacho")
            {
                return Json(database.GetInfoOrderedRoom(maPhong));
            }
            return Json("");
        }
        public JsonResult GetLoaiKhachHangs()
        {
            Database database = new Database();
            return Json(database.GetLoaiKhachHangs());
        }
        public string DatPhongFromSDP(IFormCollection formData)
        {
            Database database = new Database();
            string maPhong = formData["maPhong"].ToString();
            DateTime thoiGianNhanPhong = Convert.ToDateTime(formData["thoiGianNhanPhong"]);
            string dsKhachThue = formData["thongTinKhachThue_Str"].ToString();
            return database.DatPhongFromSDP(maPhong, thoiGianNhanPhong, dsKhachThue);
        }
        public DateTime CheckoutForKhachThue(int maKhachThue)
        {
            Database database = new Database();
            return database.CheckoutForKhachThue(maKhachThue);
        }
        public string ThayDoiKhachOFromSDP(IFormCollection formData)
        {
            Database database = new Database();
            string maPhong = formData["maPhong"].ToString();
            string dsKhachThue = formData["thongTinKhachThue_Str"].ToString();
            return database.ThayDoiKhachOFromSDP(maPhong, dsKhachThue);
        }
    }
}
