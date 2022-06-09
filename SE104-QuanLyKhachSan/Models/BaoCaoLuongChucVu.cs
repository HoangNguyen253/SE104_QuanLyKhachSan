namespace SE104_QuanLyKhachSan.Models
{
    public class BaoCaoLuongChucVu
    {
        private int _MaChucVu;

        public int MaChucVu
        {
            get { return _MaChucVu; }
            set { _MaChucVu = value;}
        }

        private string _TenChucVu;
        public string TenChucVu
        {
            get { return _TenChucVu; }
            set { _TenChucVu = value; }    
        }

        private int _TongLuong;
        public int TongLuong
        {
            get { return _TongLuong;}   
            set { _TongLuong = value;}
        }

        private double _TiLe;
        public double TiLe
        {
            get { return _TiLe; }
            set { _TiLe = value; }
        }
    }
}
