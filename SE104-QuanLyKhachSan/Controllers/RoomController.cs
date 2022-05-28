using Microsoft.AspNetCore.Mvc;
using SE104_QuanLyKhachSan.Models;
using System;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class RoomController : Controller
    {
        public string SessionKeyUser { get; private set; }

        //public JsonResult CheckLogin(Microsoft.AspNetCore.Http.IFormCollection formLogin)
        //{
        //    Phong newRoom = new Phong();
        //    Database database = new Database();

        //    newRoom.GhiChu = formLogin["#GhiChu"].ToString();
        //    newRoom.MaLoaiPhong = idLoaiPhong;
        //    newRoom.MaPhong = formLogin["#MaPhong"].ToString();
        //    newRoom.Tang = Convert.ToByte(formLogin["#Tang"]);
        //    newRoom.TrangThai = idTrangThai;

        //    database.postNewRoom(newRoom);
           
        //}
        public IActionResult Index()
        {
            /*    (document).ready(function() {
               $("#add_info_popup_window_room_button").click(function() {
           
                       }
                   });*/
            return View();
            }
    }
}
