using System;
namespace SE104_QuanLyKhachSan.Models
{
    public class LoaiPhong
    {
        private int maLoaiPhong;
        public int MaLoaiPhong
        {
            get { return maLoaiPhong; }
            set { maLoaiPhong = value; }
        }

        private string tenLoaiPhong;
        public string TenLoaiPhong
        {
            get { return tenLoaiPhong; }
            set { tenLoaiPhong = value; }
        }

        private int giaTienCoBan;
        public int GiaTienCoBan
        {
            get { return giaTienCoBan; }
            set { giaTienCoBan = value; }
        }
    }
}
