using System;
namespace SE104_QuanLyKhachSan.Models

{
    public class LoaiKhachHang
    {
        private int maLoaiKhachHang;
        public int MaLoaiKhachHang
        {
            get { return maLoaiKhachHang; }
            set { maLoaiKhachHang = value; }
        }

        private string tenLoaiKhachHang;
        public string TenLoaiKhachHang
        {
            get { return tenLoaiKhachHang; }
            set { tenLoaiKhachHang = value; }
        }
    }
}
