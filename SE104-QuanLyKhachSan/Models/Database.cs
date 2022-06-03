using MySql.Data.MySqlClient;
using SE104_QuanLyKhachSan.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SE104_QuanLyKhachSan.Models
{
    public class Database
    {
        #region general
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
        #endregion

        #region Vinh
        //Vinh - begin

        public List<object> LoadDataForSoDoPhong()
        {
            List<object> dsPhong = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT P.MaPhong, LP.TenLoaiPhong, P.TrangThai, P.Tang, MAX(ThoiGianNhanPhong) AS 'ThoiGianNhanPhong' " +
                                "FROM PHONG P INNER JOIN LOAIPHONG LP ON P.MaLoaiPhong = LP.MaLoaiPhong LEFT JOIN CHITIETHOADON CTHD ON P.MaPhong = CTHD.MaPhong " +
                                "GROUP BY P.MaPhong, LP.TenLoaiPhong, P.TrangThai, P.Tang " +
                                "ORDER BY P.Tang, P.MaPhong";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader readerResult = cmd.ExecuteReader())
                {
                    string thoigiannhanphong_res = "";
                    while (readerResult.Read())
                    {
                        thoigiannhanphong_res = readerResult["ThoiGianNhanPhong"].ToString();
                        dsPhong.Add(new
                        {
                            maPhong = readerResult["MaPhong"].ToString(),
                            tenLoaiPhong = readerResult["TenLoaiPhong"].ToString(),
                            trangThai = Convert.ToInt32(readerResult["TrangThai"]),
                            tang = Convert.ToInt32(readerResult["Tang"]),
                            thoiGianNhanPhong = thoigiannhanphong_res != "" ? Convert.ToDateTime(thoigiannhanphong_res).ToString("MM-dd-yyyy") : ""
                        });
                    }
                }
                return dsPhong;
            }
        }

        public List<object> LoadDataForSoDoPhongByFilter(int tang, int trangThai, string soPhong)
        {
            List<object> dsPhong = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT P.MaPhong, LP.TenLoaiPhong, P.TrangThai, P.Tang, MAX(ThoiGianNhanPhong) AS 'ThoiGianNhanPhong' " +
                                "FROM PHONG P INNER JOIN LOAIPHONG LP ON P.MaLoaiPhong = LP.MaLoaiPhong LEFT JOIN CHITIETHOADON CTHD ON P.MaPhong = CTHD.MaPhong " +
                                "WHERE P.Tang = @tang AND P.TrangThai = @trangThai AND RIGHT(P.MaPhong, 2) = @soPhong " +
                                "GROUP BY P.MaPhong, LP.TenLoaiPhong, P.TrangThai, P.Tang " +
                                "ORDER BY P.Tang, P.MaPhong";
                if (tang == 0)
                {
                    query = query.Replace("P.Tang = @tang AND ", "");
                }
                if (trangThai == -1)
                {
                    query = query.Replace("P.TrangThai = @trangThai AND ", "");
                }
                if (soPhong == null)
                {
                    query = query.Replace("AND RIGHT(P.MaPhong, 2) = @soPhong", "");
                }
                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (tang != 0)
                {
                    cmd.Parameters.AddWithValue("tang", tang);
                }
                if (trangThai != -1)
                {
                    cmd.Parameters.AddWithValue("trangThai", trangThai);
                }
                if (soPhong != null)
                {
                    cmd.Parameters.AddWithValue("soPhong", soPhong);
                }
                using (MySqlDataReader readerResult = cmd.ExecuteReader())
                {
                    string thoigiannhanphong_res = "";
                    while (readerResult.Read())
                    {
                        thoigiannhanphong_res = readerResult["ThoiGianNhanPhong"].ToString();
                        dsPhong.Add(new
                        {
                            maPhong = readerResult["MaPhong"].ToString(),
                            tenLoaiPhong = readerResult["TenLoaiPhong"].ToString(),
                            trangThai = Convert.ToInt32(readerResult["TrangThai"]),
                            tang = Convert.ToInt32(readerResult["Tang"]),
                            thoiGianNhanPhong = thoigiannhanphong_res != "" ? Convert.ToDateTime(thoigiannhanphong_res).ToString("MM-dd-yyyy") : ""
                        });
                    }
                }
                return dsPhong;
            }
        }

        public bool UpdateStatusForRoom(string maPhong, int trangThai)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "UPDATE PHONG " +
                    "SET TrangThai = @trangthai " +
                    "WHERE MaPhong = @maphong";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("maphong", maPhong);
                cmd.Parameters.AddWithValue("trangthai", trangThai);
                int row_Affected = cmd.ExecuteNonQuery();
                if (row_Affected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool UpdateStatusForCTHD(string maPhong, int trangThai)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                DateTime checkOutTime = DateTime.Now;
                string query = "UPDATE CHITIETHOADON " +
                    "SET TrangThai = @trangthai, ThoiGianTraPhong = @checkOutTime " +
                    "WHERE MaPhong = @maphong AND MaHoaDon is null AND TrangThai = 0";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("maphong", maPhong);
                cmd.Parameters.AddWithValue("trangthai", trangThai);
                cmd.Parameters.AddWithValue("checkOutTime", checkOutTime);
                int row_Affected = cmd.ExecuteNonQuery();
                if (row_Affected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public object GetInfoRoomToOrder(string maPhong)
        {
            object infoRoom;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT P.MaPhong, RIGHT(P.MaPhong,2) AS SoPhong, LP.TenLoaiPhong, P.Tang, LP.GiaTienCoBan AS GiaPhong " +
                    "FROM PHONG P, LOAIPHONG LP " +
                    "WHERE P.MaLoaiPhong = LP.MaLoaiPhong AND P.MaPhong = @maphong";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("maphong", maPhong);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        infoRoom = new
                        {
                            maPhong = reader["MaPhong"].ToString(),
                            soPhong = reader["SoPhong"].ToString(),
                            tenLoaiPhong = reader["TenLoaiPhong"].ToString(),
                            tang = Convert.ToInt32(reader["Tang"]),
                            giaPhong = Convert.ToInt32(reader["GiaPhong"])
                        };
                    }
                    else infoRoom = null;
                }
            }
            return infoRoom;
        }

        public List<object> GetInfoOrderedRoom(string maPhong)
        {
            List<object> infoOrderdRoom = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT P.MaPhong, RIGHT(P.MaPhong,2) AS SoPhong, LP.TenLoaiPhong, P.Tang, LP.GiaTienCoBan AS GiaPhong, CTHD.ThoiGianNhanPhong AS ThoiGianNhanPhong " +
                    "FROM PHONG P, LOAIPHONG LP, CHITIETHOADON CTHD " +
                    "WHERE P.MaLoaiPhong = LP.MaLoaiPhong AND P.MaPhong = @maphong AND CTHD.MaPhong = @maphong AND CTHD.TrangThai = 0 AND CTHD.MaHoaDon IS NULL ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("maphong", maPhong);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        infoOrderdRoom.Add(new
                        {
                            maPhong = reader["MaPhong"].ToString(),
                            soPhong = reader["SoPhong"].ToString(),
                            tenLoaiPhong = reader["TenLoaiPhong"].ToString(),
                            tang = Convert.ToInt32(reader["Tang"]),
                            giaPhong = Convert.ToInt32(reader["GiaPhong"]),
                            thoiGianNhanPhong = Convert.ToDateTime(reader["ThoiGianNhanPhong"])
                        });
                    }
                    else infoOrderdRoom = null;
                    reader.Close();
                }
                if (infoOrderdRoom != null)
                {
                    string query2 = "SELECT KT.CCCD, KT.HoTen, KT.ThoiGianCheckIn, KT.ThoiGianCheckOut, LKH.TenLoaiKhachHang, LKH.MaLoaiKhachHang, KT.DiaChi, KT.MaKhachThue " +
                        "FROM KHACHTHUE KT, CHITIETHOADON CTHD, LOAIKHACHHANG LKH " +
                        "WHERE KT.MaLoaiKhachHang = LKH.MaLoaiKhachHang AND CTHD.MaPhong = @maphong AND CTHD.MaHoaDon IS NULL AND CTHD.TrangThai = 0 AND KT.MaCTHD = CTHD.MaCTHD " +
                        "ORDER BY (KT.ThoiGianCheckOut) ASC, (KT.ThoiGianCheckIn) ASC";
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("maphong", maPhong);
                    using (MySqlDataReader reader2 = cmd2.ExecuteReader())
                    {
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                infoOrderdRoom.Add(new
                                {
                                    CCCD = reader2["CCCD"].ToString(),
                                    hoTen = reader2["HoTen"].ToString(),
                                    thoiGianCheckIn = Convert.ToDateTime(reader2["ThoiGianCheckIn"]),
                                    thoiGianCheckOut = (reader2["ThoiGianCheckOut"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(reader2["ThoiGianCheckOut"]),
                                    tenLoaiKhachHang = reader2["TenLoaiKhachHang"].ToString(),
                                    maLoaiKhachHang = Convert.ToInt32(reader2["MaLoaiKhachHang"]),
                                    diaChi = reader2["DiaChi"].ToString(),
                                    maKhachThue = Convert.ToInt32(reader2["MaKhachThue"])
                                });
                            }
                        }
                        reader2.Close();
                    }
                }
            }
            return infoOrderdRoom;
        }

        public List<object> GetLoaiKhachHangs()
        {
            List<object> dsLoaiKhachHang = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT MaLoaiKhachHang, TenLoaiKhachHang " +
                    "FROM LOAIKHACHHANG " +
                    "WHERE DaXoa = 0 ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dsLoaiKhachHang.Add(new
                            {
                                maLoaiKhachHang = reader["MaLoaiKhachHang"],
                                tenLoaiKhachHang = reader["TenLoaiKhachHang"]
                            });
                        }
                    }
                }
            }
            return dsLoaiKhachHang;
        }

        public string DatPhongFromSDP(string maPhong, DateTime thoiGianNhanPhong, string dsKhachThue)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string procedureName = "DatPhong";
                MySqlCommand cmd = new MySqlCommand(procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("_maPhong", maPhong);
                cmd.Parameters["_maPhong"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("_thoiGianNhanPhong", thoiGianNhanPhong);
                cmd.Parameters["_thoiGianNhanPhong"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("_thongTinKhachThue", dsKhachThue);
                cmd.Parameters["_thongTinKhachThue"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("_result", MySqlDbType.Int32);
                cmd.Parameters["_result"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                conn.Close();
                return (Convert.ToInt32(cmd.Parameters["_result"].Value) != -1) ? "success" : "fail";
            }
        }

        public DateTime CheckoutForKhachThue(int maKhachThue)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                DateTime checkOutTime = DateTime.Now;
                string query = "UPDATE KhachThue " +
                    "SET ThoiGianCheckOut = @checkOutTime " +
                    "WHERE MaKhachThue = @maKhachThue";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("checkOutTime", checkOutTime);
                cmd.Parameters.AddWithValue("maKhachThue", maKhachThue);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return checkOutTime;
                }
                else return (DateTime)(DateTime?)null;
            }
        }

        public string ThayDoiKhachOFromSDP(string maPhong, string dsKhachThue)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string procedureName = "ThayDoiKhachO";
                MySqlCommand cmd = new MySqlCommand(procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("_maPhong", maPhong);
                cmd.Parameters["_maPhong"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("_thongTinKhachThue", dsKhachThue);
                cmd.Parameters["_thongTinKhachThue"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("_result", MySqlDbType.Int32);
                cmd.Parameters["_result"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                conn.Close();
                return (Convert.ToInt32(cmd.Parameters["_result"].Value) != -1) ? "success" : "fail";
            }
        }
        public void UpdateImage(string path, string maNhanVien)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "UPDATE NhanVien " +
                    "SET HinhAnh=@path " +
                    "WHERE MaNhanVien=@maNhanVien;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("maNhanVien", maNhanVien);
                cmd.Parameters.AddWithValue("path", path);
                cmd.ExecuteNonQuery();
            }
        }
        public string UpdateInfoStaff(NhanVien nv)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "UPDATE NhanVien " +
                    "SET CCCD=@cccd," +
                        "HoTen=@hoTen, " +
                        "SoDienThoai = @sdt, " +
                        "NgaySinh = @ngaySinh, " +
                        "Email = @email, " +
                        "GioiTinh = @gioiTinh " +
                    "WHERE MaNhanVien=@maNhanVien;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("maNhanVien", nv.MaNhanVien);
                cmd.Parameters.AddWithValue("cccd", nv.CCCD);
                cmd.Parameters.AddWithValue("hoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("sdt", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("ngaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("email", nv.Email);
                cmd.Parameters.AddWithValue("gioiTinh", nv.GioiTinh);
                int result = cmd.ExecuteNonQuery();
                return (result >= 1) ? "success" : "fail";
            }
        }
        public string ChangePassStaff(string maNhanVien, string pass)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "UPDATE NhanVien " +
                    "SET MatKhau=@pass " +
                    "WHERE MaNhanVien=@maNhanVien;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("maNhanVien", maNhanVien);
                cmd.Parameters.AddWithValue("pass", pass);
                int result = cmd.ExecuteNonQuery();
                return (result >= 1) ? "success" : "fail";
            }
        }

        //Vinh - end
        #endregion

        #region Nguyên
        //Nguyên - begin
        public List<LoaiPhong> GetLoaiPhong()
        {
            using (MySqlConnection conncheck = this.GetConnection())
            {
                conncheck.Open();

                string str = "SELECT * " +
                    "FROM LoaiPhong;";
                MySqlCommand cmd = new MySqlCommand(str, conncheck);
                List<LoaiPhong> loaiPhongs = new List<LoaiPhong>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            byte kt = Convert.ToByte(reader["DaXoa"]);
                            if (kt == 1) continue;
                            LoaiPhong lp = new LoaiPhong();
                            lp.MaLoaiPhong = Convert.ToInt32(reader["MaLoaiPhong"]);
                            lp.TenLoaiPhong = reader["TenLoaiPhong"].ToString();
                            lp.GiaTienCoBan = Convert.ToInt32(reader["GiaTienCoBan"]);
                            loaiPhongs.Add(lp);
                        }
                        return loaiPhongs;
                    }
                    else return null;
                }
            }
        }

        public bool DeleteLoaiPhong(int maLoaiPhong)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE LOAIPHONG " +
                    "SET DaXoa=1 " +
                    "WHERE MaLoaiPhong=@MaLoaiPhong";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("MaLoaiPhong", maLoaiPhong);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdateLoaiPhong(int maLoaiPhong, string tenLoaiPhong, int giaTienCoBan)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE LOAIPHONG " +
                    "SET TenLoaiPhong=@tenLoaiPhong, GiaTienCoBan=@giaTienCoBan " +
                    "WHERE MaLoaiPhong=@maLoaiPhong AND NOT EXISTS(SELECT * " +
                                                                  "FROM loaiphong " +
                                                                  "WHERE TenLoaiPhong=@tenLoaiPhong " +
                                                                  "AND GiaTienCoBan=@giaTienCoBan " +
                                                                  "AND DaXoa = 0) ;";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maLoaiPhong", maLoaiPhong);
                cmd.Parameters.AddWithValue("tenLoaiPhong", tenLoaiPhong);
                cmd.Parameters.AddWithValue("giaTienCoBan", giaTienCoBan);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool InsertLoaiPhong(string tenLoaiPhong, int giaTienCoBan)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO LOAIPHONG(TenLoaiPhong, GiaTienCoBan, DaXoa) " +
                                        "SELECT @tenLoaiPhong, @giaTienCoBan, 0 " +
                                        "WHERE NOT EXISTS(SELECT * " +
                                                                  "FROM loaiphong " +
                                                                  "WHERE TenLoaiPhong=@tenLoaiPhong " +
                                                                  "AND DaXoa=0) ;";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("tenLoaiPhong", tenLoaiPhong);
                cmd.Parameters.AddWithValue("giaTienCoBan", giaTienCoBan);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public List<LoaiKhachHang> GetLoaiKhachHang()
        {
            using (MySqlConnection conncheck = this.GetConnection())
            {
                conncheck.Open();

                string str = "SELECT * " +
                    "FROM LoaiKhachHang;";
                MySqlCommand cmd = new MySqlCommand(str, conncheck);
                List<LoaiKhachHang> loaiKhachHangs = new List<LoaiKhachHang>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            byte kt = Convert.ToByte(reader["DaXoa"]);
                            if (kt == 1) continue;
                            LoaiKhachHang lkh = new LoaiKhachHang();
                            lkh.MaLoaiKhachHang = Convert.ToInt32(reader["MaLoaiKhachHang"]);
                            lkh.TenLoaiKhachHang = reader["TenLoaiKhachHang"].ToString();
                            loaiKhachHangs.Add(lkh);
                        }
                        return loaiKhachHangs;
                    }
                    else return null;
                }
            }
        }

        public bool DeleteLoaiKhachHang(int maLoaiKhachHang)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE LOAIKHACHHANG " +
                    "SET DaXoa=1 " +
                    "WHERE MaLoaiKhachHang=@maLoaiKhachHang";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maLoaiKhachHang", maLoaiKhachHang);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool InsertLoaiKhachHang(string tenLoaiKhachHang)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO LOAIKHACHHANG(TenLoaiKhachHang, DaXoa) " +
                                    "SELECT @tenLoaiKhachHang, 0 " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM loaikhachhang " +
                                                    "WHERE TenLoaiKhachHang=@tenLoaiKhachHang " +
                                                        "AND DaXoa = 0)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("tenLoaiKhachHang", tenLoaiKhachHang);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdateLoaiKhachHang(int maLoaiKhachHang, string tenLoaiKhachHang)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE LOAIKHACHHANG " +
                    "SET TenLoaiKhachHang=@tenLoaiKhachHang " +
                    "WHERE MaLoaiKhachHang=@maLoaiKhachHang AND NOT EXISTS(SELECT * " +
                                                    "FROM loaikhachhang " +
                                                    "WHERE TenLoaiKhachHang=@tenLoaiKhachHang " +
                                                    "AND DaXoa = 0)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maLoaiKhachHang", maLoaiKhachHang);
                cmd.Parameters.AddWithValue("tenLoaiKhachHang", tenLoaiKhachHang);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public int GetSoKhachToiDa()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT GiaTri " +
                    "FROM THAMSO " +
                    "WHERE TenThuocTinh='SoKhachToiDa'";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return Convert.ToInt32(reader["GiaTri"]);
                    }
                    else return -1;
                }
            }
        }

        public int UpdateSoKhachToiDa(int soKhachToiDa)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE THAMSO " +
                    "SET GiaTri=@soKhachToiDa " +
                    "WHERE TenThuocTinh='SoKhachToiDa' " +
                            "AND NOT EXISTS (SELECT * " +
                                    "FROM PHUTHU PT1 " +
                                    "WHERE PT1.MaLoaiPhuThu = 1 AND PT1.SoLuongApDung>@soKhachToiDa " +
                                        "AND ((PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                        "FROM PHUTHU PT2 " +
                                                                        "WHERE PT2.MaLoaiPhuThu = 1 AND PT1.SoLuongApDung = PT2.SoLuongApDung AND PT2.ThoiGianApDung <= @now) " +
                                               "AND PT1.ThoiGianApDung <= @now) " +
                                             "OR PT1.ThoiGianApDung > @now)) ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soKhachToiDa", soKhachToiDa);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return 1;
                }
                return 2;
            }
        }

        public List<PhuThu> GetPhuThuSoKhach()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT * " +
                                    "FROM PHUTHU PT1 " +
                                    "WHERE PT1.MaLoaiPhuThu = 1 " +
                                        "AND ((PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                        "FROM PHUTHU PT2 " +
                                                                        "WHERE PT2.MaLoaiPhuThu = 1 AND PT1.SoLuongApDung = PT2.SoLuongApDung AND PT2.ThoiGianApDung <= @now) " +
                                               "AND PT1.ThoiGianApDung <= @now) " +
                                             "OR PT1.ThoiGianApDung > @now) " +
                                    "ORDER BY `PT1`.`SoLuongApDung` ASC, `PT1`.`ThoiGianApDung` ASC";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                List<PhuThu> phuThus = new List<PhuThu>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int check = Convert.ToInt32(reader["TiLePhuThu"]);
                            if (check == 0) continue;
                            PhuThu pt = new PhuThu();
                            pt.MaLoaiPhuThu = Convert.ToInt32(reader["MaLoaiPhuThu"]);
                            pt.SoLuongApDung = Convert.ToInt32(reader["SoLuongApDung"]);
                            pt.TiLePhuThu = Convert.ToInt32(reader["TiLePhuThu"]);
                            pt.MaPhuThu = Convert.ToInt32(reader["MaPhuThu"]);
                            pt.ThoiGianApDung = Convert.ToDateTime(reader["ThoiGianApDung"]).ToString("yyyy-MM-ddTHH:mm:ss");
                            phuThus.Add(pt);
                        }
                        return phuThus;
                    }
                    else return null;
                }
            }
        }

        public bool DeletePhuThuSoKhach(int soKhachApDung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO PHUTHU(SoLuongApDung, TiLePhuThu, MaLoaiPhuThu, ThoiGianApDung) VALUES (@soKhachApDung, @tiLePhuThu, 1, @thoiGianApDung);";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soKhachApDung", soKhachApDung);
                cmd.Parameters.AddWithValue("tiLePhuThu", 0);
                cmd.Parameters.AddWithValue("thoiGianApDung", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool DeletePhuThuSoKhachFuture(int maPhuThuSoKhach)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "DELETE FROM PHUTHU WHERE MaPhuThu=@maPhuThuSoKhach";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maPhuThuSoKhach", maPhuThuSoKhach);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdatePhuThuSoKhach(int soKhachApDung, int tiLePhuThu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO PHUTHU(SoLuongApDung, TiLePhuThu, MaLoaiPhuThu, ThoiGianApDung) " +
                                    "SELECT @soKhachApDung, @tiLePhuThu, 1, @thoiGianApDung " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM phuthu " +
                                                    "WHERE SoLuongApDung=@soKhachApDung " +
                                                        "AND TiLePhuThu=@tiLePhuThu " +
                                                        "AND MaLoaiPhuThu=1 " +
                                                        "AND ThoiGianApDung=@thoiGianApDung)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soKhachApDung", soKhachApDung);
                cmd.Parameters.AddWithValue("tiLePhuThu", tiLePhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdatePhuThuSoKhachFuture(int soKhachApDung, int tiLePhuThu, DateTime thoiGianApDung, int maPhuThuSoKhach)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE PHUTHU " +
                                    "SET SoLuongApDung=@soKhachApDung, " +
                                        "TiLePhuThu=@tiLePhuThu, " +
                                        "ThoiGianApDung=@thoiGianApDung " +
                                    "WHERE MaPhuThu=@maPhuThuSoKhach AND NOT EXISTS(SELECT * " +
                                                                                    "FROM PHUTHU PT1 " +
                                                                                    "WHERE PT1.SoLuongApDung=@soKhachApDung " +
                                                                                        "AND PT1.TiLePhuThu=@tiLePhuThu " +
                                                                                        "AND PT1.ThoiGianApDung=@thoiGianApDung)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maPhuThuSoKhach", maPhuThuSoKhach);
                cmd.Parameters.AddWithValue("soKhachApDung", soKhachApDung);
                cmd.Parameters.AddWithValue("tiLePhuThu", tiLePhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", thoiGianApDung);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool InsertPhuThuSoKhach(int soLuongApDung, int tiLePhuThu, DateTime thoiGianApDung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO PHUTHU(SoLuongApDung, TiLePhuThu, MaLoaiPhuThu, ThoiGianApDung) " +
                                    "SELECT @soLuongApDung, @tiLePhuThu, 1, @thoiGianApDung " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM PHUTHU PT1 " +
                                                    "WHERE PT1.MaLoaiPhuThu = 1 AND PT1.SoLuongApDung = @soLuongApDung " +
                                                        "AND ((PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                                        "FROM PHUTHU PT2 " +
                                                                                        "WHERE PT2.MaLoaiPhuThu = 1 " +
                                                                                            "AND PT1.SoLuongApDung = PT2.SoLuongApDung " +
                                                                                            "AND PT2.ThoiGianApDung <= @now) " +
                                                                  "AND PT1.ThoiGianApDung <= @now AND PT1.TiLePhuThu <> 0 AND @thoiGianApDung <= @now) " +
                                                              "OR (PT1.ThoiGianApDung > @now " +
                                                                  "AND @thoiGianApDung > @now " +
                                                                  "AND TiLePhuThu= @tiLePhuThu " +
                                                                  "AND ThoiGianApDung=@thoiGianApDung))) ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("tiLePhuThu", tiLePhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", thoiGianApDung.ToString("yyyy-MM-dd HH:mm:ss"));
                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public List<PhuThuLKH> GetPhuThuLoaiKhachHang()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT MaPhuThuLKH, " +
                                            "SoLuongApDung, " +
                                            "HeSoPhuThu, " +
                                            "ThoiGianApDung, " +
                                            "PT1.MaLoaiKhachHang, " +
                                            "LKH.TenLoaiKhachHang " +
                                     "FROM phuthulkh PT1 INNER JOIN loaikhachhang LKH ON (PT1.MaLoaiKhachHang = LKH.MaLoaiKhachHang AND LKH.DaXoa = 0) " +
                                     "WHERE (PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                        "FROM phuthulkh PT2 " +
                                                                        "WHERE PT1.SoLuongApDung = PT2.SoLuongApDung " +
                                                                            "AND PT1.MaLoaiKhachHang = PT2.MaLoaiKhachHang " +
                                                                            "AND PT2.ThoiGianApDung <= @now) " +
                                                "AND PT1.ThoiGianApDung <= @now) " +
                                            "OR PT1.ThoiGianApDung > @now " +
                                     "ORDER BY `PT1`.`MaLoaiKhachHang` ASC, `PT1`.`SoLuongApDung` ASC, `PT1`.`ThoiGianApDung` ASC";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                List<PhuThuLKH> phuThuLKHs = new List<PhuThuLKH>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int check = Convert.ToInt32(reader["HeSoPhuThu"]);
                            if (check == 0) continue;
                            PhuThuLKH ptlkh = new PhuThuLKH();
                            ptlkh.MaPhuThuLKH = Convert.ToInt32(reader["MaPhuThuLKH"]);
                            ptlkh.TenLoaiKhachHang = reader["TenLoaiKhachHang"].ToString();
                            ptlkh.HeSoPhuThu = (double)System.Math.Round(Convert.ToDouble(reader["HeSoPhuThu"]), 1);
                            ptlkh.MaLoaiKhachHang = Convert.ToInt32(reader["MaLoaiKhachHang"]);
                            ptlkh.ThoiGianApDung = Convert.ToDateTime(reader["ThoiGianApDung"]).ToString("yyyy-MM-ddTHH:mm:ss");
                            ptlkh.SoLuongApDung = Convert.ToInt32(reader["SoLuongApDung"]);
                            phuThuLKHs.Add(ptlkh);
                        }
                        return phuThuLKHs;
                    }
                    else return null;
                }
            }
        }

        public bool DeleteHeSoPhuThu(int maLoaiKhachHang, int soLuongApDung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO `phuthulkh` (`SoLuongApDung`, `HeSoPhuThu`, `ThoiGianApDung`, `MaLoaiKhachHang`) " +
                                    "VALUES (@soLuongApDung, @heSoPhuThu, @thoiGianApDung, @maLoaiKhachHang);";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("maLoaiKhachHang", maLoaiKhachHang);
                cmd.Parameters.AddWithValue("heSoPhuThu", 0);
                cmd.Parameters.AddWithValue("thoiGianApDung", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool DeleteHeSoPhuThuFuture(int maPhuThuLKH)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "DELETE FROM phuthulkh WHERE MaPhuThuLKH=@maPhuThuLKH";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maPhuThuLKH", maPhuThuLKH);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdateHeSoPhuThu(int maLoaiKhachHang, int soLuongApDung, double heSoPhuThu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO `phuthulkh` (`SoLuongApDung`, `HeSoPhuThu`, `ThoiGianApDung`, `MaLoaiKhachHang`) " +
                                    "SELECT @soLuongApDung, @heSoPhuThu, @thoiGianApDung, @maLoaiKhachHang " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM phuthulkh " +
                                                    "WHERE SoLuongApDung=@soLuongApDung " +
                                                        "AND HeSoPhuThu=@heSoPhuThu " +
                                                        "AND ThoiGianApDung=@thoiGianApDung " +
                                                        "AND MaLoaiKhachHang=@maLoaiKhachHang)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("heSoPhuThu", heSoPhuThu);
                cmd.Parameters.AddWithValue("maLoaiKhachHang", maLoaiKhachHang);
                cmd.Parameters.AddWithValue("thoiGianApDung", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdateHeSoPhuThuFuture(int maLoaiKhachHang, int soLuongApDung, double heSoPhuThu, DateTime thoiGianApDung, int maHeSoPhuThu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE PHUTHULKH " +
                                    "SET SoLuongApDung=@soLuongApDung, " +
                                        "MaLoaiKhachHang=@maLoaiKhachHang, " +
                                        "HeSoPhuThu=@heSoPhuThu, " +
                                        "ThoiGianApDung=@thoiGianApDung " +
                                    "WHERE MaPhuThuLKH = @maHeSoPhuThu AND NOT EXISTS(SELECT * " +
                                                                                    "FROM PHUTHULKH PT1 " +
                                                                                    "WHERE SoLuongApDung=@soLuongApDung " +
                                                                                    "AND PT1.MaLoaiKhachHang=@maLoaiKhachHang " +
                                                                                    "AND PT1.HeSoPhuThu=@heSoPhuThu " +
                                                                                    "AND PT1.ThoiGianApDung=@thoiGianApDung)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maHeSoPhuThu", maHeSoPhuThu);
                cmd.Parameters.AddWithValue("maLoaiKhachHang", maLoaiKhachHang);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("heSoPhuThu", heSoPhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", thoiGianApDung.ToString("yyyy-MM-dd HH:mm:ss"));

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool InsertHeSoPhuThu(int maLoaiKhachHang, int soLuongApDung, double heSoPhuThu, DateTime thoiGianApDung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO `phuthulkh` (`SoLuongApDung`, `HeSoPhuThu`, `ThoiGianApDung`, `MaLoaiKhachHang`) " +
                                    "SELECT @soLuongApDung, @heSoPhuThu, @thoiGianApDung, @maLoaiKhachHang " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                     "FROM phuthulkh PT1 INNER JOIN loaikhachhang LKH ON (PT1.MaLoaiKhachHang = LKH.MaLoaiKhachHang AND LKH.DaXoa = 0) " +
                                                     "WHERE SoLuongApDung = @soLuongApDung AND PT1.MaLoaiKhachHang = @maLoaiKhachHang " +
                                                         "AND ((PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                                            "FROM phuthulkh PT2 " +
                                                                                            "WHERE PT1.SoLuongApDung = PT2.SoLuongApDung " +
                                                                                                "AND PT1.MaLoaiKhachHang = PT2.MaLoaiKhachHang " +
                                                                                                "AND PT2.ThoiGianApDung <= @now) " +
                                                                    "AND PT1.ThoiGianApDung <= @now AND HeSoPhuThu <> 0 AND @thoiGianApDung <= @now ) " +
                                                                "OR (PT1.ThoiGianApDung > @now " +
                                                                    "AND PT1.ThoiGianApDung=@thoiGianApDung " +
                                                                    "AND @thoiGianApDung > @now " +
                                                                    "AND PT1.HeSoPhuThu=@heSoPhuThu))) ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maLoaiKhachHang", maLoaiKhachHang);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("heSoPhuThu", heSoPhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", thoiGianApDung.ToString("yyyy-MM-dd HH:mm:ss"));
                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public List<LoaiPhuThu> GetLoaiPhuThuCICO()
        {
            using (MySqlConnection conncheck = this.GetConnection())
            {
                conncheck.Open();

                string str = "SELECT * " +
                    "FROM loaiphuthu " +
                    "WHERE maloaiphuthu <> 1;";
                MySqlCommand cmd = new MySqlCommand(str, conncheck);
                List<LoaiPhuThu> loaiPhuThus = new List<LoaiPhuThu>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            LoaiPhuThu lpt = new LoaiPhuThu();
                            lpt.MaLoaiPhuThu = Convert.ToInt32(reader["MaLoaiPhuThu"]);
                            lpt.TenLoaiPhuThu = reader["TenLoaiPhuThu"].ToString();
                            loaiPhuThus.Add(lpt);
                        }
                        return loaiPhuThus;
                    }
                    else return null;
                }
            }
        }

        public List<PhuThu> GetPhuThuCICO()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT MaPhuThu, " +
                                            "SoLuongApDung, " +
                                            "TiLePhuThu, " +
                                            "ThoiGianApDung, " +
                                            "PT1.MaLoaiPhuThu, " +
                                            "LPT.TenLoaiPhuThu " +
                                     "FROM phuthu PT1 INNER JOIN loaiphuthu LPT ON PT1.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                                     "WHERE PT1.MaLoaiPhuThu <> 1 AND ((PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                        "FROM phuthu PT2 " +
                                                                        "WHERE PT1.SoLuongApDung = PT2.SoLuongApDung " +
                                                                            "AND PT1.MaLoaiPhuThu = PT2.MaLoaiPhuThu " +
                                                                            "AND PT2.ThoiGianApDung <= @now) " +
                                                "AND PT1.ThoiGianApDung <= @now) " +
                                            "OR PT1.ThoiGianApDung > @now) " +
                                     "ORDER BY `PT1`.`MaLoaiPhuThu` ASC, `PT1`.`SoLuongApDung` ASC, `PT1`.`ThoiGianApDung` ASC";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                List<PhuThu> phuThus = new List<PhuThu>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int check = Convert.ToInt32(reader["TiLePhuThu"]);
                            if (check == 0) continue;
                            PhuThu pt = new PhuThu();
                            pt.MaPhuThu = Convert.ToInt32(reader["MaPhuThu"]);
                            pt.TenLoaiPhuThu = reader["TenLoaiPhuThu"].ToString();
                            pt.TiLePhuThu = Convert.ToInt32(reader["TiLePhuThu"]);
                            pt.MaLoaiPhuThu = Convert.ToInt32(reader["MaLoaiPhuThu"]);
                            pt.ThoiGianApDung = Convert.ToDateTime(reader["ThoiGianApDung"]).ToString("yyyy-MM-ddTHH:mm:ss");
                            pt.SoLuongApDung = Convert.ToInt32(reader["SoLuongApDung"]);
                            phuThus.Add(pt);
                        }
                        return phuThus;
                    }
                    else return null;
                }
            }
        }

        public bool DeletePhuThuCICO(int maLoaiPhuThu, int soLuongApDung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO `phuthu` (`SoLuongApDung`, `TiLePhuThu`, `ThoiGianApDung`, `MaLoaiPhuThu`) " +
                                    "SELECT @soLuongApDung, @tiLePhuThu, @thoiGianApDung, @maLoaiPhuThu " +
                                    "WHERE NOT EXISTS (SELECT *" +
                                                        "FROM phuthu pt1 " +
                                                        "WHERE pt1.SoLuongApDung=@soLuongApDung " +
                                                            "AND pt1.TiLePhuThu=@tiLePhuThu " +
                                                            "AND pt1.ThoiGianApDung=@thoiGianApDung " +
                                                            "AND pt1.MaLoaiPhuThu=@maLoaiPhuThu);";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("maLoaiPhuThu", maLoaiPhuThu);
                cmd.Parameters.AddWithValue("tiLePhuThu", 0);
                cmd.Parameters.AddWithValue("thoiGianApDung", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool DeletePhuThuCICOFuture(int maPhuThu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "DELETE FROM phuthu WHERE MaPhuThu=@maPhuThu";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maPhuThu", maPhuThu);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdatePhuThuCICO(int maLoaiPhuThu, int soLuongApDung, int tiLePhuThu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO `phuthu` (`SoLuongApDung`, `TiLePhuThu`, `ThoiGianApDung`, `MaLoaiPhuThu`) " +
                                    "SELECT @soLuongApDung, @tiLePhuThu, @thoiGianApDung, @maLoaiPhuThu " +
                                    "WHERE NOT EXISTS (SELECT *" +
                                                        "FROM phuthu pt1 " +
                                                        "WHERE pt1.SoLuongApDung=@soLuongApDung " +
                                                            "AND pt1.TiLePhuThu=@tiLePhuThu " +
                                                            "AND pt1.ThoiGianApDung=@thoiGianApDung " +
                                                            "AND pt1.MaLoaiPhuThu=@maLoaiPhuThu);";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("maLoaiPhuThu", maLoaiPhuThu);
                cmd.Parameters.AddWithValue("tiLePhuThu", tiLePhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdatePhuThuCICOFuture(int maLoaiPhuThu, int soLuongApDung, int tiLePhuThu, DateTime thoiGianApDung, int maPhuThu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE PHUTHU " +
                                    "SET SoLuongApDung=@soLuongApDung, " +
                                        "MaLoaiPhuThu=@maLoaiPhuThu, " +
                                        "TiLePhuThu=@tiLePhuThu, " +
                                        "ThoiGianApDung=@thoiGianApDung " +
                                    "WHERE MaPhuThu= @maPhuThu AND NOT EXISTS(SELECT * " +
                                                                                    "FROM PHUTHU PT1 " +
                                                                                    "WHERE SoLuongApDung=@soLuongApDung " +
                                                                                    "AND PT1.MaLoaiPhuThu=@maLoaiPhuThu " +
                                                                                    "AND PT1.TiLePhuThu=@tiLePhuThu " +
                                                                                    "AND PT1.ThoiGianApDung=@thoiGianApDung)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maPhuThu", maPhuThu);
                cmd.Parameters.AddWithValue("maLoaiPhuThu", maLoaiPhuThu);
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("tiLePhuThu", tiLePhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", thoiGianApDung.ToString("yyyy-MM-dd HH:mm:ss"));

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool InsertPhuThuCICO(int maLoaiPhuThu, int soLuongApDung, int tiLePhuThu, DateTime thoiGianApDung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO `phuthu` (`SoLuongApDung`, `TiLePhuThu`, `ThoiGianApDung`, `MaLoaiPhuThu`) " +
                                    "SELECT @soLuongApDung, @tiLePhuThu, @thoiGianApDung, @maLoaiPhuThu " +
                                    "WHERE NOT EXISTS (SELECT * " +
                                                     "FROM phuthu PT1 INNER JOIN loaiphuthu LPT ON PT1.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                                                     "WHERE SoLuongApDung = @soLuongApDung AND PT1.MaLoaiPhuThu = @maLoaiPhuThu " +
                                                         "AND ((PT1.ThoiGianApDung >= ALL(SELECT PT2.ThoiGianApDung " +
                                                                                            "FROM phuthu PT2 " +
                                                                                            "WHERE PT1.SoLuongApDung = PT2.SoLuongApDung " +
                                                                                                "AND PT1.MaLoaiPhuThu = PT2.MaLoaiPhuThu " +
                                                                                                "AND PT2.ThoiGianApDung <= @now) " +
                                                                    "AND PT1.ThoiGianApDung <= @now AND TiLePhuThu <> 0 AND @thoiGianApDung <= @now ) " +
                                                                "OR (PT1.ThoiGianApDung > @now " +
                                                                    "AND @thoiGianApDung > @now " +
                                                                    "AND PT1.ThoiGianApDung=@thoiGianApDung " +
                                                                    "AND PT1.TiLePhuThu=@tiLePhuThu))) ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maLoaiPhuThu", maLoaiPhuThu);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("soLuongApDung", soLuongApDung);
                cmd.Parameters.AddWithValue("tiLePhuThu", tiLePhuThu);
                cmd.Parameters.AddWithValue("thoiGianApDung", thoiGianApDung.ToString("yyyy-MM-dd HH:mm:ss"));
                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public object getGioCICO()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT * " +
                                    "FROM thamso " +
                                    "WHERE TenThuocTinh IN ('GioCheckIn', 'GioCheckOut');";
                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        string gioCheckIn = "";
                        string gioCheckOut = "";
                        while (reader.Read())
                        {
                            if (reader["TenThuocTinh"].ToString() == "GioCheckIn")
                            {
                                gioCheckIn = reader["GiaTri"].ToString();
                            }
                            else
                            {
                                gioCheckOut = reader["GiaTri"].ToString();
                            }
                        }
                        return new { gioCheckIn = gioCheckIn, gioCheckOut = gioCheckOut };
                    }
                    return null;
                }
            }
        }

        public bool UpdateGioCICO(string gioCheckIn, string gioCheckOut)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryUpdateCI = "UPDATE thamso " +
                                        "SET GiaTri=@gioCheckIn " +
                                        "WHERE TenThuocTinh='GioCheckIn';";

                string queryUpdateCO = "UPDATE thamso " +
                                        "SET GiaTri=@gioCheckOut " +
                                        "WHERE TenThuocTinh='GioCheckOut';";

                MySqlCommand cmdCI = new MySqlCommand(queryUpdateCI, connectioncheck);
                MySqlCommand cmdCO = new MySqlCommand(queryUpdateCO, connectioncheck);
                cmdCI.Parameters.AddWithValue("gioCheckIn", gioCheckIn);
                cmdCO.Parameters.AddWithValue("gioCheckOut", gioCheckOut);

                int isSuccessCI = cmdCI.ExecuteNonQuery();
                if (isSuccessCI == 1)
                {
                    if (cmdCO.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public List<ChucVu> GetChucVu()
        {
            using (MySqlConnection conncheck = this.GetConnection())
            {
                conncheck.Open();

                string str = "SELECT * " +
                    "FROM chucvu " +
                    "WHERE DaXoa<>1;";
                MySqlCommand cmd = new MySqlCommand(str, conncheck);
                List<ChucVu> chucVus = new List<ChucVu>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ChucVu cv = new ChucVu();
                            cv.MaChucVu = Convert.ToInt32(reader["MaChucVu"]);
                            cv.TenChucVu = reader["TenChucVu"].ToString();
                            chucVus.Add(cv);
                        }
                        return chucVus;
                    }
                    else return null;
                }
            }
        }

        public bool DeleteChucVu(int maChucVu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE chucvu " +
                    "SET DaXoa=1 " +
                    "WHERE MaChucVu=@maChucVu";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maChucVu", maChucVu);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool InsertChucVu(string tenChucVu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "INSERT INTO chucvu(TenChucVu, DaXoa) " +
                                    "SELECT @tenChucVu, 0 " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM chucvu " +
                                                    "WHERE TenChucVu=@tenChucVu " +
                                                        "AND DaXoa = 0)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("tenChucVu", tenChucVu);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool UpdateChucVu(int maChucVu, string tenChucVu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "UPDATE chucvu " +
                    "SET TenChucVu=@tenChucVu " +
                    "WHERE MaChucVu=@maChucVu AND NOT EXISTS(SELECT * " +
                                                    "FROM chucvu " +
                                                    "WHERE TenChucVu=@tenChucVu " +
                                                        "AND DaXoa = 0)";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("maChucVu", maChucVu);
                cmd.Parameters.AddWithValue("tenChucVu", tenChucVu);

                int isSuccess = cmd.ExecuteNonQuery();
                if (isSuccess == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public string getLuongToiThieuVung()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT GiaTri " +
                                    "FROM thamso " +
                                    "WHERE TenThuocTinh='LuongToiThieuVung';";
                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader["GiaTri"].ToString();
                    }
                    return null;
                }
            }
        }

        public bool UpdateLuongToiThieuVung(int luongToiThieuVung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "UPDATE thamso " +
                              "SET GiaTri=@luongToiThieuVung " +
                              "WHERE TenThuocTinh='LuongToiThieuVung';";

                MySqlCommand cmd = new MySqlCommand(query, connectioncheck);
                cmd.Parameters.AddWithValue("luongToiThieuVung", luongToiThieuVung);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public List<PhanQuyen> GetPhanQuyen()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "SELECT pq.MaQuyen, quyen.TenQuyen, cv.MaChucVu, cv.TenChucVu " +
                    "FROM (phanquyen pq INNER JOIN chucvu cv on pq.MaChucVu = cv.MaChucVu) " +
                            "INNER JOIN quyen ON quyen.MaQuyen = pq.MaQuyen " +
                    "WHERE cv.DaXoa = 0 " +
                    "ORDER BY `cv`.`MaChucVu` ASC, `pq`.`MaQuyen` ASC";

                MySqlCommand cmd = new MySqlCommand(query, connectioncheck);
                List<PhanQuyen> phanQuyens = new List<PhanQuyen>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PhanQuyen ph = new PhanQuyen();
                            ph.MaQuyen = Convert.ToInt32(reader["MaQuyen"]);
                            ph.TenQuyen = reader["TenQuyen"].ToString();
                            ph.MaChucVu = Convert.ToInt32(reader["MaChucVu"]);
                            ph.TenChucVu = reader["TenChucVu"].ToString();
                            phanQuyens.Add(ph);
                        }
                        return phanQuyens;
                    }
                    else return null;
                }
            }
        }
        public List<Quyen> GetQuyen()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "SELECT * FROM `quyen` ORDER BY `quyen`.`MaQuyen` ASC;";

                MySqlCommand cmd = new MySqlCommand(query, connectioncheck);
                List<Quyen> quyens = new List<Quyen>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Quyen quyen = new Quyen();
                            quyen.MaQuyen = Convert.ToInt32(reader["MaQuyen"]);
                            quyen.TenQuyen = reader["TenQuyen"].ToString();
                            quyens.Add(quyen);
                        }
                        return quyens;
                    }
                    else return null;
                }
            }
        }

        public bool Decentralization(string stringPermission)
        {
            List<string> permissions = new List<string>();
            permissions = stringPermission.Split("@@@").ToList();
            permissions.RemoveAt(permissions.Count - 1);
            int length = permissions.Count;
            bool needInsert = (length > 0) ? true : false;

            string submit = "INSERT INTO phanquyen VALUES ";
            for (int i = 0; i < length; i++)
            {
                List<string> str = permissions[i].Split("$$").ToList();
                string maChucVu = str[0];
                List<string> listQuyen = str[1].Split("#").ToList();
                listQuyen.RemoveAt(listQuyen.Count - 1);
                int lengthListQuyen = listQuyen.Count;
                for (int j = 0; j < lengthListQuyen; j++)
                {
                    if (listQuyen[j] != "0")
                    {
                        submit += "(" + maChucVu + "," + (j + 1) + "),";
                    }
                }
            }
            submit = submit.Remove(submit.Length - 1);
            string deleteOldPermission = "DELETE FROM phanquyen WHERE MaChucVu <> 1;";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "Decentralization";

                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@deleteOldPermission", deleteOldPermission);
                cmd.Parameters["@deleteOldPermission"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@insertNewPermision", submit);
                cmd.Parameters["@insertNewPermision"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@needInsert", needInsert);
                cmd.Parameters["@needInsert"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("@isSucccess", MySqlDbType.Int32);
                cmd.Parameters["@isSucccess"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToBoolean(cmd.Parameters["@isSucccess"].Value);
            }
        }
        public string GetPhanQuyenForSession(int maChucVu)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "SELECT pq.MaQuyen " +
                    "FROM phanquyen pq INNER JOIN chucvu cv on pq.MaChucVu = cv.MaChucVu " +
                    "WHERE cv.DaXoa = 0 AND pq.MaChucVu = @maChucVu " +
                    "ORDER BY pq.`MaQuyen` ASC";

                MySqlCommand cmd = new MySqlCommand(query, connectioncheck);
                cmd.Parameters.AddWithValue("maChucVu", maChucVu);
                string permissionPerNhanVien = "";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            permissionPerNhanVien += reader["MaQuyen"].ToString();
                        }
                    }
                    return permissionPerNhanVien;
                }
            }
        }
        //Nguyên - end
        #endregion

        #region Nghĩa
        //Nghĩa - begin
        struct ThongKeKhach
        {
            public string ngay;
            public byte KhachNoiDia;
            public byte KhachNN;
        }
        public NhanVien GetNV(String maNV)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getNV, conn);
                cmd.Parameters.AddWithValue("maNV", maNV);
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

        public List<HoaDon> GetAllBill()
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
                            String maNV = result["MaNhanVien"].ToString();
                            bill.NV = GetNV(maNV);
                            bill.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                            bill.DoiTuongThanhToan = result["DoiTuongThanhToan"].ToString();
                            bill.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                            bill.ChiTiet = GetDetailById(bill.MaHoaDon);
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

        public LoaiPhong GetLPByMP(String maPhong)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getLPByMP, conn);
                cmd.Parameters.AddWithValue("MaPhong", maPhong);
                var result = cmd.ExecuteReader();
                if (result != null)
                {
                    result.Read();
                    LoaiPhong lp = new LoaiPhong();
                    lp.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                    lp.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                    lp.GiaTienCoBan = Convert.ToInt32(result["GiaTienCoBan"]);
                    lp.DaXoa = Convert.ToByte(result["DaXoa"]);
                    conn.Close();
                    return lp;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
        }
        public List<ChiTietHoaDon> GetAllDetailBills()
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
                            detail.MaCTHD = Convert.ToInt32(result["MaCTHD"]);
                            detail.MaPhong = result["MaPhong"].ToString();
                            detail.LoaiPhong = GetLPByMP(detail.MaPhong);
                            detail.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                            if (result["ThoiGianTraPhong"] == DBNull.Value)
                            {
                                detail.ThoiGianTraPhong = Convert.ToDateTime(null);
                            }
                            else
                            {
                                detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                            }
                            detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                            if (result["MaHoaDon"] == DBNull.Value)
                            {
                                detail.MaHoaDon = null;
                            }
                            else
                            {
                                detail.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                            }
                            detail.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                            detail.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
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

        public List<ChiTietHoaDon> GetDetailById(int MaHoaDon)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getDetailById, conn);
                cmd.Parameters.AddWithValue("maHoaDon", MaHoaDon);
                List<ChiTietHoaDon> details = new List<ChiTietHoaDon>();
                using (var result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        ChiTietHoaDon detail = new ChiTietHoaDon();
                        detail.MaCTHD = Convert.ToInt16(result["MaCTHD"]);
                        detail.MaHoaDon = Convert.ToInt16(result["MaHoaDon"]);
                        detail.MaPhong = result["MaPhong"].ToString();
                        detail.DsSoLuong = GetListSoLuong(detail.MaCTHD);
                        detail.LoaiPhong = GetLPByMP(detail.MaPhong);
                        detail.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                        detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                        detail.GhiChu = result["GhiChu"].ToString();
                        detail.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                        if(result["PhuThuCICO"] == DBNull.Value)
                        {
                            detail.PhuThuCICO = Convert.ToInt32(null);
                        } else
                        {
                            detail.PhuThuCICO = Convert.ToInt32(result["PhuThuCICO"]);
                        }
                        detail.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                        detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                        details.Add(detail);
                    }
                    conn.Close();
                    return details;
                }
            }
        }

        public HoaDon GetHoaDon(int MaHoaDon)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getHoaDon, conn);
                cmd.Parameters.AddWithValue("@maHoaDon", MaHoaDon);
                var result = cmd.ExecuteReader();
                result.Read();
                if (result != null)
                {
                    HoaDon hoaDon = new HoaDon();
                    hoaDon.NV = GetNV(result["MaNhanVien"].ToString());
                    hoaDon.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                    hoaDon.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                    hoaDon.DoiTuongThanhToan = result["DoiTuongThanhToan"].ToString();
                    hoaDon.ChiTiet = GetDetailById(MaHoaDon);
                    return hoaDon;
                }
                else
                {
                    return null;
                }
            }
        }

        public ChiTietHoaDon GetNullDetailByRoomID(String roomID)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getNullDetailByRoomID, conn);
                cmd.Parameters.AddWithValue("maPhong", roomID);
                var result = cmd.ExecuteReader();
                if (result.Read())
                {
                    ChiTietHoaDon ct = new ChiTietHoaDon();
                    ct.MaCTHD = Convert.ToInt16(result["MaCTHD"]);
                    ct.MaPhong = result["MaPhong"].ToString();
                    LoaiPhong lp = GetLPByMP(ct.MaPhong);
                    ct.LoaiPhong = lp;
                    ct.MaHoaDon = null;
                    ct.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                    if (result["ThoiGianTraPhong"] == DBNull.Value)
                    {
                        ct.ThoiGianTraPhong = Convert.ToDateTime(null);
                    }
                    else
                    {
                        ct.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                    }
                    ct.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                    ct.GhiChu = result["GhiChu"].ToString();
                    ct.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                    ct.TrangThai = Convert.ToByte(result["TrangThai"]);
                    conn.Close();
                    return ct;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
        }

        public bool CheckInsertDetailToBill(String roomID)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                ChiTietHoaDon detail = GetNullDetailByRoomID(roomID);
                if (detail == null)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
        }

        private static List<ChiTietHoaDon> pendingList = new List<ChiTietHoaDon>();

        public void PendingDetail(String roomID)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                bool checkRoom = CheckInsertDetailToBill(roomID);
                if (checkRoom == true)
                {
                    pendingList.Add(GetNullDetailByRoomID(roomID));
                }
                conn.Close();
            }
        }

        public bool IsPendingListNotNull()
        {
            return pendingList.Count != 0;
        }

        public void CancelDetailBill()
        {
            pendingList = new List<ChiTietHoaDon>();
        }

        // Sua 2-6-2022
        public double PhuThuCI(DateTime thoiGianNhanPhong)
        {
            if (thoiGianNhanPhong.Hour < 14)
            {
                double ketqua = 0;
                int soGio = 14 - thoiGianNhanPhong.Hour;
                MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getPhuThuCI, conn);
                cmd.Parameters.AddWithValue("thoiGianCI", thoiGianNhanPhong.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("soGio", soGio);
                var result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    ketqua = Convert.ToDouble((Convert.ToDouble(result["TiLePhuThu"])) / 100);
                }
                conn.Close();
                return ketqua;
            }
            else
            {
                return 0;
            }
        }

        public double PhuThuCO(DateTime thoiGianTraPhong)
        {
            if (thoiGianTraPhong.Hour > 12)
            {
                double ketqua = 0;
                int soGio = thoiGianTraPhong.Hour - 12;
                MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getPhuThuCO, conn);
                cmd.Parameters.AddWithValue("thoiGianCO", thoiGianTraPhong.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("soGio", soGio);
                var result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    ketqua = Convert.ToDouble((Convert.ToDouble(result["TiLePhuThu"])) / 100);
                }
                conn.Close();
                return ketqua;
            }
            else { return 0; }
        }

        public int SaveListDetailBill(String maNV, String doiTuong)
        {
            if (pendingList != null)
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    HoaDon hoaDonMoi = new HoaDon();

                    MySqlCommand pendBill = new MySqlCommand(SQLQuery.insertPendingBill, conn);
                    pendBill.ExecuteNonQuery();

                    MySqlCommand getId = new MySqlCommand(SQLQuery.getPendingBillID, conn);
                    var result = getId.ExecuteReader();
                    result.Read();
                    int id = Convert.ToInt32(result["MaHoaDon"]);
                    result.Close();
                    int total = 0;
                    foreach (ChiTietHoaDon detail in pendingList)
                    {
                        int totalDetail = 0;
                        int mact = detail.MaCTHD;
                        string maPhong = detail.MaPhong;
                        MySqlCommand cmdUpdateCO = new MySqlCommand(SQLQuery.updateTimeCO, conn);
                        cmdUpdateCO.Parameters.AddWithValue("thoiGianCO", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdUpdateCO.Parameters.AddWithValue("maCT", mact);
                        cmdUpdateCO.ExecuteNonQuery();

                        MySqlCommand cmdUpdatePayRoom = new MySqlCommand(SQLQuery.updatePayRoom, conn);
                        cmdUpdatePayRoom.Parameters.AddWithValue("maPhong", maPhong);
                        cmdUpdatePayRoom.ExecuteNonQuery();

                        List<SoLuongKhachThue> listSLKT = ConvertToSLKT(mact);
                        foreach (SoLuongKhachThue luongKhach in listSLKT)
                        {
                            totalDetail += luongKhach.ThanhTien;
                        }
                        int phuThuCI = (int)(PhuThuCI(detail.ThoiGianNhanPhong) * detail.GiaPhong);
                        int phuThuCO = (int)(PhuThuCO(detail.ThoiGianTraPhong) * detail.GiaPhong);
                        int tongPhuThu = phuThuCI + phuThuCO;
                        totalDetail += tongPhuThu;
                        detail.PhuThuCICO = tongPhuThu;
                        MySqlCommand updateCT = new MySqlCommand(SQLQuery.updateBillIdForDetail, conn);
                        updateCT.Parameters.AddWithValue("maHoaDon", id);
                        updateCT.Parameters.AddWithValue("maCT", mact);
                        updateCT.Parameters.AddWithValue("tongTien", totalDetail);
                        updateCT.Parameters.AddWithValue("phuThuCICO", detail.PhuThuCICO);
                        updateCT.ExecuteNonQuery();
                        total += totalDetail;
                    }

                    hoaDonMoi.MaHoaDon = id;
                    hoaDonMoi.ThoiGianXuat = DateTime.Now;
                    hoaDonMoi.ChiTiet = GetDetailById(id);
                    hoaDonMoi.NV = GetNV(maNV);
                    hoaDonMoi.DoiTuongThanhToan = doiTuong;
                    hoaDonMoi.TongSoTien = total;

                    MySqlCommand updateBill = new MySqlCommand(SQLQuery.updateBillInfor, conn);
                    updateBill.Parameters.AddWithValue("maNV", hoaDonMoi.NV.MaNhanVien);
                    updateBill.Parameters.AddWithValue("thoiGianXuat", hoaDonMoi.ThoiGianXuat.ToString("yyyy-MM-dd HH:mm:ss"));
                    updateBill.Parameters.AddWithValue("tongTien", hoaDonMoi.TongSoTien);
                    updateBill.Parameters.AddWithValue("dtThanhToan", hoaDonMoi.DoiTuongThanhToan);
                    updateBill.Parameters.AddWithValue("id", hoaDonMoi.MaHoaDon);
                    updateBill.ExecuteNonQuery();

                    pendingList = new List<ChiTietHoaDon>();
                    return id;
                }
            }
            return 0;
        }

        public List<KhachThue> GetListKhachThue(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getListKhachThue, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                List<KhachThue> listKhachThue = new List<KhachThue>();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        KhachThue khachThue = new KhachThue();
                        khachThue.MaKhachThue = Convert.ToInt32(result["MaKhachThue"]);
                        khachThue.CCCD = result["CCCD"].ToString();
                        khachThue.ThoiGianCheckin = Convert.ToDateTime(result["ThoiGianCheckIn"]);
                        if (result["ThoiGianCheckOut"] != DBNull.Value)
                        {
                            khachThue.ThoiGianCheckout = Convert.ToDateTime(result["ThoiGianCheckOut"]);
                        }
                        else
                        {
                            khachThue.ThoiGianCheckout = Convert.ToDateTime(null);
                        }
                        khachThue.HoTen = result["HoTen"].ToString();
                        khachThue.MaLoaiKhachHang = Convert.ToInt16(result["MaLoaiKhachHang"]);
                        khachThue.DiaChi = result["DiaChi"].ToString();
                        khachThue.MaCTHD = Convert.ToInt32(result["MaCTHD"]);
                        listKhachThue.Add(khachThue);
                    }
                    conn.Close();
                    return listKhachThue;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
        }

        public List<SoLuongKhachThue> GetListSoLuong(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getListSoLuong, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                List<SoLuongKhachThue> kq = new List<SoLuongKhachThue>();
                if (result != null)
                {
                    while (result.Read())
                    {
                        SoLuongKhachThue record = new SoLuongKhachThue();
                        record.MaCTHD = maCTHD;
                        record.SoKhachNN = Convert.ToByte(result["SoKhachNN"]);
                        record.SoKhachThue = Convert.ToByte(result["SoKhachThue"]);
                        record.SoNgayThue = Convert.ToByte(result["SoNgayThue"]);
                        record.GhiChu = result["GhiChu"].ToString();
                        record.DonGia = Convert.ToInt32(result["DonGia"]);
                        record.ThanhTien = Convert.ToInt32(result["ThanhTien"]);
                        record.PhuThu = Convert.ToInt32(result["PhuThu"]);
                        record.HeSoKhach = (float)Convert.ToDouble(result["HeSoKhach"]);
                        kq.Add(record);
                    }
                    return kq;
                }
                else
                {
                    return null;
                }
            }
        }
        public ChiTietHoaDon GetDetailByDetailID(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getDetailByDetailId, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                if (result.Read())
                {
                    ChiTietHoaDon kq = new ChiTietHoaDon();
                    kq.MaCTHD = maCTHD;
                    kq.MaPhong = result["MaPhong"].ToString();
                    if (result["MaHoaDon"] == DBNull.Value)
                    {
                        kq.MaHoaDon = null;
                    }
                    else
                    {
                        kq.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                    }
                    kq.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                    kq.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                    kq.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                    if (result["PhuThuCICO"] == DBNull.Value)
                    {
                        kq.PhuThuCICO = 0;
                    }
                    else
                    {
                        kq.PhuThuCICO = Convert.ToInt32(result["PhuThuCICO"]);
                    }
                    kq.GhiChu = result["GhiChu"].ToString();
                    kq.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                    kq.TrangThai = Convert.ToByte(result["TrangThai"]);
                    kq.DsKhachThue = GetListKhachThue(maCTHD);
                    kq.DsSoLuong = GetListSoLuong(maCTHD);
                    return kq;
                }
                else
                {
                    return null;
                }
            }
        }

        public int TinhTien(int donGia, int soNgay, int phuThu, float heSo)
        {
            int result = (int)(((donGia * soNgay) + ((donGia * phuThu) / 100)) * heSo);
            return result;
        }

        public List<SoLuongKhachThue> ConvertToSLKT(int maCTHD)
        {

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.updateCheckOutKhach, conn);
                cmd.Parameters.AddWithValue("thoiGianCO", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand(SQLQuery.getCheckInCheckOutDetail, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var resultTimeRange = cmd.ExecuteReader();
                resultTimeRange.Read();
                DateTime checkIn = Convert.ToDateTime(resultTimeRange["ThoiGianNhanPhong"]);
                DateTime checkOut = Convert.ToDateTime(resultTimeRange["ThoiGianTraPhong"]);
                resultTimeRange.Close();

                TimeSpan diff = checkOut - checkIn;

                List<DateTime> days = new List<DateTime>();
                if (diff.Hours > 10)
                {
                    for (int i = 0; i <= diff.Days + 1; i++)
                    {
                        days.Add(checkIn.Date.AddDays(i));
                    }
                }
                else
                {
                    for (int i = 0; i <= diff.Days; i++)
                    {
                        days.Add(checkIn.Date.AddDays(i));
                    }
                }

                List<ThongKeKhach> listThongKe = new List<ThongKeKhach>();

                // Convert timeCI timeCO
                string checkInString = checkIn.ToString("yyyy-MM-dd HH:mm:ss");
                string checkOutString = checkOut.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (DateTime day in days)
                {
                    string ngay = day.Year + "-" + day.Month + "-" + day.Day;
                    cmd = new MySqlCommand(SQLQuery.countKhachNN, conn);
                    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                    cmd.Parameters.AddWithValue("thoiGianCO", ngay);
                    var result = cmd.ExecuteReader();
                    result.Read();

                    ThongKeKhach newRecord = new ThongKeKhach();
                    newRecord.ngay = ngay;
                    newRecord.KhachNN = Convert.ToByte(result["KhachNN"]);
                    result.Close();

                    cmd = new MySqlCommand(SQLQuery.countKhachNoiDia, conn);
                    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                    cmd.Parameters.AddWithValue("thoiGianCO", ngay);
                    result = cmd.ExecuteReader();
                    result.Read();

                    newRecord.KhachNoiDia = Convert.ToByte(result["KhachNoiDia"]);
                    result.Close();
                    listThongKe.Add(newRecord);
                }
                // Xong bang thong ke luong khach theo ngay

                Dictionary<string, byte> dictCount = new Dictionary<string, byte>();
                foreach (ThongKeKhach record in listThongKe)
                {
                    // Struct key = KhachNoiDia + KhachNN
                    string key = record.KhachNoiDia.ToString() + record.KhachNN.ToString();

                    if (dictCount.ContainsKey(key))
                    {
                        dictCount[key]++;
                    }
                    else
                    {
                        dictCount.Add(key, 1);
                    }
                }
                // Da co duoc Dict so luong khach theo loai khach va theo so luong ngay
                List<SoLuongKhachThue> kq = new List<SoLuongKhachThue>();
                foreach (KeyValuePair<string, byte> entry in dictCount)
                {
                    string key = entry.Key;
                    SoLuongKhachThue history = new SoLuongKhachThue();
                    history.SoKhachNN = Convert.ToByte(key[1].ToString());
                    history.SoKhachThue = Convert.ToByte(history.SoKhachNN + Convert.ToByte(key[0].ToString()));
                    history.SoNgayThue = entry.Value;
                    history.MaCTHD = maCTHD;
                    cmd = new MySqlCommand(SQLQuery.getGiaPhongByDetailID, conn);
                    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                    var res = cmd.ExecuteReader();
                    res.Read();
                    history.DonGia = Convert.ToInt32(res["GiaTienCoBan"]);
                    res.Close();
                    if (history.SoKhachThue > 2)
                    {
                        cmd = new MySqlCommand(SQLQuery.getPhuThuTheoSoLuong, conn);
                        cmd.Parameters.AddWithValue("soLuong", history.SoKhachThue);
                        cmd.Parameters.AddWithValue("thoiGianCI", checkInString);
                        var phuthu = cmd.ExecuteReader();
                        phuthu.Read();
                        if (!phuthu.HasRows)
                        {
                            history.PhuThu = 0;
                        }
                        else
                        {
                            if (phuthu["TiLePhuThu"] == DBNull.Value)
                            {
                                history.PhuThu = Convert.ToInt32(0);
                            }
                            else
                                history.PhuThu = Convert.ToInt32(phuthu["TiLePhuThu"]);
                        }
                        phuthu.Close();
                    }

                    //Tinh he so lkh
                    cmd = new MySqlCommand(SQLQuery.getHeSoLKH, conn);
                    cmd.Parameters.AddWithValue("thoiGianCI", checkIn);
                    cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
                    cmd.Parameters.AddWithValue("soKhachND", Convert.ToByte(key[0].ToString()));
                    var heso = cmd.ExecuteReader();
                    heso.Read();
                    if (heso["HeSo"] != DBNull.Value)
                    {
                        history.HeSoKhach = (float)Convert.ToDouble(heso["HeSo"]);
                    }
                    else
                    {
                        history.HeSoKhach = 1;
                    }
                    history.ThanhTien = TinhTien(history.DonGia, history.SoNgayThue, history.PhuThu, history.HeSoKhach);
                    history.GhiChu = null;
                    heso.Close();

                    // Insert to DB
                    cmd = new MySqlCommand(SQLQuery.insertSLKT, conn);
                    cmd.Parameters.AddWithValue("soLuongKhach", history.SoKhachThue);
                    cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
                    cmd.Parameters.AddWithValue("soNgay", history.SoNgayThue);
                    cmd.Parameters.AddWithValue("ghiChu", history.GhiChu);
                    cmd.Parameters.AddWithValue("donGia", history.DonGia);
                    cmd.Parameters.AddWithValue("phuThu", history.PhuThu);
                    cmd.Parameters.AddWithValue("heSo", history.HeSoKhach);
                    cmd.Parameters.AddWithValue("thanhTien", history.ThanhTien);
                    cmd.Parameters.AddWithValue("maCTHD", history.MaCTHD);
                    cmd.ExecuteNonQuery();
                    kq.Add(history);
                }
                return kq;
            }
        }

        public void DeleteDetailById(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand del = new MySqlCommand(SQLQuery.deleteDetailByID, conn);
                del.Parameters.AddWithValue("maCTHD", maCTHD);
                del.ExecuteNonQuery();
            }
        }

        public void UpdateCancelStatusDetail(int maCTHD)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand update = new MySqlCommand(SQLQuery.updateCancelStatusDetail, conn);
                update.Parameters.AddWithValue("maCTHD", maCTHD);
                update.ExecuteNonQuery();
            }
        }

        //Nghĩa - end
        #endregion

        #region Hiếu
        //Hiếu - begin
        public List<NhanVien> getAllDetailStaff()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailStaff, conn);
                List<NhanVien> listNhanVien = new List<NhanVien>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            NhanVien nv = new NhanVien();
                            nv.MaNhanVien = result["MaNhanVien"].ToString();
                            nv.MatKhau = result["MatKhau"].ToString();
                            nv.CCCD = result["CCCD"].ToString();
                            nv.HoTen = result["HoTen"].ToString();
                            nv.GioiTinh = Convert.ToByte(result["GioiTinh"]);
                            nv.NgaySinh = Convert.ToDateTime(result["NgaySinh"]);
                            nv.Email = result["Email"].ToString();
                            nv.SoDienThoai = result["SoDienThoai"].ToString();
                            nv.NgayVaoLam = Convert.ToDateTime(result["NgayVaoLam"]);
                            nv.MaChucVu = Convert.ToUInt16(result["MaChucVu"]);
                            nv.TenChucVu = result["TenChucVu"].ToString();
                            nv.HinhAnh = result["HinhAnh"].ToString();
                            nv.Luong = Convert.ToInt32(result["Luong"]);
                            listNhanVien.Add(nv);
                        }
                        conn.Close();
                        return listNhanVien;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }
        public List<Phong> getAllDetailRoom()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailRoom, conn);
                List<Phong> listPhong = new List<Phong>();

                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            Phong p = new Phong();
                            p.MaPhong = result["MaPhong"].ToString();
                            p.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            p.Tang = Convert.ToByte(result["Tang"]);
                            p.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                            p.TrangThai = Convert.ToByte(result["TrangThai"]);
                            p.GhiChu = result["GhiChu"].ToString();


                            listPhong.Add(p);
                        }
                        conn.Close();
                        return listPhong;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }
        public List<LoaiPhong> getAllRoomStyle()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllRoomStyle, conn);
                List<LoaiPhong> listLoaiPhong = new List<LoaiPhong>();

                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            LoaiPhong lp = new LoaiPhong();

                            lp.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                            lp.GiaTienCoBan = Convert.ToInt32(result["GiaTienCoBan"]);
                            lp.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                            lp.DaXoa = Convert.ToByte(result["DaXoa"]);
                            listLoaiPhong.Add(lp);
                        }
                        conn.Close();
                        return listLoaiPhong;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        public object postNewRoom(Phong ph)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewRoom, conn);
                cmd.Parameters.AddWithValue("MaPhong", ph.MaPhong);
                cmd.Parameters.AddWithValue("MaLoaiPhong", ph.MaLoaiPhong);
                cmd.Parameters.AddWithValue("Tang", ph.Tang);
                cmd.Parameters.AddWithValue("TrangThai", ph.TrangThai);
                cmd.Parameters.AddWithValue("GhiChu", ph.GhiChu);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    //Form thành công
                }
                else
                {
                    //Form báo lỗi: trùng id/ Tầng không quá 255
                }
                conn.Close();
            }
            return null;

        }
        public object postNewStaff(NhanVien nv)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewStaff, conn);

                cmd.Parameters.AddWithValue("MaNhanVien", nv.MaNhanVien);
                cmd.Parameters.AddWithValue("MatKhau", nv.MatKhau);
                cmd.Parameters.AddWithValue("CCCD", nv.CCCD);
                cmd.Parameters.AddWithValue("HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("GioiTinh", nv.GioiTinh);
                cmd.Parameters.AddWithValue("NgaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("Email", nv.Email);
                cmd.Parameters.AddWithValue("SoDienThoai", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("NgayVaoLam", nv.NgayVaoLam);
                cmd.Parameters.AddWithValue("MaChucVu", nv.MaChucVu);
                cmd.Parameters.AddWithValue("HinhAnh", nv.HinhAnh);
                cmd.Parameters.AddWithValue("Luong", nv.Luong);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    //Form thành công
                }
                else
                {
                    //Form báo lỗi:
                }
                conn.Close();
            }
            return null;

        }
        public List<ChucVu> getAllDetailRoles()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDetailRoles, conn);
                List<ChucVu> listChucVu = new List<ChucVu>();

                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            ChucVu cv = new ChucVu();
                            cv.MaChucVu = Convert.ToByte(result["MaChucVu"]);
                            cv.TenChucVu = result["TenChucVu"].ToString();
                            cv.DaXoa = Convert.ToByte(result["DaXoa"]);

                            listChucVu.Add(cv);
                        }
                        conn.Close();
                        return listChucVu;
                    }
                    else
                    {

                        conn.Close();
                        return null;
                    }

                }
            }
        }
        public NhanVien getChosenStaff(string maNhanVien)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getChosenStaff, conn);
                NhanVien nhanvien = new NhanVien();

                cmd.Parameters.AddWithValue("MaNhanVien", maNhanVien);


                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            NhanVien nv = new NhanVien();
                            nv.MaNhanVien = result["MaNhanVien"].ToString();
                            nv.MatKhau = result["MatKhau"].ToString();
                            nv.CCCD = result["CCCD"].ToString();
                            nv.HoTen = result["HoTen"].ToString();
                            nv.GioiTinh = Convert.ToByte(result["GioiTinh"]);
                            nv.NgaySinh = Convert.ToDateTime(result["NgaySinh"]);
                            nv.Email = result["Email"].ToString();
                            nv.SoDienThoai = result["SoDienThoai"].ToString();
                            nv.NgayVaoLam = Convert.ToDateTime(result["NgayVaoLam"]);
                            nv.MaChucVu = Convert.ToUInt16(result["MaChucVu"]);
                            nv.HinhAnh = result["HinhAnh"].ToString();
                            nv.Luong = Convert.ToInt32(result["Luong"]);
                            nhanvien = nv;
                        }
                        conn.Close();
                        return nhanvien;
                    }
                    else
                    {

                        conn.Close();
                        return null;
                    }

                }
            }
        }

        public object deleteRoom(string ph)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.deleteRoom, conn);
                cmd.Parameters.AddWithValue("MaPhong", ph);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    //Form thành công
                }
                else
                {
                    //Form báo lỗi: trùng id/ Tầng không quá 255
                }
                conn.Close();
            }
            return null;

        }
        //Hiếu - end
        #endregion

        #region Trí
        //Trí - begin
        public List<BaoCaoDoanhThuThang> getAllBCDTThang()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getAllDoanhThuThang, conn);
                List<BaoCaoDoanhThuThang> bills = new List<BaoCaoDoanhThuThang>();
                //lấy danh sách báo cáo có tiền
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
                        return bills;
                    }
                    else
                    {
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
                            bill.MaDotTraLuong = MaBC;
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
                string str = "  SELECT dottraluong.MaDotTraLuong , traluong.MaNhanVien , nhanvien.HoTen , chucvu.TenChucVu, nhanvien.CCCD , `Thuong`, `Phat`, `GhiChu`, `SoTien` ,nhanvien.GioiTinh FROM traluong, nhanvien, chucvu, dottraluong WHERE traluong.MaDotTraLuong = dottraluong.MaDotTraLuong AND nhanvien.MaNhanVien = traluong.MaNhanVien AND traluong.MaChucVu = chucvu.MaChucVu AND Month(dottraluong.NgayTraLuong) = @thang AND YEAR(dottraluong.NgayTraLuong) = @nam ";
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
                           
                            bill.MaDotTraLuong = Convert.ToInt32(result["MaNhanVien"]); 
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
            //tạo khóabc
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
            //tìm khóa đã tạo
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

            //chọn ra loại phòng có trong cthd
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

            //chọn ra tất cả loại phòng còn hoạt động
            List<ChiTietBaoCaoDoanhThuThang> list_lp = new List<ChiTietBaoCaoDoanhThuThang>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = "SELECT `MaLoaiPhong`, `TenLoaiPhong` FROM `loaiphong` WHERE loaiphong.DaXoa = 0 ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd = new MySqlCommand(str, conn);
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
                            bill.SoTien = 0;
                            bill.TiLe = 0;
                            list_lp.Add(bill);
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

                for (int i = 0; i < list_lp.Count; i++)
                {
                    for (int j = 0; j < list_CTBC.Count; j++)
                    {
                        if(list_lp[i].MaLoaiPhong == list_CTBC[j].MaLoaiPhong)
                        {
                            list_lp[i].SoTien = list_CTBC[j].SoTien;
                            list_lp[i].TiLe = list_lp[i].SoTien / tongTien * 100;
                        }
                        else
                        {
                            list_lp[i].SoTien = 0;
                            list_lp[i].TiLe = 0;
                        }
                    }
                }

                foreach (var item in list_lp)
                {
                    string str = " INSERT INTO `ctbcdoanhthuthang`(`MaBCDoanhThu`, `MaLoaiPhong`, `SoTien`, `TiLe`) VALUES (@mabcdoanhthu, @malp, @sotien, @tile) ";
                    MySqlCommand cmd = new MySqlCommand(str, conn);
                    cmd.Parameters.AddWithValue("mabcdoanhthu", item.MaBCDoanhThu);
                    cmd.Parameters.AddWithValue("malp", item.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("sotien", item.SoTien);
                    cmd.Parameters.AddWithValue("tile", item.TiLe);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return 1;
            }
            return 0;
        }

        public List<BaoCaoLuongChucVu> getAllBaoCaoLuongChucVu(DateTime dt)
        {
            //lấy list chức vụ còn xài
            List<BaoCaoLuongChucVu> list_chucvu = new List<BaoCaoLuongChucVu>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT `MaChucVu`, `TenChucVu` FROM `chucvu` WHERE chucvu.DaXoa = 0 ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        BaoCaoLuongChucVu bil = new BaoCaoLuongChucVu();
                        bil.MaChucVu = Convert.ToInt32(result["MaChucVu"]);
                        bil.TenChucVu = (result["TenChucVu"]).ToString();
                        bil.TongLuong = 0;
                        bil.TiLe = 0;
                        list_chucvu.Add(bil);
                    }
                }
            }

            //list nv thực tế
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string str = " SELECT tl.MaChucVu, cv.TenChucVu , SUM(tl.SoTien) as SoTien FROM traluong tl, dottraluong dtl, chucvu cv WHERE tl.MaDotTraLuong = dtl.MaDotTraLuong AND cv.MaChucVu = tl.MaChucVu AND Year(dtl.NgayTraLuong) = @nam AND Month(dtl.NgayTraLuong) = @thang GROUP BY tl.MaChucVu ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", dt.Year);
                cmd.Parameters.AddWithValue("thang", dt.Month);

                List<BaoCaoLuongChucVu> list_chucvucotraluong = new List<BaoCaoLuongChucVu>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            BaoCaoLuongChucVu bil = new BaoCaoLuongChucVu();
                            bil.MaChucVu = Convert.ToInt32(result["MaChucVu"]);
                            bil.TenChucVu = (result["TenChucVu"]).ToString();
                            bil.TongLuong = Convert.ToInt32(result["SoTien"]);
                            bil.TiLe = 0;
                            list_chucvucotraluong.Add(bil);
                        }
                        int TongTien = 0;
                        foreach (var item in list_chucvucotraluong)
                        {
                            TongTien += item.TongLuong;
                        }
                        for (int i = 0; i < list_chucvu.Count; i++)
                        {
                            for (int j = 0; j < list_chucvucotraluong.Count; j++)
                            {
                                if (list_chucvu[i].MaChucVu == list_chucvucotraluong[j].MaChucVu)
                                {
                                    list_chucvu[i].TongLuong = list_chucvucotraluong[j].TongLuong;
                                    list_chucvu[i].TiLe = list_chucvucotraluong[j].TongLuong/ TongTien * 100;
                                }
                                else
                                {
                                    list_chucvu[i].TongLuong = 0;
                                    list_chucvu[i].TiLe = 0;
                                }
                            }
                        }
                        conn.Close();
                        return list_chucvu;
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

        public int UpdateLuongStaf(ChiTietDotTraLuong info_staff)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = " UPDATE `traluong` SET `Thuong`= @thuong ,`Phat`= @phat ,`GhiChu`= @ghichu,`SoTien`= @sotien WHERE traluong.MaDotTraLuong = @madot AND traluong.MaNhanVien = @manv ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("thuong", info_staff.Thuong);
                cmd.Parameters.AddWithValue("phat", info_staff.Phat);
                cmd.Parameters.AddWithValue("ghichu", info_staff.GhiChu);
                cmd.Parameters.AddWithValue("sotien", info_staff.SoTien);
                cmd.Parameters.AddWithValue("madot", info_staff.MaDotTraLuong);
                cmd.Parameters.AddWithValue("manv", info_staff.MaNhanVien);
                int result = cmd.ExecuteNonQuery();
                return result;
            }
           return 0;
        }

        public ThongKeDoanhThu GetThuChi(DateTime ThangBaoCao)
        {
            ThongKeDoanhThu th = new ThongKeDoanhThu();
            th.TienThu = th.TienChi = th.LoiNhuan = 0;
            th.ThangBaoCao = ThangBaoCao;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string str = " SELECT TongTienPhong  FROM chitiethoadon , hoadon WHERE chitiethoadon.MaHoaDon = hoadon.MaHoaDon AND Year(hoadon.ThoiGianXuat) = @nam AND Month(hoadon.ThoiGianXuat) = @thang  ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);

                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            th.TienThu += Convert.ToInt32(result["TongTienPhong"]);
                        }
                    }
                }

            }

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string str = " SELECT traluong.SoTien FROM `traluong` , dottraluong WHERE traluong.MaDotTraLuong = dottraluong.MaDotTraLuong AND Year(dottraluong.NgayTraLuong) = @nam AND Month(dottraluong.NgayTraLuong) = @thang  ";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("nam", ThangBaoCao.Year);
                cmd.Parameters.AddWithValue("thang", ThangBaoCao.Month);

                List<int> Tien = new List<int>();
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            th.TienChi += Convert.ToInt32(result["SoTien"]);
                        }
                    }
                }
            }

            th.LoiNhuan = th.TienChi - th.TienChi;

            return th;       
        }   
        //Trí - end
        #endregion
    }
}
