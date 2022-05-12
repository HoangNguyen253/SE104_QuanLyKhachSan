using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class HoaDon
    {
        private int maHoaDon; 
        public int MaHoaDon
        {
            get { return maHoaDon; }
            set { maHoaDon = value; }
        }

        private DateTime thoiGianXuat;
        public DateTime ThoiGianXuat
        {
            get { return thoiGianXuat; }
            set { thoiGianXuat = value; }
        }

        private string maNhanVien;
        public string MaNhanVien
        {
            get { return maNhanVien; }
            set { maNhanVien = value; }
        }

        private int tongSoTien;
        public int TongSoTien
        {
            get { return tongSoTien; }
            set { tongSoTien = value; }
        }

        private string doiTuongThanhToan;
        public string DoiTuongThanhToan
        {
            get { return doiTuongThanhToan; }
            set { doiTuongThanhToan = value; }
        }
    }
}
