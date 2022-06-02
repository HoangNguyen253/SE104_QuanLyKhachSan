using System;

namespace SE104_QuanLyKhachSan.Models
{
    public class ThongKeDoanhThu
    {
        private int _TienThu;
        public int TienThu
        {
            get { return _TienThu; }
               set { _TienThu = value; }
        }

        private int _TienChi;

        public int TienChi
        {
            get { return _TienChi; }
            set { _TienChi = value; }
        }

        private int _LoiNhuan;
        public int LoiNhuan
        {
            get { return _LoiNhuan; }
            set { _LoiNhuan = value; }
        }

        private DateTime _ThangBaoCao;
        public DateTime ThangBaoCao
        {
            get { return _ThangBaoCao; }
            set { _ThangBaoCao = value; }
        }
    }
}
