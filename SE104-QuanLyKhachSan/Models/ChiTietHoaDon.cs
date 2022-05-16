using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class ChiTietHoaDon
    {
        private int maCTHD;

        public int MaCTHD
        {
            get { return maCTHD; }
            set { maCTHD = value; }
        }

        private string maPhong;
        public string MaPhong
        {
            get { return maPhong; }
            set { maPhong = value; }
        }

        private LoaiPhong lp;
        public LoaiPhong LoaiPhong
        {
            get { return lp; }
            set { lp = value; }
        }

        private int maHoaDon;
        public int MaHoaDon
        {
            get { return maHoaDon; }
            set { maHoaDon = value; }
        }

        private DateTime thoiGianNhanPhong;
        public DateTime ThoiGianNhanPhong
        {
            get { return thoiGianNhanPhong; }
            set { thoiGianNhanPhong = value; }
        }

        private DateTime thoiGianTraPhong;
        public DateTime ThoiGianTraPhong
        {
            get { return thoiGianTraPhong; }
            set { thoiGianNhanPhong = value; }
        }

        private int giaPhong;
        public int GiaPhong
        {
            get { return giaPhong; }
            set { giaPhong = value; }
        }

        private string ghiChu;
        public string GhiChu
        {
            get { return ghiChu; }
            set { ghiChu = value; }
        }

        private int tongTienPhong;
        public int TongTienPhong
        {
            get { return tongTienPhong; }
            set { tongTienPhong = value; }
        }

        private byte trangThai;
        public byte TrangThai
        {
            get { return trangThai; }
            set { trangThai = value; }
        }
    }
}
