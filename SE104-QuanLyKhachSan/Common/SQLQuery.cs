namespace SE104_QuanLyKhachSan.Common
{
    public class SQLQuery
    {
        public static string getAllBill = " select                                    " +
                                            "     hd.MaHoaDon                           " +
                                            "     , hd.MaNhanVien                       " +
                                            "     , nv.HoTen                            " +
                                            "     , hd.ThoiGianXuat                     " +
                                            "     , hd.DoiTuongThanhToan                " +
                                            "     , hd.TongSoTien                       " +
                                            " from                                      " +
                                            "     hoadon hd                             " +
                                            "     left join nhanvien nv                 " +
                                            "         on hd.MaNhanVien = nv.MaNhanVien; ";

        public static string getAllDetailBills = " SELECT											    " +
                                                "     cthd.*                                            " +
                                                " FROM                                                  " +
                                                "     chitiethoadon cthd LEFT JOIN phong p              " +
                                                "         ON cthd.MaPhong = p.MaPhong JOIN loaiphong lp " +
                                                "         ON p.MaLoaiPhong = lp.MaLoaiPhong				";
        public static string getAllDetailStaff = " SELECT								    " +
                                                   "     *                                     " +
                                                   " FROM                                      " +
                                                   " nhanvien nv, chucvu cv where nv.MaChucVu = cv.MaChucVu            ";
        public static string getAllDetailRoom = " SELECT								    " +
                                                 "     *                                     " +
                                                 " from                                      " +
                                                 "     phong p                          " +
                                                 "     join loaiphong lp                " +
                                                 "     on lp.MaLoaiPhong = p.MaLoaiPhong; ";
        public static string postNewRoom = "insert into phong(MaPhong, MaLoaiPhong, Tang, TrangThai, GhiChu) values (@MaPhong, @MaLoaiPhong, @Tang, @TrangThai, @GhiChu)";
        public static string postNewStaff = "insert into nhanvien(MaNhanVien,MatKhau,CCCD,HoTen,GioiTinh,NgaySinh,Email,SoDienThoai,NgayVaoLam,MaChucVu,HinhAnh,Luong) values " +
                                              "(@MaNhanVien,@MatKhau,@CCCD,@HoTen,@GioiTinh,@NgaySinh,@Email,@SoDienThoai,@NgayVaoLam,@MaChucVu,@HinhAnh,@Luong)";
        public static string getAllRoomStyle = "select * from loaiphong";
        public static string getAllDetailRoles = "select * from chucvu";
        public static string getChosenStaff = "select * from nhanvien where MaNhanVien = @MaNhanVien";
        public static string deleteRoom = "delete from phong where MaPhong = @MaPhong";
                                                "         ON cthd.MaPhong = p.MaPhong                   ";

        public static string getNV =    " SELECT				 "
                                      + "     *                  "
                                      + " FROM                   "
                                      + "     nhanvien           "
                                      + " WHERE                  "
                                      + "     MaNhanVien = @maNV ";

        public static string getLPByMP =  " SELECT                                      "
                                        + "     lp.*                                    "
                                        + " FROM                                        "
                                        + "     loaiphong lp                            "
                                        + "     LEFT JOIN phong p                       "
                                        + "         ON lp.MaLoaiPhong = p.MaLoaiPhong   "
                                        + " WHERE                                       "
                                        + "     p.MaPhong = @MaPhong            ";

        public static string getDetailById =  " SELECT                   "
                                            + "     *                    "
                                            + " FROM                     "
                                            + "     chitiethoadon ct     "
                                            + " WHERE                    "
                                            + "     MaHoaDon = @maHoaDon ";

        public static string getDetailByDetailId =    " SELECT               "
                                                    + "     *                "
                                                    + " FROM                 "
                                                    + "     chitiethoadon    "
                                                    + " WHERE                "
                                                    + "     MaCTHD = @maCTHD ";

        public static string getListSoLuong =     " SELECT                  "
                                                + "     *                   "
                                                + " FROM                    "
                                                + "     soluongkhachthue    "
                                                + " WHERE                   "
                                                + "     MaCTHD = @maCTHD	";

        public static string getNullDetailByRoomID =  " SELECT                                              "
                                                    + "     *                                               "
                                                    + " FROM                                                "
                                                    + "     chitiethoadon ct                                "
                                                    + " WHERE                                               "
                                                    + "     MaPhong = @maPhong                              "
                                                    + "     AND MaHoaDon IS NULL                            "
                                                    + "     AND TrangThai = 0                               ";

        public static string insertPendingBill =      " INSERT                     "
                                                    + " INTO hoadon(               "
                                                    + "     MaNhanVien             "
                                                    + "     , TongSoTien           "
                                                    + "     , DoiTuongThanhToan    "
                                                    + " )                          "
                                                    + " VALUES (NULL, 0, '') ";

        public static string getHoaDon =  " SELECT                      "
                                        + "     ThoiGianXuat            "
                                        + "     , MaNhanVien            "
                                        + "     , TongSoTien            "
                                        + "     , DoiTuongThanhToan     "
                                        + " FROM                        "
                                        + "     hoadon                  "
                                        + " WHERE                       "
                                        + "     MaHoaDon = @maHoaDon	";

        public static string getPendingBillID =   " SELECT             "
                                                + "     MaHoaDon       "
                                                + " FROM               "
                                                + "     hoadon         "
                                                + " WHERE              "
                                                + "     TongSoTien = 0 ";

        public static string updateBillIdForDetail =  " UPDATE chitiethoadon            "
                                                    + " SET MaHoaDon = @maHoaDon        "
                                                    + "     , TongTienPhong = @tongTien "
                                                    + "     , PhuThuCICO = @phuThuCICO  "
                                                    + "     , TrangThai = 1             "
                                                    + " WHERE MaCTHD = @maCT            ";

        public static string deletePendingBill =  " DELETE              "
                                                + " FROM                "
                                                + "     hoadon          "
                                                + " WHERE               "
                                                + "     TongSoTien = 0; ";

        public static string updateBillInfor =    " UPDATE hoadon                           "
                                                + " SET                                     "
                                                + "     ThoiGianXuat = @thoiGianXuat        "
                                                + "     , MaNhanVien = @maNV                "
                                                + "     , TongSoTien = @tongTien            "
                                                + "     , DoiTuongThanhToan = @dtThanhToan  "
                                                + " WHERE                                   "
                                                + "     MaHoaDon = @id;						";

        public static string getListKhachThue =   " SELECT               "
                                                + "     *                "
                                                + " FROM                 "
                                                + "     khachthue        "
                                                + " WHERE                "
                                                + "     MaCTHD = @maCTHD ";
    
        public static string getCheckInCheckOutDetail =   " SELECT                  "
                                                        + "     ThoiGianNhanPhong   "
                                                        + "     , ThoiGianTraPhong  "
                                                        + " FROM                    "
                                                        + "     chitiethoadon       "
                                                        + " WHERE                   "
                                                        + "     MaCTHD = @maCTHD    ";
        // Field ThoiGianCheckOut cua khach co null
        public static string updateCheckOutKhach =    " UPDATE khachthue                    "
                                                    + " SET                                 "
                                                    + "     ThoiGianCheckOut = @thoiGianCO  "
                                                    + " WHERE                               "
                                                    + "     MaCTHD = @maCTHD                "
                                                    + "     AND ThoiGianCheckOut = NULL		";

        public static string countKhachNN =   " SELECT                                                  "
                                            + "     COUNT(*) AS KhachNN                                 "
                                            + " FROM                                                    "
                                            + "     khachthue                                           "
                                            + " WHERE                                                   "
                                            + "     MaCTHD = @maCTHD                                    "
                                            + "     AND MaLoaiKhachHang = 2                             "
                                            + "     AND (                                               "
                                            + "         CAST(ThoiGianCheckOut AS DATE) >= @thoiGianCO   "
                                            + "         OR ThoiGianCheckOut IS NULL                     "
                                            + "     )                                                   "
                                            + "     AND CAST(ThoiGianCheckIn AS DATE) <= @thoiGianCO	";


        public static string countKhachNoiDia =   " SELECT                                                  "
                                                + "     COUNT(*) AS KhachNoiDia                             "
                                                + " FROM                                                    "
                                                + "     khachthue                                           "
                                                + " WHERE                                                   "
                                                + "     MaCTHD = @maCTHD                                    "
                                                + "     AND MaLoaiKhachHang = 1                             "
                                                + "     AND (                                               "
                                                + "         CAST(ThoiGianCheckOut AS DATE) >= @thoiGianCO   "
                                                + "         OR ThoiGianCheckOut IS NULL                     "
                                                + "     )                                                   "
                                                + "     AND CAST(ThoiGianCheckIn AS DATE) <= @thoiGianCO	";

        public static string getGiaPhongByDetailID =      " SELECT                                                  "
                                                        + "     lp.GiaTienCoBan                                     "
                                                        + " FROM                                                    "
                                                        + "     chitiethoadon ct                                    "
                                                        + "     LEFT JOIN phong p                                   "
                                                        + "         ON ct.MaPhong = p.MaPhong JOIN loaiphong lp     "
                                                        + "         ON p.MaLoaiPhong = lp.MaLoaiPhong               "
                                                        + " WHERE                                                   "
                                                        + "     ct.MaCTHD = @maCTHD 								";

        public static string getPhuThuTheoSoLuong =   " SELECT                                  "
                                                    + "     TiLePhuThu                          "
                                                    + " FROM                                    "
                                                    + "     phuthu                              "
                                                    + " WHERE                                   "
                                                    + "     SoLuongApDung = @soLuong            "
                                                    + "     AND ThoiGianApDung <= @thoiGianCI   "
                                                    + " ORDER BY                                "
                                                    + "     ThoiGianApDung DESC                 "
                                                    + " LIMIT                                   "
                                                    + "     1									";

        public static string getHeSoLKH =     " SELECT                                                  			"
                                            + "     MAX(HeSoPhuThu) AS HeSo                                   		"
                                            + " FROM                                                    			"
                                            + "     phuthulkh                                           			"
                                            + " WHERE                                                   			"
                                            + "     ThoiGianApDung <= @thoiGianCI             						"
                                            + "     AND (                                               			"
                                            + "         (MaLoaiKhachHang = 2 AND SoLuongApDung = @soKhachNN)     	"
                                            + "         OR (MaLoaiKhachHang = 1 AND SoLuongApDung = @soKhachND)  	"
                                            + "     )																";

        public static string insertSLKT =     " INSERT                  "
                                            + " INTO soluongkhachthue(  "
                                            + "     SoKhachThue         "
                                            + "     , SoKhachNN         "
                                            + "     , SoNgayThue        "
                                            + "     , GhiChu            "
                                            + "     , DonGia            "
                                            + "     , PhuThu            "
                                            + "     , HeSoKhach         "
                                            + "     , ThanhTien         "
                                            + "     , MaCTHD            "
                                            + " )                       "
                                            + " VALUES (                "
                                            + "     @soLuongKhach       "
                                            + "     , @soKhachNN        "
                                            + "     , @soNgay           "
                                            + "     , @ghiChu           "
                                            + "     , @donGia           "
                                            + "     , @phuThu           "
                                            + "     , @heSo             "
                                            + "     , @thanhTien        "
                                            + "     , @maCTHD           "
                                            + " );						";

        public static string deleteDetailByID =   " DELETE                  "
                                                + " FROM                    "
                                                + "     chitiethoadon       "
                                                + " WHERE                   "
                                                + "     MaCTHD = @maCTHD   ;";

        public static string updateCancelStatusDetail =   " UPDATE chitiethoadon    "
                                                        + " SET                     "
                                                        + "     TrangThai = 3       "
                                                        + " WHERE                   "
                                                        + "     MaCTHD = @maCTHD   ;";

    }
}
