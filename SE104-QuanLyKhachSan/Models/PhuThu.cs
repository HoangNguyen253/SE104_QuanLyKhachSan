using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class PhuThu
    {
        private int maPhuThu;
        public int MaPhuThu
        {
            get { return maPhuThu; }
            set { maPhuThu = value; }
        }

        private int soLuongApDung;
        public int SoLuongApDung
        {
            get { return soLuongApDung; }
            set { soLuongApDung = value; }
        }

        private int tiLePhuThu;
        public int TiLePhuThu
        {
            get { return tiLePhuThu; }
            set { tiLePhuThu = value; }
        }

        private int maLoaiPhuThu;
        public int MaLoaiPhuThu
        {
            get { return maLoaiPhuThu; }
            set { maLoaiPhuThu = value; }
        }

        private string tenLoaiPhuThu;
        public string TenLoaiPhuThu
        {
            get { return tenLoaiPhuThu; }
            set { tenLoaiPhuThu = value; }
        }

        private string thoiGianApDung;
        public string ThoiGianApDung
        {
            get { return thoiGianApDung; }
            set { thoiGianApDung = value; }
        }
    }
}
