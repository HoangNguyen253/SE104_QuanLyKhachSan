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

        private int maLoaiPhong
        {
            set { maLoaiPhong = value; }
            get { return maLoaiPhong; }
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
