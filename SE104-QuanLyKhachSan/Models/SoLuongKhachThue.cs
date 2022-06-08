namespace SE104_QuanLyKhachSan.Models
{
    public class SoLuongKhachThue
    {
        private int maSLKT;
        public int MaSLKT
        {
            get { return maSLKT; }
            set { maSLKT = value; }
        }

        private byte soKhachThue;
        public byte SoKhachThue
        {
            get { return soKhachThue; }
            set { soKhachThue = value; }
        }

        // Them vao structure DB
        private byte soKhachNN;
        public byte SoKhachNN
        {
            get { return soKhachNN; }
            set { soKhachNN = value; }
        }

        private byte soNgayThue;
        public byte SoNgayThue
        {
            get { return soNgayThue; }
            set { soNgayThue = value; }
        }

        private string ghiChu;
        public string GhiChu
        {
            get { return ghiChu; }
            set { ghiChu = value; }
        }

        private int donGia;
        public int DonGia
        {
            get { return donGia; }
            set { donGia = value; }
        }

        private int phuThu;
        public int PhuThu
        {
            get { return phuThu; }
            set { phuThu = value; }
        }

        private double heSoKhach;
        public double HeSoKhach
        {
            get { return heSoKhach; }
            set { heSoKhach = value; }
        }

        private int thanhTien;
        public int ThanhTien
        {
            get { return thanhTien; }
            set { thanhTien = value; }
        }

        private int maCTHD;
        public int MaCTHD
        {
            get { return maCTHD; }
            set { maCTHD = value; }
        }
    }
}
