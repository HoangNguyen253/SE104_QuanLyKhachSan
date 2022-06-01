namespace SE104_QuanLyKhachSan.Models
{
    public class BaoCaoLuongChucVu
    {
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
