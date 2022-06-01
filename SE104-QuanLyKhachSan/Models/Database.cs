using MySql.Data.MySqlClient;
using SE104_QuanLyKhachSan.Common;
using System;
using System.Collections.Generic;

namespace SE104_QuanLyKhachSan.Models
{
    public class Database
    {
        public string ConnectionString { get; set; }
        public Database()
        {
            this.ConnectionString = "server = 127.0.0.1; user id = root; password =; port = 3307; database = gloriahotel;";
        }
        public Database(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return (new MySqlConnection(ConnectionString));
        }

        public NhanVien GetUser(string userName, string password)
        {
            using (MySqlConnection conncheck = this.GetConnection())
            {
                conncheck.Open();
                string str = "SELECT * " +
                    "FROM NhanVien " +
                    "WHERE (@userName IN (MaNhanVien, CCCD, Email, SoDienThoai)) AND MatKhau = @password;";
                MySqlCommand cmd = new MySqlCommand(str, conncheck);
                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("password", password);
                NhanVien nv = new NhanVien();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        nv.MaNhanVien = reader["MaNhanVien"].ToString();
                        nv.MatKhau = reader["MatKhau"].ToString();
                        nv.CCCD = reader["CCCD"].ToString();
                        nv.HoTen = reader["HoTen"].ToString();
                        nv.GioiTinh = Convert.ToByte(reader["GioiTinh"]);
                        nv.NgaySinh = Convert.ToDateTime(reader["NgaySinh"]);
                        nv.Email = reader["Email"].ToString();
                        nv.SoDienThoai = reader["SoDienThoai"].ToString();
                        nv.NgayVaoLam = Convert.ToDateTime(reader["NgayVaoLam"]);
                        nv.MaChucVu = Convert.ToUInt16(reader["MaChucVu"]);
                        nv.HinhAnh = reader["HinhAnh"].ToString();
                        nv.Luong = Convert.ToInt32(reader["Luong"]);
                        return nv;
                    }
                    else return null;
                }
            }
        }

        public List<HoaDon> getAllBill()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllBill, conn);
                List<HoaDon> bills = new List<HoaDon>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            HoaDon bill = new HoaDon();
                            bill.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                            bill.NV.HoTen = result["HoTen"].ToString();
                            bill.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                            bill.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<ChiTietHoaDon> getAllDetailBills()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailBills, conn);
                List<ChiTietHoaDon> detailBills = new List<ChiTietHoaDon>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietHoaDon detail = new ChiTietHoaDon();
                            detail.MaPhong = result["MaPhong"].ToString();
                            detail.LoaiPhong.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                            detail.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                            detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                            detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                            detailBills.Add(detail);
                        }
                        conn.Close();
                        return detailBills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<BaoCaoDoanhThuThang> getAllBCDTThang()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDoanhThuThang, conn);
                List<BaoCaoDoanhThuThang> bills = new List<BaoCaoDoanhThuThang>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            BaoCaoDoanhThuThang bill = new BaoCaoDoanhThuThang();
                            bill.MaBCDoanhThu = Convert.ToInt32(result["MaBCDoanhThu"]);
                            bill.ThangBaoCao = Convert.ToDateTime(result["ThangBaoCao"]);
                            bill.ThoiGianLap = Convert.ToDateTime(result["ThoiGianLap"]);
                            bill.TongTien = Convert.ToInt32(result["TongTien"]);
                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<ChiTietBaoCaoDoanhThuThang> getDetailBCDTThangbyID(int MaBC)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string str = " SELECT                                    " +
                                            "  bc.MaBCDoanhThu                    " +
                                            " , bc.MaLoaiPhong                     " +
                                            " , lp.TenLoaiPhong  " +
                                            " , bc.SoTien                 " +
                                            " , bc.TiLe" +
                                          
                                            "  FROM                               " +
                                            "     ctbcdoanhthuthang bc, loaiphong lp         " +
                                            "   WHERE bc.MaLoaiPhong = lp.MaLoaiPhong       " +
                                          
                                             " AND bc.MaBCDoanhThu = @mabc ";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("mabc", MaBC);
                List<ChiTietBaoCaoDoanhThuThang> bills = new List<ChiTietBaoCaoDoanhThuThang>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietBaoCaoDoanhThuThang bill = new ChiTietBaoCaoDoanhThuThang();

                            bill.MaBCDoanhThu = Convert.ToInt32(result["MaBCDoanhThu"]);
                            bill.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            bill.TenLoaiPhong = (result["TenLoaiPhong"]).ToString();
                            bill.SoTien = Convert.ToInt32(result["SoTien"]);
                            bill.TiLe = Convert.ToInt32(result["TiLe"]);
                           
                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<ChiTietBaoCaoDoanhThuThang> getDetailBCDTThangbyMonth(string thang)
        {
            DateTime dt = DateTime.ParseExact(thang, "yyyy-MM", null);
            
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string str = " SELECT                                    " +
                                            "  bc.MaBCDoanhThu                    " +
                                            " , bc.MaLoaiPhong                     " +
                                            " , lp.TenLoaiPhong  " +
                                            " , bc.SoTien                 " +
                                            " , bc.TiLe  " +
                                            "   , b.ThangBaoCao                 " +
                                            "  FROM                               " +
                                            "    baocaodoanhthuthang b, ctbcdoanhthuthang bc, loaiphong lp         " +
                                            "   WHERE bc.MaLoaiPhong = lp.MaLoaiPhong       " +
                                            "  AND b.MaBCDoanhThu = bc.MaBCDoanhThu       " +
                                        " AND Month(b.ThangBaoCao) = @thang " +
                                         " AND Year(b.ThangBaoCao) = @nam ";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(str, conn);

                cmd.Parameters.AddWithValue("thang", dt.Month);
                cmd.Parameters.AddWithValue("nam", dt.Year);
                List<ChiTietBaoCaoDoanhThuThang> bills = new List<ChiTietBaoCaoDoanhThuThang>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietBaoCaoDoanhThuThang bill = new ChiTietBaoCaoDoanhThuThang();

                            bill.MaBCDoanhThu = Convert.ToInt32(result["MaBCDoanhThu"]);
                            bill.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            bill.TenLoaiPhong = (result["TenLoaiPhong"]).ToString();
                            bill.SoTien = Convert.ToInt32(result["SoTien"]);
                            bill.TiLe = Convert.ToInt32(result["TiLe"]);
                            
                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<ChiTietDotTraLuong> getDetailDotTraLuongbyID(int MaBC)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string str = " SELECT traluong.MaNhanVien , nhanvien.HoTen , chucvu.TenChucVu, nhanvien.CCCD , `Thuong`, `Phat`, `GhiChu`, `SoTien` ,nhanvien.GioiTinh  FROM `traluong` , nhanvien, chucvu WHERE traluong.MaNhanVien = nhanvien.MaNhanVien AND chucvu.MaChucVu = traluong.MaChucVu AND MaDotTraLuong = @mabc  ";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("mabc", MaBC);
                List<ChiTietDotTraLuong> bills = new List<ChiTietDotTraLuong>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietDotTraLuong bill = new ChiTietDotTraLuong();
                           
                            bill.MaNhanVien = result["MaNhanVien"].ToString();
                            bill.TenNhanVien = result["HoTen"].ToString();
                            bill.TenChucVu = result["TenChucVu"].ToString();
                            bill.CCCD = result["CCCD"].ToString();
                            bill.Thuong = Convert.ToInt32(result["Thuong"]);
                            bill.Phat = Convert.ToInt32(result["Thuong"]);
                            bill.GhiChu = result["GhiChu"].ToString();
                            bill.SoTien = Convert.ToInt32(result["SoTien"]);
                            bill.GioiTinh = Convert.ToInt32(result["GioiTinh"]);
                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<ChiTietDotTraLuong> getDetailDotTraLuongbyMonth(string thang)
        {
            DateTime dt = DateTime.ParseExact(thang, "yyyy-MM", null);

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string str = " SELECT  traluong.MaNhanVien , nhanvien.HoTen , chucvu.TenChucVu, nhanvien.CCCD , `Thuong`, `Phat`, `GhiChu`, `SoTien` ,nhanvien.GioiTinh FROM traluong, nhanvien, chucvu, dottraluong WHERE traluong.MaDotTraLuong = dottraluong.MaDotTraLuong AND nhanvien.MaNhanVien = traluong.MaNhanVien AND traluong.MaChucVu = chucvu.MaChucVu AND Month(dottraluong.NgayTraLuong) = @thang AND YEAR(dottraluong.NgayTraLuong) = @nam ";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", dt.Year);
                cmd.Parameters.AddWithValue("thang", dt.Month);
               
                List<ChiTietDotTraLuong> bills = new List<ChiTietDotTraLuong>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                           
                            ChiTietDotTraLuong bill = new ChiTietDotTraLuong();
                            bill.MaDotTraLuong = Convert.ToInt32(result["MaDotTraLuong"]);
                            bill.MaNhanVien = result["MaNhanVien"].ToString();
                            bill.MaChucVu = Convert.ToInt32(result["MaChucVu"]);
                            bill.Thuong = Convert.ToInt32(result["Thuong"]);
                            bill.Phat = Convert.ToInt32(result["Thuong"]);
                            bill.GhiChu = result["GhiChu"].ToString();
                            bill.SoTien = Convert.ToInt32(result["SoTien"]);


                            bills.Add(bill);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public int XoaBaoCaoDoanhThuThang(int MaBC)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " DELETE FROM ctbcdoanhthuthang WHERE MaBCDoanhThu = @mabcdoanhthu  ;" +
                            " DELETE FROM baocaodoanhthuthang WHERE MaBCDoanhThu = @mabcdoanhthu ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("mabcdoanhthu", MaBC);
                int check = cmd.ExecuteNonQuery();
                conn.Close();
                return check;
            }
        }

        public int XoaBaoCaoDotLuong(int MaBC)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " DELETE FROM `traluong` WHERE traluong.MaDotTraLuong = @mabcdoanhthu  ;" +
                            " DELETE FROM dottraluong WHERE dottraluong.MaDotTraLuong = @mabcdoanhthu ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("mabcdoanhthu", MaBC);
                int check = cmd.ExecuteNonQuery();
                conn.Close();
                return check;
            }
        }

        public int IsBCDTThangExist(DateTime ThangBaoCao)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT * FROM baocaodoanhthuthang bc WHERE Year(bc.ThangBaoCao) = @nam AND Month(bc.ThangBaoCao) = @thang ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month.ToString());
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year.ToString());
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        conn.Close();
                        return 1; //Đã tồn tại
                    }
                    else
                    {
                        conn.Close();
                        return 0;
                    }
                }
                conn.Close();
            }
        }
        public int UpdateCTBCDTThang(DateTime ThangBaoCao, DateTime ThoigianLap)
        {

            if (IsBCDTThangExist(ThangBaoCao) == 1)
                return 0;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " INSERT INTO baocaodoanhthuthang (`ThangBaoCao`, `ThoiGianLap`, `TongTien`) VALUES (@thangbaocao, @ngaylap , 0) ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("thangbaocao", ThangBaoCao);
                cmd.Parameters.AddWithValue("ngaylap", ThoigianLap.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
                conn.Close();
    
            }
        
            int maBC = 0;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {    
                conn.Open();
                string str = " SELECT MaBCDoanhThu FROM `baocaodoanhthuthang` WHERE year(ThangBaoCao) = @nam AND month(ThangBaoCao) = @thang  ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd = new MySqlCommand(str, conn);  
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            maBC = Convert.ToInt32(result["MaBCDoanhThu"]);
                        }
                    }
                }
                conn.Close();
            }

            
            List<ChiTietBaoCaoDoanhThuThang> list_CTBC = new List<ChiTietBaoCaoDoanhThuThang>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT lp.MaLoaiPhong, lp.TenLoaiPhong , SUM(ct.TongTienPhong) AS SoTien FROM phong p, loaiphong lp, chitiethoadon ct, hoadon hd WHERE lp.MaLoaiPhong =p.MaLoaiPhong AND p.MaPhong = ct.MaPhong AND ct.MaHoaDon = hd.MaHoaDon AND Year(hd.ThoiGianXuat)= @nam AND Month(hd.ThoiGianXuat) = @thang GROUP BY lp.MaLoaiPhong ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietBaoCaoDoanhThuThang bill = new ChiTietBaoCaoDoanhThuThang();
                            bill.MaBCDoanhThu = maBC;
                            bill.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            bill.TenLoaiPhong = (result["TenLoaiPhong"]).ToString();
                            bill.SoTien = Convert.ToInt32(result["SoTien"]);
                            bill.TiLe = 0;
                            list_CTBC.Add(bill);
                        }
                    }
                }
                conn.Close();
            }
      

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                int tongTien = 0;
                foreach (var item in list_CTBC)
                {
                    tongTien += item.SoTien;
                }

                foreach (var item in list_CTBC)
                {
                    string str = " INSERT INTO `ctbcdoanhthuthang`(`MaBCDoanhThu`, `MaLoaiPhong`, `SoTien`, `TiLe`) VALUES (@mabcdoanhthu, @malp, @sotien, @tile) ";
                    MySqlCommand cmd = new MySqlCommand(str, conn);
                    cmd.Parameters.AddWithValue("mabcdoanhthu", item.MaBCDoanhThu);
                    cmd.Parameters.AddWithValue("malp", item.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("sotien", item.SoTien);
                    cmd.Parameters.AddWithValue("tile", item.SoTien * 100 / tongTien);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return 1;
            }
            return 0;
        }

        public List<BaoCaoLuongChucVu> getAllBaoCaoLuongChucVu(DateTime dt)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                
                string str = " SELECT cv.TenChucVu , SUM(tl.SoTien) as SoTien FROM traluong tl, dottraluong dtl, chucvu cv WHERE tl.MaDotTraLuong = dtl.MaDotTraLuong AND cv.MaChucVu = tl.MaChucVu AND Year(dtl.NgayTraLuong) = @nam AND Month(dtl.NgayTraLuong) = @thang GROUP BY tl.MaChucVu ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", dt.Year);
                cmd.Parameters.AddWithValue("thang", dt.Month);

                List<BaoCaoLuongChucVu> bills = new List<BaoCaoLuongChucVu>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            BaoCaoLuongChucVu bil = new BaoCaoLuongChucVu();
                            bil.TenChucVu = (result["TenChucVu"]).ToString();
                            bil.TongLuong = Convert.ToInt32(result["SoTien"]);
                            bil.TiLe = 0;
                            bills.Add(bil);
                        }
                        int TongTien = 0;
                        foreach(var item in bills)
                        {
                            TongTien += item.TongLuong;
                        }
                        foreach (var item in bills)
                        {
                            item.TiLe = Math.Round(item.TongLuong * 1.0 * 100 / TongTien,0);
                        }
                        conn.Close();
                        return bills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public List<DotLuong> getAllDotLuong()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT * FROM `dottraluong` ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                List<DotLuong> detailBills = new List<DotLuong>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            DotLuong detail = new DotLuong();
                            detail.MaDotTraLuong = Convert.ToInt32(result["MaDotTraLuong"]);
                            detail.NgayTraLuong = Convert.ToDateTime(result["NgayTraLuong"]);
                            detail.Ngaylap = Convert.ToDateTime(result["ThoiGianLap"]);
                            detailBills.Add(detail);
                        }
                        conn.Close();
                        return detailBills;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public int IsBCDLExist(DateTime ThangBaoCao)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT * FROM dottraluong bc WHERE Year(bc.NgayTraLuong) = @nam AND Month(bc.NgayTraLuong) = @thang ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        conn.Close();
                        return 1; //Đã tồn tại
                    }
                    else
                    {
                        conn.Close();
                        return 0;
                    }
                }
                conn.Close();
            }
        }

        public int UpdateCTDotLuong(DateTime ThangBaoCao, DateTime ThoigianLap)
        {

            if (IsBCDLExist(ThangBaoCao) == 1)
                return 0;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " INSERT INTO `dottraluong`( `NgayTraLuong`, `ThoiGianLap`) VALUES ( @thangbaocao , @ngaylap ) ";
                
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("thangbaocao", ThangBaoCao);
                cmd.Parameters.AddWithValue("ngaylap", ThoigianLap.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
                conn.Close();

            }

            int MaDotTraLuong = 0;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT MaDotTraLuong FROM `dottraluong` WHERE year(NgayTraLuong) = @nam AND month(NgayTraLuong) = @thang  ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            MaDotTraLuong = Convert.ToInt32(result["MaDotTraLuong"]);
                        }
                    }
                }
                conn.Close();
            }


            List<ChiTietDotTraLuong> list_CTBC = new List<ChiTietDotTraLuong>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT nv.MaNhanVien, nv.MaChucVu, nv.Luong FROM nhanvien nv ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChiTietDotTraLuong bill = new ChiTietDotTraLuong();
                            bill.MaNhanVien = result["MaNhanVien"].ToString();
                            bill.MaChucVu = Convert.ToInt32(result["MaChucVu"]);
                            bill.SoTien = Convert.ToInt32(result["Luong"]);
                            list_CTBC.Add(bill);
                        }
                    }
                }
                conn.Close();
            }

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                foreach (var item in list_CTBC)
                {
                    string str = " INSERT INTO `traluong`(`MaDotTraLuong`, `MaNhanVien`, `MaChucVu`, `Thuong`, `Phat`, `GhiChu`, `SoTien`)  " +
                        " VALUES (@madottraluong, @manhanvien, @machucvu, 0, 0 , '', @sotien) ";
                    MySqlCommand cmd = new MySqlCommand(str, conn);
                    /*cmd.Parameters.AddWithValue("mabcdoanhthu", item.MaBCDoanhThu);
                    cmd.Parameters.AddWithValue("malp", item.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("sotien", item.SoTien);
                    cmd.Parameters.AddWithValue("tile", item.SoTien * 100 / tongTien);*/
                    cmd.Parameters.AddWithValue("madottraluong", MaDotTraLuong);
                    cmd.Parameters.AddWithValue("manhanvien", item.MaNhanVien);
                    cmd.Parameters.AddWithValue("machucvu", item.MaChucVu);
                    cmd.Parameters.AddWithValue("sotien", item.SoTien);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return 1;
            }
            return 0;
        }
    }
}
