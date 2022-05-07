using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class NhanVien
    {
        private string maNhanVien;
        public string MaNhanVien
        {
            get { return maNhanVien; }
            set { maNhanVien = value; }
        }

        private string matKhau;
        public string MatKhau
        {
            get { return matKhau; }
            set { matKhau = value; }
        }

        private string cccd;
        public string CCCD
        {
            get { return cccd; }
            set { cccd = value; }
        }

        private string hoTen;
        public string HoTen
        {
            get { return hoTen; }
            set { hoTen = value; }
        }

        private byte gioiTinh;
        public byte GioiTinh
        {
            get { return gioiTinh; }
            set { gioiTinh = value; }
        }

        private DateTime ngaySinh;
        public DateTime NgaySinh
        {
            get { return ngaySinh; }
            set { ngaySinh = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string soDienThoai;
        public string SoDienThoai
        {
            get { return soDienThoai; }
            set { soDienThoai = value; }
        }

        private DateTime ngayVaoLam;
        public DateTime NgayVaoLam
        {
            get { return ngayVaoLam; }
            set { ngayVaoLam = value; }
        }

        private ushort maChucVu;
        public ushort MaChucVu
        {
            get { return maChucVu; }
            set { maChucVu = value; }
        }

        private string hinhAnh;
        public string HinhAnh
        {
            get { return hinhAnh; }
            set { hinhAnh = value; }
        }

        private int luong;
        public int Luong
        {
            get { return luong; }
            set { luong = value; }
        }
    }
}
