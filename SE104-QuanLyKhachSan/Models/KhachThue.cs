using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class KhachThue
    {
        private int maKhachThue;
        public int MaKhachThue
        {
            get { return maKhachThue; }
            set { maKhachThue = value; }
        }

        private string cccd;
        public string CCCD
        {
            get { return cccd; }
            set { cccd = value; }
        }

        private DateTime thoiGianCheckin;
        public DateTime ThoiGianCheckin
        {
            get { return thoiGianCheckin; }
            set { thoiGianCheckin = value; }
        }

        private DateTime thoiGianCheckout;
        public DateTime ThoiGianCheckout
        {
            get { return thoiGianCheckout; }
            set { thoiGianCheckout = value; }
        }

        private string hoTen;
        public string HoTen
        {
            get { return hoTen; }
            set { hoTen = value; }
        }

        private int maLoaiKhachHang;
        public int MaLoaiKhachHang
        {
            get { return maLoaiKhachHang; }
            set { maLoaiKhachHang = value; }
        }

        private string? diaChi;
        public string? DiaChi
        {
            get { return diaChi; }
            set { diaChi = value; }
        }

        private int? maCTHD;
        public int? MaCTHD
        {
            get { return maCTHD; }
            set { maCTHD = value; }
        }
    }
}
