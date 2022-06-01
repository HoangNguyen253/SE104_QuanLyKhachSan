using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class BaoCaoDoanhThuThang
    {
        private int _MaBCDoanhThu;
        public int MaBCDoanhThu
        {
            get { return _MaBCDoanhThu; }
            set { _MaBCDoanhThu = value; }
        }

        private DateTime _ThangBaoCao;
        public DateTime ThangBaoCao
        {
            get { return _ThangBaoCao; }
            set { _ThangBaoCao = value; }
        }

        private DateTime _ThoiGianLap;


        public DateTime ThoiGianLap
        { 
            get { return _ThoiGianLap; }
            set { _ThoiGianLap = value; }
        }

        private int _TongTien;

        public int TongTien
        {
            get { return _TongTien; }
            set { _TongTien = value;}
        }
    }
}
