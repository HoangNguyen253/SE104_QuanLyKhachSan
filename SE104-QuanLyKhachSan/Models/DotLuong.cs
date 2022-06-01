using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class DotLuong
    {
        private int _MaDotTraLuong;
        public int MaDotTraLuong
        {
            get { return _MaDotTraLuong; }
            set { _MaDotTraLuong = value; }
        }

        private DateTime _NgayTraLuong;
        public DateTime NgayTraLuong
        {
            get { return _NgayTraLuong; }
            set { _NgayTraLuong = value; }
        }

        private DateTime _Ngaylap;
        public DateTime Ngaylap
        {
            get { return _Ngaylap; }
            set { _Ngaylap = value; }
        }
    }
}
