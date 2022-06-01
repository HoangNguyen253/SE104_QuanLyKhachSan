namespace SE104_QuanLyKhachSan.Common
{
    public class SQLQuery
    {
        public static string getAllBill =   " select                                    " +
                                            "     hd.MaHoaDon                           " +
                                            "     , nv.HoTen                            " +
                                            "     , hd.ThoiGianXuat                     " +
                                            "     , hd.DoiTuongThanhToan                " +
                                            "     , hd.TongSoTien                       " +
                                            " from                                      " +
                                            "     hoadon hd                             " +
                                            "     left join nhanvien nv                 " +
                                            "         on hd.MaNhanVien = nv.MaNhanVien; ";

        public static string getAllDetailBills = " SELECT											    " +
                                                "     cthd.MaPhong                                      " +
                                                "     , lp.TenLoaiPhong                                 " +
                                                "     , ThoiGianNhanPhong                               " +
                                                "     , ThoiGianTraPhong                                " +
                                                "     , cthd.TrangThai                                  " +
                                                " FROM                                                  " +
                                                "     chitiethoadon cthd LEFT JOIN phong p              " +
                                                "         ON cthd.MaPhong = p.MaPhong JOIN loaiphong lp " +
                                                "         ON p.MaLoaiPhong = lp.MaLoaiPhong				";
        public static string getAllDoanhThuThang = " SELECT                                    " +
                                            "  bc.MaBCDoanhThu                    " +
                                            "  , bc.ThangBaoCao                    " +
                                            " , bc.ThoiGianLap                    " +
                                            " , bc.TongTien                    " +
                                            "   FROM                               " +
                                            "     baocaodoanhthuthang bc         ";
        public static string getAllDetailDoanhThuThang = " SELECT                                    " +
                                            " bc.MaBCDoanhThu                    " +
                                            " bc.MaLoaiPhong                     " +
                                            " lc.TenLoaiPhong                   " + 
                                            " ,bc.ThoiGianLap                    " +
                                            "  FROM                               " +
                                            "     ctbcdoanhthuthang bc, loaiphong lp         " +
                                            "   where bc.MaLoaiPhong = lp.MaLoaiPhong       ";
    }
}
