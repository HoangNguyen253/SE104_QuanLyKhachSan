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
        public static string getAllDetailStaff = " SELECT								    " +
                                                   "     *                                     " +
                                                   " FROM                                      " +
                                                   " nhanvien            ";
        public static string getAllDetailRoom = " SELECT								    " +
                                                 "     *                                     " +
                                                 " from                                      " +
                                                 "     phong p                          " +
                                                 "     join loaiphong lp                " +
                                                 "     on lp.MaLoaiPhong = p.MaLoaiPhong; ";
        public static string postNewRoom = " insert	into phong (maphong, maloaiphong, tang, trangthai, ghichu) values (@MaPhong, @MaLoaiPhong, @Tang, @TrangThai, @GhiChu)";
        public static string getAllRoomStyle = "select * from loaiphong";
    }
}
