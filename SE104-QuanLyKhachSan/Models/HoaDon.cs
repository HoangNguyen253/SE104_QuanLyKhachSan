using System;
using System.Collections.Generic;

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

        private NhanVien nv;
        public NhanVien NV
        {
            get { return nv; }
            set { nv = value; }
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

        private List<ChiTietHoaDon> chiTiet;
        public List<ChiTietHoaDon> ChiTiet
        {
            get { return chiTiet; }
            set { chiTiet = value; }
        }
    }
}
