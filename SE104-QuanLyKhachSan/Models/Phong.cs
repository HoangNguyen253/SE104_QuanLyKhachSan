using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class Phong
    {
        private string maPhong;
        public string MaPhong
        {
            get { return maPhong; }
            set { maPhong = value; }
        }
        private int maLoaiPhong;
        public int MaLoaiPhong
        {
            set { maLoaiPhong = value; }
            get { return maLoaiPhong; }
        }
        private string tenLoaiPhong;
        public string TenLoaiPhong
        {
            get { return tenLoaiPhong; }
            set { tenLoaiPhong = value; }
        }
        private byte tang;
        public byte Tang
        {
            get { return tang; }
            set { tang = value; }
        }
        private byte soPhong;
        public byte SoPhong
        {
            get { return soPhong; }
            set { soPhong = value; }
        }
        private byte trangThai;
        public byte TrangThai
        {
            get { return trangThai; }
            set { trangThai = value; }
        }

        private string ghiChu;
        public string GhiChu
        {
            get { return ghiChu; }
            set { ghiChu = value; }
        }
          
    }
}
