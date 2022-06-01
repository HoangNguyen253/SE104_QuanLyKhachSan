namespace SE104_QuanLyKhachSan.Models
{
    public class ChiTietBaoCaoDoanhThuThang
    {
        private int _MaBCDoanhThu;
        public int MaBCDoanhThu
        {
            get { return _MaBCDoanhThu; }
            set { _MaBCDoanhThu = value; }
        }

        private string _TenLoaiPhong;
        public string TenLoaiPhong
        {
            get { return _TenLoaiPhong;}
            set { _TenLoaiPhong = value;}
        }

        private int _MaLoaiPhong;
        public int MaLoaiPhong
        {
            get { return _MaLoaiPhong;}
            set { _MaLoaiPhong = value;}
        }

        private int _SoTien;
        public int SoTien
        {
            get { return _SoTien; }
            set { _SoTien = value; }
        }

        private int _TiLe;
        
        public int TiLe
        {
            get { return _TiLe; }
            set { _TiLe = value; }
        }

       
       
    }
}
