using Microsoft.AspNetCore.Mvc;
using SE104_QuanLyKhachSan.Models;
using System;

namespace SE104_QuanLyKhachSan.Controllers
{
    public class RegulationController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }

        public JsonResult GetLoaiPhong()
        {
            Database db = new Database();
            return Json(db.GetLoaiPhong());
        }

        public string DeleteLoaiPhong(int maLoaiPhong)
        {
            Database db =new Database();
            if (db.DeleteLoaiPhong(maLoaiPhong))
            {
                return "true";
            }
            return "false";
        }
        public string UpdateLoaiPhong(int maLoaiPhong, string tenLoaiPhong, int giaTienCoBan)
        {
            Database db =new Database();
            if (db.UpdateLoaiPhong(maLoaiPhong, tenLoaiPhong, giaTienCoBan)) return "true";
            return "false";
        }
        public string InsertLoaiPhong(string tenLoaiPhong, int giaTienCoBan)
        {
            Database db = new Database();
            if (db.InsertLoaiPhong(tenLoaiPhong, giaTienCoBan)) return "true";
            return "false";
        }

        public JsonResult GetLoaiKhachHang()
        {
            Database db = new Database();
            return Json(db.GetLoaiKhachHang());
        }

        public string DeleteLoaiKhachHang(int maLoaiKhachHang)
        {
            Database db = new Database();
            if (db.DeleteLoaiKhachHang(maLoaiKhachHang))
            {
                return "true";
            }
            return "false";
        }

        public string UpdateLoaiKhachHang(int maLoaiKhachHang, string tenLoaiKhachHang)
        {
            Database db = new Database();
            if (db.UpdateLoaiKhachHang(maLoaiKhachHang, tenLoaiKhachHang)) return "true";
            return "false";
        }

        public string InsertLoaiKhachHang(string tenLoaiKhachHang)
        {
            Database db = new Database();
            if (db.InsertLoaiKhachHang(tenLoaiKhachHang)) return "true";
            return "false";
        }
        
        public string GetSoKhachToiDa()
        {
            Database db = new Database();
            return db.GetSoKhachToiDa().ToString();
        }

        public int UpdateSoKhachToiDa(int soKhachToiDa)
        {
            Database db = new Database();
            if (db.UpdateSoKhachToiDa(soKhachToiDa) == 1) return 1;
            return 2;
        }

        public JsonResult GetPhuThuSoKhach()
        {
            Database db = new Database();
            return Json(db.GetPhuThuSoKhach());
        }

        public string DeletePhuThuSoKhach(int soKhachApDung, string ngayApDung, string maPhuThuSoKhach)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);
            if ((int)(date - DateTime.Now).TotalSeconds > 0)
            {
                if (db.DeletePhuThuSoKhachFuture(Convert.ToInt32(maPhuThuSoKhach))) return "true";
                return "false";
            }
            if (db.DeletePhuThuSoKhach(soKhachApDung))
            {
                return "true";
            }
            return "false";
        }

        public string UpdatePhuThuSoKhach(int soKhachApDung, int tiLePhuThu, string ngayApDung, string maPhuThuSoKhach)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);
            if (maPhuThuSoKhach != "null")
            {
                if (db.UpdatePhuThuSoKhachFuture(soKhachApDung, tiLePhuThu, date, Convert.ToInt32(maPhuThuSoKhach))) return "true";
                return "false";
            }
            if (db.UpdatePhuThuSoKhach(soKhachApDung, tiLePhuThu))
            {
                return "true";
            }
            return "false";
        }

        public string InsertPhuThuSoKhach(int soLuongApDung, int tiLePhuThu, string thoiGianApDung)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(thoiGianApDung);

            if (db.InsertPhuThuSoKhach(soLuongApDung, tiLePhuThu, date))
            {
                return "true";
            }
            return "false";
        }
        public JsonResult GetPhuThuLoaiKhachHang()
        {
            Database db = new Database();
            return Json(db.GetPhuThuLoaiKhachHang());
        }

        public string DeleteHeSoPhuThu(int maLoaiKhachHang, int soLuongApDung, string ngayApDung, int maPhuThuLKH)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);
            if ((int)(date - DateTime.Now).TotalSeconds > 0)
            {
                if (db.DeleteHeSoPhuThuFuture(maPhuThuLKH)) return "true";
                return "false";
            }
            if (db.DeleteHeSoPhuThu(maLoaiKhachHang, soLuongApDung))
            {
                return "true";
            }
            return "false";
        }

        public string UpdateHeSoPhuThu(int maLoaiKhachHang, int soLuongApDung, double heSoPhuThu, string ngayApDung, string maHeSoPhuThu)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);
            if (maHeSoPhuThu != "null")
            {
                if (db.UpdateHeSoPhuThuFuture(maLoaiKhachHang, soLuongApDung, heSoPhuThu, date, Convert.ToInt32(maHeSoPhuThu))) return "true";
                return "false";
            }
            if (db.UpdateHeSoPhuThu(maLoaiKhachHang, soLuongApDung, heSoPhuThu))
            {
                return "true";
            }
            return "false";
        }

        public string InsertHeSoPhuThu(int maLoaiKhachHang, int soLuongApDung, double heSoPhuThu, string ngayApDung)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);

            if (db.InsertHeSoPhuThu(maLoaiKhachHang, soLuongApDung, heSoPhuThu,date))
            {
                return "true";
            }
            return "false";
        }


        public JsonResult GetLoaiPhuThuCICO()
        {
            Database db = new Database();
            return Json(db.GetLoaiPhuThuCICO());
        }

        public JsonResult GetPhuThuCICO()
        {
            Database db = new Database();
            return Json(db.GetPhuThuCICO());
        }

        public string DeletePhuThuCICO(int maLoaiPhuThu, int soLuongApDung, string ngayApDung, int maPhuThu)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);
            if ((int)(date - DateTime.Now).TotalSeconds > 0)
            {
                if (db.DeletePhuThuCICOFuture(maPhuThu)) return "true";
                return "false";
            }
            if (db.DeletePhuThuCICO(maLoaiPhuThu, soLuongApDung))
            {
                return "true";
            }
            return "false";
        }

        public string UpdatePhuThuCICO(int maLoaiPhuThu, int soLuongApDung, int tiLePhuThu, string ngayApDung, string maPhuThu)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);
            if (maPhuThu != "null")
            {
                if (db.UpdatePhuThuCICOFuture(maLoaiPhuThu, soLuongApDung, tiLePhuThu, date, Convert.ToInt32(maPhuThu))) return "true";
                return "false";
            }
            if (db.UpdatePhuThuCICO(maLoaiPhuThu, soLuongApDung, tiLePhuThu))
            {
                return "true";
            }
            return "false";
        }

        public string InsertPhuThuCICO(int maLoaiPhuThu, int soLuongApDung, int tiLePhuThu, string ngayApDung)
        {
            Database db = new Database();
            DateTime date = Convert.ToDateTime(ngayApDung);

            if (db.InsertPhuThuCICO(maLoaiPhuThu, soLuongApDung, tiLePhuThu, date))
            {
                return "true";
            }
            return "false";
        }

        public JsonResult GetGioCICO()
        {
            Database db = new Database();
            return new JsonResult(db.getGioCICO());
        }
        public string UpdateGioCICO(string gioCheckIn, string gioCheckOut)
        {
            Database db = new Database();
            if (db.UpdateGioCICO(gioCheckIn, gioCheckOut)) return "true";
            return "false";
        }

        public JsonResult GetChucVu()
        {
            Database db = new Database();
            return Json(db.GetChucVu());
        }

        public string DeleteChucVu(int maChucVu)
        {
            Database db = new Database();
            if (db.DeleteChucVu(maChucVu))
            {
                return "true";
            }
            return "false";
        }

        public string UpdateChucVu(int maChucVu, string tenChucVu)
        {
            Database db = new Database();
            if (db.UpdateChucVu(maChucVu, tenChucVu)) return "true";
            return "false";
        }

        public string InsertChucVu(string tenChucVu)
        {
            Database db = new Database();
            if (db.InsertChucVu(tenChucVu)) return "true";
            return "false";
        }

        public string getLuongToiThieuVung()
        {
            Database db =new Database();
            return (db.getLuongToiThieuVung());
        }
        public string UpdateLuongToiThieuVung(int luongToiThieuVung)
        {
            Database database = new Database();
            if (database.UpdateLuongToiThieuVung(luongToiThieuVung)) return "true";
            return "false";
        }
    }
}
