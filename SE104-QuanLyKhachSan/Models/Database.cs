using MySql.Data.MySqlClient;
using SE104_QuanLyKhachSan.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
                    "WHERE (@userName IN (MaNhanVien, CCCD, Email, SoDienThoai)) AND MatKhau = @password AND MaChucVu<>0;";
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
                                "WHERE P.Tang = @tang AND P.TrangThai = @trangThai AND RIGHT(P.MaPhong, 2) LIKE @soPhong " +
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
                if (soPhong == null || soPhong == "")
                {
                    query = query.Replace("P.MaPhong, 2", "P.MaPhong, 0");
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
                if (soPhong != null && soPhong != "")
                {
                    cmd.Parameters.AddWithValue("soPhong", "%"+soPhong);
                }
                else
                {
                    cmd.Parameters.AddWithValue("soPhong", "");
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
                    "FROM LOAIKHACHHANG ";
                  
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

        public int GetSoKhachToiDaForThayDoiKhachO()
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

                string queryString = "DELETE FROM loaiphong " +
                    "WHERE MaLoaiPhong=@maLoaiPhong " +
                        "AND NOT EXISTS (SELECT * " +
                                     "FROM phong p " +
                                     "WHERE @maLoaiPhong=p.MaLoaiPhong) " +
                        "AND NOT EXISTS (SELECT * " +
                                        "FROM ctbcdoanhthuthang ct " +
                                        "WHERE ct.MaLoaiPhong=@maLoaiPhong)";

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
                                                                  "WHERE TenLoaiPhong=@tenLoaiPhong) ;";

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

                string queryString = "INSERT INTO LOAIPHONG(TenLoaiPhong, GiaTienCoBan) " +
                                        "SELECT @tenLoaiPhong, @giaTienCoBan " +
                                        "WHERE NOT EXISTS(SELECT * " +
                                                                  "FROM loaiphong " +
                                                                  "WHERE TenLoaiPhong=@tenLoaiPhong) ;";

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

                string queryString = "DELETE FROM loaikhachhang " +
                    "WHERE MaLoaiKhachHang=@maLoaiKhachHang " +
                        "AND NOT EXISTS(SELECT * " +
                                        "FROM khachthue kt " +
                                        "WHERE kt.MaLoaiKhachHang = @maLoaiKhachHang) " +
                        "AND NOT EXISTS(SELECT * " +
                                        "FROM phuthulkh ptlkh " +
                                        "WHERE ptlkh.MaLoaiKhachHang = @maLoaiKhachHang);";

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

                string queryString = "INSERT INTO LOAIKHACHHANG(TenLoaiKhachHang) " +
                                    "SELECT @tenLoaiKhachHang " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM loaikhachhang " +
                                                    "WHERE TenLoaiKhachHang=@tenLoaiKhachHang)";

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
                                                    "WHERE TenLoaiKhachHang=@tenLoaiKhachHang)";

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
                    "WHERE TenThuocTinh='SoKhachToiDa';";

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
                                             "OR PT1.ThoiGianApDung > @now)) " +
                            "AND NOT EXISTS (SELECT * " +
                                    "FROM phuthulkh ptlkh " +
                                    "WHERE ptlkh.SoLuongApDung>@soKhachToiDa " +
                                        "AND ((ptlkh.ThoiGianApDung >= ALL(SELECT ptlkh2.ThoiGianApDung " +
                                                                        "FROM phuthulkh ptlkh2 " +
                                                                        "WHERE ptlkh.SoLuongApDung = ptlkh2.SoLuongApDung AND ptlkh2.ThoiGianApDung <= @now) " +
                                               "AND ptlkh.ThoiGianApDung <= @now) " +
                                             "OR ptlkh.ThoiGianApDung > @now)) ";

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
                                    "WHERE PT1.MaLoaiPhuThu = 1 AND PT1.TiLePhuThu <> 0 " +
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

        public List<PhuThu> GetLichSuPhuThuSoKhach()
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string queryString = "SELECT * " +
                                    "FROM PHUTHU PT1 " +
                                    "WHERE PT1.MaLoaiPhuThu = 1 AND PT1.TiLePhuThu <> 0 " +
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
                                     "FROM phuthulkh PT1 INNER JOIN loaikhachhang LKH ON (PT1.MaLoaiKhachHang = LKH.MaLoaiKhachHang  " +
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

        public List<PhuThuLKH> GetLichSuPhuThuLoaiKhachHang()
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
                                     "FROM phuthulkh PT1 INNER JOIN loaikhachhang LKH ON (PT1.MaLoaiKhachHang = LKH.MaLoaiKhachHang " +
                                     "WHERE HeSoPhuThu<>0 " +
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
                                                     "FROM phuthulkh PT1 INNER JOIN loaikhachhang LKH ON (PT1.MaLoaiKhachHang = LKH.MaLoaiKhachHang) " +
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

        public List<PhuThu> GetLichSuPhuThuCICO()
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
                                     "WHERE PT1.MaLoaiPhuThu <> 1 " +
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
                    "FROM chucvu;";
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

                string queryString = "DELETE FROM chucvu " +
                    "WHERE MaChucVu=@maChucVu " +
                        "AND NOT EXISTS(SELECT * " +
                                      "FROM ctbcchitraluong ctbc " +
                                      "WHERE ctbc.MaChucVu=@maChucVu) " +
                        "AND NOT EXISTS(SELECT * " +
                                       "FROM phanquyen pq " +
                                       "WHERE pq.MaChucVu=@maChucVu) " +
                        "AND NOT EXISTS(SELECT * " +
                                       "FROM nhanvien nv " +
                                       "WHERE nv.MaChucVu=@maChucVu);";

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

                string queryString = "INSERT INTO chucvu(TenChucVu) " +
                                    "SELECT @tenChucVu " +
                                    "WHERE NOT EXISTS(SELECT * " +
                                                    "FROM chucvu " +
                                                    "WHERE TenChucVu=@tenChucVu)";

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
                                                    "WHERE TenChucVu=@tenChucVu)";

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
                    "WHERE pq.MaChucVu = @maChucVu " +
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

        public bool SetOTPForNhanVien(string otp, string email)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "UPDATE nhanvien " +
                    "SET OTP=@otp, ThoiGianLayOTP=@now " +
                    "WHERE Email=@email AND MaChucVu<>0;";

                MySqlCommand cmd = new MySqlCommand(query, connectioncheck);
                cmd.Parameters.AddWithValue("otp", otp);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("email", email);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                return true;
            }
        }

        private string GeneratePassword()
        {
            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public int ResetPassword(string otp, string email)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string str = "SELECT ThoiGianLayOTP, OTP FROM nhanvien WHERE Email=@email";
                MySqlCommand cmd = new MySqlCommand(str, connectioncheck);
                cmd.Parameters.AddWithValue("email", email);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        DateTime OTPTime = Convert.ToDateTime(reader["ThoiGianLayOTP"]);
                        DateTime _now = DateTime.Now;
                        if ((int)(_now - OTPTime).TotalSeconds > 180)
                        {
                            return 1;
                        }
                        else
                        {
                            string otpInDatabase= reader["OTP"].ToString();
                            string matKhau = GeneratePassword();
                            if (otpInDatabase == otp)
                            {
                                reader.Close();
                                string queryUpdate = "UPDATE nhanvien " +
                                                    "SET MatKhau=@matKhau " +
                                                    "WHERE Email=@email;";
                                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connectioncheck);
                                cmdUpdate.Parameters.AddWithValue("matKhau", matKhau);
                                cmdUpdate.Parameters.AddWithValue("email", email);
                                if (cmdUpdate.ExecuteNonQuery() > 0)
                                {
                                    Mailer mail = new Mailer();
                                    string bodyMail = "<h2>Chào bạn</h2><p>Mật khẩu mới của của bạn là:" + matKhau + "</p>";
                                    if (mail.Send(email, "Mã OTP", bodyMail) == "OK")
                                    {
                                        return 2;
                                    }
                                    else
                                    {
                                        return 5;
                                    }
                                }
                            }
                            return 3;
                        }
                    }
                    else
                    {
                        return 4;
                    }
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
            return null;
            //using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            //{
            //    conn.Open();
            //    MySqlCommand cmd = new MySqlCommand(SQLQuery.updateCheckOutKhach, conn);
            //    cmd.Parameters.AddWithValue("thoiGianCO", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
            //    cmd.ExecuteNonQuery();

            //    cmd = new MySqlCommand(SQLQuery.getCheckInCheckOutDetail, conn);
            //    cmd.Parameters.AddWithValue("maCTHD", maCTHD);
            //    var resultTimeRange = cmd.ExecuteReader();
            //    resultTimeRange.Read();
            //    DateTime checkIn = Convert.ToDateTime(resultTimeRange["ThoiGianNhanPhong"]);
            //    DateTime checkOut = Convert.ToDateTime(resultTimeRange["ThoiGianTraPhong"]);
            //    resultTimeRange.Close();

            //    TimeSpan diff = checkOut - checkIn;

            //    List<DateTime> days = new List<DateTime>();
            //    if (diff.Hours > 10)
            //    {
            //        for (int i = 0; i <= diff.Days + 1; i++)
            //        {
            //            days.Add(checkIn.Date.AddDays(i));
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i <= diff.Days; i++)
            //        {
            //            days.Add(checkIn.Date.AddDays(i));
            //        }
            //    }

            //    List<ThongKeKhach> listThongKe = new List<ThongKeKhach>();

            //    // Convert timeCI timeCO
            //    string checkInString = checkIn.ToString("yyyy-MM-dd HH:mm:ss");
            //    string checkOutString = checkOut.ToString("yyyy-MM-dd HH:mm:ss");

            //    foreach (DateTime day in days)
            //    {
            //        string ngay = day.Year + "-" + day.Month + "-" + day.Day;
            //        cmd = new MySqlCommand(SQLQuery.countKhachNN, conn);
            //        cmd.Parameters.AddWithValue("maCTHD", maCTHD);
            //        cmd.Parameters.AddWithValue("thoiGianCO", ngay);
            //        var result = cmd.ExecuteReader();
            //        result.Read();

            //        ThongKeKhach newRecord = new ThongKeKhach();
            //        newRecord.ngay = ngay;
            //        newRecord.KhachNN = Convert.ToByte(result["KhachNN"]);
            //        result.Close();

            //        cmd = new MySqlCommand(SQLQuery.countKhachNoiDia, conn);
            //        cmd.Parameters.AddWithValue("maCTHD", maCTHD);
            //        cmd.Parameters.AddWithValue("thoiGianCO", ngay);
            //        result = cmd.ExecuteReader();
            //        result.Read();

            //        newRecord.KhachNoiDia = Convert.ToByte(result["KhachNoiDia"]);
            //        result.Close();
            //        listThongKe.Add(newRecord);
            //    }
            //    // Xong bang thong ke luong khach theo ngay

            //    Dictionary<string, byte> dictCount = new Dictionary<string, byte>();
            //    foreach (ThongKeKhach record in listThongKe)
            //    {
            //        // Struct key = KhachNoiDia + KhachNN
            //        string key = record.KhachNoiDia.ToString() + record.KhachNN.ToString();

            //        if (dictCount.ContainsKey(key))
            //        {
            //            dictCount[key]++;
            //        }
            //        else
            //        {
            //            dictCount.Add(key, 1);
            //        }
            //    }
            //    // Da co duoc Dict so luong khach theo loai khach va theo so luong ngay
            //    List<SoLuongKhachThue> kq = new List<SoLuongKhachThue>();
            //    foreach (KeyValuePair<string, byte> entry in dictCount)
            //    {
            //        string key = entry.Key;
            //        SoLuongKhachThue history = new SoLuongKhachThue();
            //        history.SoKhachNN = Convert.ToByte(key[1].ToString());
            //        history.SoKhachThue = Convert.ToByte(history.SoKhachNN + Convert.ToByte(key[0].ToString()));
            //        history.SoNgayThue = entry.Value;
            //        history.MaCTHD = maCTHD;
            //        cmd = new MySqlCommand(SQLQuery.getGiaPhongByDetailID, conn);
            //        cmd.Parameters.AddWithValue("maCTHD", maCTHD);
            //        var res = cmd.ExecuteReader();
            //        res.Read();
            //        history.DonGia = Convert.ToInt32(res["GiaTienCoBan"]);
            //        res.Close();
            //        if (history.SoKhachThue > 2)
            //        {
            //            cmd = new MySqlCommand(SQLQuery.getPhuThuTheoSoLuong, conn);
            //            cmd.Parameters.AddWithValue("soLuong", history.SoKhachThue);
            //            cmd.Parameters.AddWithValue("thoiGianCI", checkInString);
            //            var phuthu = cmd.ExecuteReader();
            //            phuthu.Read();
            //            if (!phuthu.HasRows)
            //            {
            //                history.PhuThu = 0;
            //            }
            //            else
            //            {
            //                if (phuthu["TiLePhuThu"] == DBNull.Value)
            //                {
            //                    history.PhuThu = Convert.ToInt32(0);
            //                }
            //                else
            //                    history.PhuThu = Convert.ToInt32(phuthu["TiLePhuThu"]);
            //            }
            //            phuthu.Close();
            //        }

            //        //Tinh he so lkh
            //        cmd = new MySqlCommand(SQLQuery.getHeSoLKH, conn);
            //        cmd.Parameters.AddWithValue("thoiGianCI", checkIn);
            //        cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
            //        cmd.Parameters.AddWithValue("soKhachND", Convert.ToByte(key[0].ToString()));
            //        var heso = cmd.ExecuteReader();
            //        heso.Read();
            //        if (heso["HeSo"] != DBNull.Value)
            //        {
            //            history.HeSoKhach = (float)Convert.ToDouble(heso["HeSo"]);
            //        }
            //        else
            //        {
            //            history.HeSoKhach = 1;
            //        }
            //        history.ThanhTien = TinhTien(history.DonGia, history.SoNgayThue, history.PhuThu, history.HeSoKhach);
            //        history.GhiChu = null;
            //        heso.Close();

            //        // Insert to DB
            //        cmd = new MySqlCommand(SQLQuery.insertSLKT, conn);
            //        cmd.Parameters.AddWithValue("soLuongKhach", history.SoKhachThue);
            //        cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
            //        cmd.Parameters.AddWithValue("soNgay", history.SoNgayThue);
            //        cmd.Parameters.AddWithValue("ghiChu", history.GhiChu);
            //        cmd.Parameters.AddWithValue("donGia", history.DonGia);
            //        cmd.Parameters.AddWithValue("phuThu", history.PhuThu);
            //        cmd.Parameters.AddWithValue("heSo", history.HeSoKhach);
            //        cmd.Parameters.AddWithValue("thanhTien", history.ThanhTien);
            //        cmd.Parameters.AddWithValue("maCTHD", history.MaCTHD);
            //        cmd.ExecuteNonQuery();
            //        kq.Add(history);
            //    }
            //    return kq;
            //}
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
            try
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
            catch (Exception)
            {

                throw;
            }
        }
        public List<Phong> getAllDetailRoom()
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }
public List<LoaiPhong> getAllRoomStyle()
{
try
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
catch (Exception)
{



throw;
}
}

        public string postNewRoom(Phong ph)
        {
       
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewRoom, conn);
                    cmd.Parameters.AddWithValue("MaPhong", ph.MaPhong);
                    cmd.Parameters.AddWithValue("MaLoaiPhong", ph.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("Tang", ph.Tang);
                    cmd.Parameters.AddWithValue("SoPhong", ph.SoPhong);
                    cmd.Parameters.AddWithValue("TrangThai", ph.TrangThai);
                    cmd.Parameters.AddWithValue("GhiChu", ph.GhiChu);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "success";
                }
                catch (Exception ex)
                {
                    return "fail";
                
                }
            }


        }
        public string postNewStaff(NhanVien nv)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    string queryGetmanv = "TaoMaNhanVien";
                    MySqlCommand cmdGetNV = new MySqlCommand(queryGetmanv, conn);
                    cmdGetNV.CommandType = CommandType.StoredProcedure;



                    cmdGetNV.Parameters.AddWithValue("_ngayVaoLam", nv.NgayVaoLam);
                    cmdGetNV.Parameters["_ngayVaoLam"].Direction = ParameterDirection.Input;



                    cmdGetNV.Parameters.Add("_maNhanVien", MySqlDbType.VarChar);
                    cmdGetNV.Parameters["_maNhanVien"].Direction = ParameterDirection.Output;



                    cmdGetNV.ExecuteNonQuery();
                    nv.MaNhanVien = cmdGetNV.Parameters["_maNhanVien"].Value.ToString();

                    string mk = GeneratePassword();
                    MySqlCommand cmd = new MySqlCommand(SQLQuery.postNewStaff, conn);

                    cmd.Parameters.AddWithValue("MaNhanVien", nv.MaNhanVien);
                    cmd.Parameters.AddWithValue("MatKhau", mk);
                    cmd.Parameters.AddWithValue("CCCD", nv.CCCD);
                    cmd.Parameters.AddWithValue("HoTen", nv.HoTen);
                    cmd.Parameters.AddWithValue("GioiTinh", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("NgaySinh", nv.NgaySinh);
                    cmd.Parameters.AddWithValue("Email", nv.Email);
                    cmd.Parameters.AddWithValue("SoDienThoai", nv.SoDienThoai);
                    cmd.Parameters.AddWithValue("NgayVaoLam", nv.NgayVaoLam);
                    cmd.Parameters.AddWithValue("MaChucVu", nv.MaChucVu);
                    cmd.Parameters.AddWithValue("HinhAnh", "/image/NhanVien/account.png");
               
                    cmd.Parameters.AddWithValue("Luong", nv.Luong);
                    cmd.ExecuteNonQuery();


                    conn.Close();
                    return "success";
                }
                catch
                {
                    return "fail";
                }
            }


        }
        public List<ChucVu> getAllDetailRoles()
{
try
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
catch (Exception)
{



throw;
}
}
        public NhanVien getChosenStaff(string maNhanVien)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }

        public Phong getChosenRoom(string maPhong)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQLQuery.getChosenRoom, conn);
                    Phong phong = new Phong();

                    cmd.Parameters.AddWithValue("MaPhong", maPhong);


                    using (var result = cmd.ExecuteReader())
                    {
                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                Phong ph = new Phong();
                                ph.MaPhong = result["MaPhong"].ToString();
                                ph.MaLoaiPhong = Convert.ToByte(result["MaLoaiPhong"]);
                                ph.Tang = Convert.ToByte(result["Tang"]);
                                ph.SoPhong = Convert.ToByte(result["SoPhong"]);
                                ph.GhiChu = (result["GhiChu"]).ToString();
                                ph.TrangThai = Convert.ToByte(result["TrangThai"]);

                                phong = ph;
                            }
                            conn.Close();
                            return phong;
                        }
                        else
                        {

                            conn.Close();
                            return null;
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UpdateStaff(NhanVien info_staff)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string str = " UPDATE `nhanvien` SET `CCCD`= @CCCD ,`HoTen`= @HoTen ,`SoDienThoai`= @SoDienThoai,`NgaySinh`= @NgaySinh, Email = @Email, GioiTinh = @GioiTinh, MaChucVu = @MaChucVu, Luong = @Luong, MatKhau = @MatKhau, NgayVaoLam = @NgayVaoLam, MaNhanVien = @MaNhanVien WHERE MaNhanVien = @MaNhanVien ";
                    MySqlCommand cmd = new MySqlCommand(str, conn);
                    cmd.Parameters.AddWithValue("CCCD", info_staff.CCCD);
                    cmd.Parameters.AddWithValue("SoDienThoai", info_staff.SoDienThoai);
                    cmd.Parameters.AddWithValue("NgaySinh", info_staff.NgaySinh);
                    cmd.Parameters.AddWithValue("Email", info_staff.Email);
                    cmd.Parameters.AddWithValue("GioiTinh", info_staff.GioiTinh);
                    cmd.Parameters.AddWithValue("MaNhanVien", info_staff.MaNhanVien);
                    cmd.Parameters.AddWithValue("HoTen", info_staff.HoTen);
                    cmd.Parameters.AddWithValue("Luong", info_staff.Luong);
                    cmd.Parameters.AddWithValue("MaChucVu", info_staff.MaChucVu);
                    cmd.Parameters.AddWithValue("NgayVaoLam", info_staff.NgayVaoLam);

                    cmd.Parameters.AddWithValue("MatKhau", info_staff.MatKhau);


                   cmd.ExecuteNonQuery();
                    return "success";
                }

               
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string UpdateRoom(Phong ph)
        {
            try
            {
                using (MySqlConnection connectioncheck = this.GetConnection())
                {
                    connectioncheck.Open();

                    string queryString = "UPDATE Phong " +
                        "SET maLoaiPhong=@maLoaiPhong, Tang=@tang, TrangThai = @trangThai, GhiChu = @ghiChu, SoPhong = @soPhong, MaPhong = @maPhong " +
                        "WHERE MaPhong=@maPhong ";

                    MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                    cmd.Parameters.AddWithValue("maLoaiPhong", ph.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("maPhong", ph.MaPhong);
                    cmd.Parameters.AddWithValue("trangThai", ph.TrangThai);
                    cmd.Parameters.AddWithValue("ghiChu", ph.GhiChu);
                    cmd.Parameters.AddWithValue("tang", ph.Tang);
                    cmd.Parameters.AddWithValue("soPhong", ph.SoPhong);

                   cmd.ExecuteNonQuery();
                    return "success";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        //1 đã xài, 0 chưa xài bao giờ
        public bool IsStaffUsed(string MaNhanVien)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();
                string queryString = " SELECT DISTINCT hoadon.MaNhanVien FROM hoadon WHERE EXISTS(SELECT nhanvien.MaNhanVien FROM nhanvien) AND MaNhanVien = @manv ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("manv", MaNhanVien);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                        return true;
                }
            }
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();
                string queryString = " SELECT DISTINCT traluong.MaNhanVien FROM traluong WHERE EXISTS(SELECT nhanvien.MaNhanVien FROM nhanvien) AND MaNhanVien = @Manv ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("Manv", MaNhanVien);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                        return true;
                }
            }
            return false;
        }

        public bool IsRoomUsed(string MaPhong)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();
                string queryString = " SELECT DISTINCT cthd.MaPhong FROM chitiethoadon cthd WHERE EXISTS(SELECT MaPhong FROM phong) AND MaPhong = @map ";

                MySqlCommand cmd = new MySqlCommand(queryString, connectioncheck);
                cmd.Parameters.AddWithValue("map", MaPhong);
                using (var result = cmd.ExecuteReader())
                {
                    if (result.HasRows)
                        return true;
                    else
                        return false;
                }
            }
        }

        public string DeleteStaff(string MaNhanVien)
        {
            //nv chua duoc dung => xoa cmn luon
            if (!IsStaffUsed(MaNhanVien))
            {

                using (MySqlConnection connectioncheck = this.GetConnection())
                {
                    connectioncheck.Open();
                    string str = " DELETE FROM nhanvien WHERE  MaNhanVien = @manv ";
                    MySqlCommand cmd = new MySqlCommand(str, connectioncheck);
                    cmd.Parameters.AddWithValue("manv", MaNhanVien);
                    int check = cmd.ExecuteNonQuery();
                    if (check >= 1)
                        return "success";
                    else
                        return "fail";
                }
            }
            //neu da ton tai thi update lai status => chức vụ đã sa thải
            else
            {
                int MaChucVuSaThai = 0;
                using (MySqlConnection connectioncheck = this.GetConnection())
                {
                    connectioncheck.Open();
                    string str = " SELECT `MaChucVu` FROM `chucvu` WHERE TenChucVu = 'Đã sa thải' ";
                    MySqlCommand cmd = new MySqlCommand(str, connectioncheck);
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            MaChucVuSaThai = Convert.ToInt32(result["MaChucVu"]);
                        }
                    }
                }

                using (MySqlConnection connectioncheck = this.GetConnection())
                {
                    connectioncheck.Open();
                    string str = " UPDATE `nhanvien` SET MaChucVu = @machucvu WHERE nhanvien.MaNhanVien = @manv ";
                    MySqlCommand cmd = new MySqlCommand(str, connectioncheck);
                    cmd.Parameters.AddWithValue("manv", MaNhanVien);
                    cmd.Parameters.AddWithValue("machucvu", MaChucVuSaThai);
                    int check = cmd.ExecuteNonQuery();
                    if (check >= 1)
                        return "fired";
                    else
                        return "fail";
                }
            }
        }

        public string DeleteRoom(string MaPhong)
        {
            //nv chua duoc dung => xoa cmn luon
            if (!IsRoomUsed(MaPhong))
            {

                using (MySqlConnection connectioncheck = this.GetConnection())
                {
                    connectioncheck.Open();
                    string str = " DELETE FROM phong WHERE MaPhong = @maphong ";
                    MySqlCommand cmd = new MySqlCommand(str, connectioncheck);
                    cmd.Parameters.AddWithValue("maphong", MaPhong);
                    int check = cmd.ExecuteNonQuery();
                    if (check >= 1)
                        return "success";
                    else
                        return "fail";
                }
            }
            //neu da ton tai thi update lai status => chức vụ đã sa thải
            else
            {

                using (MySqlConnection connectioncheck = this.GetConnection())
                {
                    connectioncheck.Open();
                    string str = " UPDATE `phong` SET TrangThai = @matrangthai WHERE MaPhong = @map ";
                    MySqlCommand cmd = new MySqlCommand(str, connectioncheck);
                    cmd.Parameters.AddWithValue("map", MaPhong);
                    cmd.Parameters.AddWithValue("matrangthai", 5);
                    int check = cmd.ExecuteNonQuery();
                    if (check >= 1)
                        return "fired";
                    else
                        return "fail";
                }
            }
        }

        public int Reset_Password(string email)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();
                string matKhau = GeneratePassword();
                string queryUpdate = "UPDATE nhanvien " +
                                                   "SET MatKhau=@matKhau " +
                                                   "WHERE Email=@email;";
                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connectioncheck);
                cmdUpdate.Parameters.AddWithValue("matKhau", matKhau);
                cmdUpdate.Parameters.AddWithValue("email", email);
                if (cmdUpdate.ExecuteNonQuery() > 0)
                {
                    Mailer mail = new Mailer();
                    string bodyMail = "<h2>Chào bạn</h2><p>Mật khẩu mới của của bạn là:" + matKhau + "</p>";
                    if (mail.Send(email, "Mã OTP", bodyMail) == "OK")
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                return 3;
            }
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
                string str = "  SELECT dottraluong.MaDotTraLuong , traluong.MaNhanVien , nhanvien.HoTen , chucvu.TenChucVu, nhanvien.CCCD , `Thuong`, `Phat`, `GhiChu`, `SoTien` ,nhanvien.GioiTinh FROM traluong, nhanvien, chucvu, dottraluong WHERE traluong.MaDotTraLuong = dottraluong.MaDotTraLuong AND nhanvien.MaNhanVien = traluong.MaNhanVien AND traluong.MaChucVu = chucvu.MaChucVu AND Month(dottraluong.NgayTraLuong) = @thang AND YEAR(dottraluong.NgayTraLuong) = @nam AND TenChucVu != 'Đã sa thải' ";
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
                string str = " SELECT `MaLoaiPhong`, `TenLoaiPhong` FROM `loaiphong`  ";
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
                string str = " SELECT `MaChucVu`, `TenChucVu` FROM `chucvu` ";
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
                                    if(TongTien == 0)
                                        list_chucvu[i].TiLe = 0;
                                    else
                                    {
                                        double tt = list_chucvu[i].TongLuong * 1.0 / TongTien * 100;
                                        list_chucvu[i].TiLe = Math.Round(tt,2);
                                    }

                                }
                            }
                        }
                        return list_chucvu;
                    }
                    else
                    {
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


        // Vinh - Hóa đơn + Tra cứu thuê phòng - Begin

        public List<object> LoadDataForDSHD()
        {
            List<object> dsHoaDon = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT HD.MaHoaDon, HD.ThoiGianXuat, NV.HoTen, HD.TongSoTien, HD.DoiTuongThanhToan " +
                    "FROM HOADON HD, NHANVIEN NV " +
                    "WHERE NV.MaNhanVien = HD.MaNhanVien ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader readerResult = cmd.ExecuteReader())
                {
                    while (readerResult.Read())
                    {
                        dsHoaDon.Add(new
                        {
                            maHoaDon = Convert.ToInt32(readerResult["MaHoaDon"]),
                            thoiGianXuat = Convert.ToDateTime(readerResult["ThoiGianXuat"]),
                            hoTenNV = readerResult["HoTen"].ToString(),
                            tongSoTien = Convert.ToInt32(readerResult["TongSoTien"]),
                            doiTuongThanhToan = readerResult["DoiTuongThanhToan"]
                        });
                    }
                }
                return dsHoaDon;
            }
        }
        public CTHD1Phong ThongKeSLKTCho1Phong(string maPhong, DateTime _curTGTraPhong)
        {
            List<KhachThue> dsKhachThue = new List<KhachThue>();
            int _curMaCTHD = 0;
            int _curGiaPhong = 0;
            string _curMaPhong = "";
            DateTime _curTGNhanPhong = DateTime.Now;
            List<PhuThuLKH> dsPhuThuLKH = new List<PhuThuLKH>();
            List<PhuThu> dsPhuThuSoKhach = new List<PhuThu>();
            List<PhuThu> dsPhuThuCheckin = new List<PhuThu>();
            List<PhuThu> dsPhuThuCheckout = new List<PhuThu>();
            List<object> dsSoLuongTheoLoaiKhach = new List<object>();
            string[] checkInTime = { "00", "00" };
            string[] checkOutTime = { "00", "00" };
            int overCheckIn = 0;
            int overCheckOut = 0;
            int tiLePhuThuChechIn = 0;
            int tiLePhuThuChechOut = 0;

            CTHD1Phong CTHDPhong = new CTHD1Phong();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //Lấy chi tiết hóa đơn của phòng
                string queryCTHD = "SELECT CTHD.MaCTHD, CTHD.MaPhong, CTHD.ThoiGianNhanPhong, CTHD.GiaPhong " +
                    "FROM CHITIETHOADON CTHD " +
                    "WHERE CTHD.MaPhong = @maPhong AND CTHD.MaHoaDon IS NULL AND CTHD.TrangThai = 0 ";
                MySqlCommand cmdCTHD = new MySqlCommand(queryCTHD, conn);
                cmdCTHD.Parameters.AddWithValue("maPhong", maPhong);
                using (MySqlDataReader readerCTHD = cmdCTHD.ExecuteReader())
                {
                    if (readerCTHD.HasRows)
                    {
                        readerCTHD.Read();
                        _curMaCTHD = Convert.ToInt32(readerCTHD["MaCTHD"]);
                        _curMaPhong = readerCTHD["MaPhong"].ToString();
                        _curTGNhanPhong = Convert.ToDateTime(readerCTHD["ThoiGianNhanPhong"]);
                        _curGiaPhong = Convert.ToInt32(readerCTHD["GiaPhong"]);
                    }
                }
                CTHDPhong.MaCTHD = _curMaCTHD;
                CTHDPhong.MaPhong = maPhong;
                CTHDPhong.ThoiGianNhanPhong = _curTGNhanPhong;
                CTHDPhong.ThoiGianTraPhong = _curTGTraPhong;
                CTHDPhong.GiaPhong = _curGiaPhong;

                //Lấy quy định về phụ thu checkin
                string queryPhuThuCheckin = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Check In' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuCheckin = new MySqlCommand(queryPhuThuCheckin, conn);
                cmdPhuThuCheckin.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuCheckin = cmdPhuThuCheckin.ExecuteReader())
                {
                    if (readerPhuThuCheckin.HasRows)
                    {
                        while (readerPhuThuCheckin.Read())
                        {
                            dsPhuThuCheckin.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuCheckin["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuCheckin["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuCheckin["SoLuongApDung"]),
                                TenLoaiPhuThu = "Checkin",
                                ThoiGianApDung = readerPhuThuCheckin["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuCheckin["TiLePhuThu"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu checkout
                string queryPhuThuCheckout = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Check Out' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuCheckout = new MySqlCommand(queryPhuThuCheckout, conn);
                cmdPhuThuCheckout.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuCheckout = cmdPhuThuCheckout.ExecuteReader())
                {
                    if (readerPhuThuCheckout.HasRows)
                    {
                        while (readerPhuThuCheckout.Read())
                        {
                            dsPhuThuCheckout.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuCheckout["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuCheckout["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuCheckout["SoLuongApDung"]),
                                TenLoaiPhuThu = "Checkout",
                                ThoiGianApDung = readerPhuThuCheckout["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuCheckout["TiLePhuThu"])
                            });
                        }
                    }
                }

                //Lấy thông tin giờ checkin checkout
                string queryGioCheckInOut = "SELECT * " +
                    "FROM THAMSO TS " +
                    "WHERE TS.TenThuocTinh = 'GioCheckIn' OR TS.TenThuocTinh = 'GioCheckOut'";
                MySqlCommand cmdGioCheckInOut = new MySqlCommand(queryGioCheckInOut, conn);
                using (MySqlDataReader readerGioCheckInOut = cmdGioCheckInOut.ExecuteReader())
                {
                    if (readerGioCheckInOut.HasRows)
                    {
                        while (readerGioCheckInOut.Read())
                        {
                            if (readerGioCheckInOut["TenThuocTinh"].ToString() == "GioCheckIn")
                            {
                                checkInTime = readerGioCheckInOut["GiaTri"].ToString().Split(":");
                            }
                            else if (readerGioCheckInOut["TenThuocTinh"].ToString() == "GioCheckOut")
                            {
                                checkOutTime = readerGioCheckInOut["GiaTri"].ToString().Split(":");
                            }
                        }
                    }
                }
                //Tính checkin sớm, checkout trễ
                if (_curTGNhanPhong.Hour < Convert.ToInt32(checkInTime[0]))
                {
                    overCheckIn = (int)Math.Round((double)((Convert.ToInt32(checkInTime[0]) - _curTGNhanPhong.Hour) * 60 + (Convert.ToInt32(checkInTime[1]) - _curTGNhanPhong.Minute)) / 60);
                }
                if (_curTGTraPhong.Hour > Convert.ToInt32(checkOutTime[0]))
                {
                    overCheckOut = (int)Math.Round((double)((_curTGTraPhong.Hour - Convert.ToInt32(checkOutTime[0])) * 60 + (_curTGTraPhong.Minute) - Convert.ToInt32(checkOutTime[1])) / 60);
                }
                foreach (PhuThu phuThuCheckIn in dsPhuThuCheckin)
                {
                    if (phuThuCheckIn.SoLuongApDung <= overCheckIn && phuThuCheckIn.TiLePhuThu > tiLePhuThuChechIn)
                    {
                        tiLePhuThuChechIn = phuThuCheckIn.TiLePhuThu;
                    }
                }
                foreach (PhuThu phuThuCheckOut in dsPhuThuCheckout)
                {
                    if (phuThuCheckOut.SoLuongApDung <= overCheckOut && phuThuCheckOut.TiLePhuThu > tiLePhuThuChechOut)
                    {
                        tiLePhuThuChechOut = phuThuCheckOut.TiLePhuThu;
                    }
                }

                //Lấy danh sách khách thuê của phòng
                string queryKhachThue = "SELECT KT.MaKhachThue, KT.CCCD, KT.ThoiGianCheckIn, KT.ThoiGianCheckOut, KT.HoTen, KT.MaLoaiKhachHang, KT.DiaChi, KT.MaCTHD " +
                    "FROM KHACHTHUE KT, CHITIETHOADON CTHD " +
                    "WHERE KT.MaCTHD = CTHD.MaCTHD AND CTHD.MaPhong = @maPhong AND CTHD.MaHoaDon IS NULL AND CTHD.TrangThai = 0 ";
                MySqlCommand cmdKhachThue = new MySqlCommand(queryKhachThue, conn);
                cmdKhachThue.Parameters.AddWithValue("maPhong", maPhong);
                using (MySqlDataReader readerKhachThue = cmdKhachThue.ExecuteReader())
                {
                    if (readerKhachThue.HasRows)
                    {
                        while (readerKhachThue.Read())
                        {
                            dsKhachThue.Add(new KhachThue
                            {
                                MaKhachThue = Convert.ToInt32(readerKhachThue["MaKhachThue"]),
                                CCCD = readerKhachThue["CCCD"].ToString(),
                                ThoiGianCheckin = Convert.ToDateTime(readerKhachThue["ThoiGianCheckIn"]),
                                ThoiGianCheckout = (readerKhachThue["ThoiGianCheckOut"] == DBNull.Value) ? (_curTGTraPhong) : Convert.ToDateTime(readerKhachThue["ThoiGianCheckOut"]),
                                HoTen = readerKhachThue["HoTen"].ToString(),
                                MaLoaiKhachHang = Convert.ToInt32(readerKhachThue["MaLoaiKhachHang"]),
                                DiaChi = readerKhachThue["DiaChi"].ToString(),
                                MaCTHD = Convert.ToInt32(readerKhachThue["MaCTHD"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu loại khách hàng
                string queryPhuThuLKH = "SELECT * " +
                    "FROM PHUTHULKH PTLKH " +
                    "WHERE PTLKH.ThoiGianApDung <= @thoiGianNhanPhong AND PTLKH.ThoiGianApDung = (SELECT MAX(PT.ThoiGianApDung) " +
                    "                                                                                FROM PHUTHULKH PT " +
                    "                                                                                WHERE PT.MaLoaiKhachHang = PTLKH.MaLoaiKhachHang) ";
                MySqlCommand cmdPhuThuLKH = new MySqlCommand(queryPhuThuLKH, conn);
                cmdPhuThuLKH.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuLKH = cmdPhuThuLKH.ExecuteReader())
                {
                    if (readerPhuThuLKH.HasRows)
                    {
                        while (readerPhuThuLKH.Read())
                        {
                            dsPhuThuLKH.Add(new PhuThuLKH
                            {
                                MaPhuThuLKH = Convert.ToInt32(readerPhuThuLKH["MaPhuThuLKH"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuLKH["SoLuongApDung"]),
                                HeSoPhuThu = Math.Round(Convert.ToDouble(readerPhuThuLKH["HeSoPhuThu"]), 1),
                                ThoiGianApDung = "",
                                MaLoaiKhachHang = Convert.ToInt32(readerPhuThuLKH["MaLoaiKhachHang"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu theo số khách 
                string queryPhuThuSoKhach = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Theo số khách' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuSoKhach = new MySqlCommand(queryPhuThuSoKhach, conn);
                cmdPhuThuSoKhach.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuSoKhach = cmdPhuThuSoKhach.ExecuteReader())
                {
                    if (readerPhuThuSoKhach.HasRows)
                    {
                        while (readerPhuThuSoKhach.Read())
                        {
                            dsPhuThuSoKhach.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuSoKhach["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuSoKhach["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuSoKhach["SoLuongApDung"]),
                                TenLoaiPhuThu = "Theo số khách",
                                ThoiGianApDung = readerPhuThuSoKhach["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuSoKhach["TiLePhuThu"])
                            });
                        }
                    }
                }




            }
            double _heSoPhuThuMax = 1;
            int _phuThuSoKhach = 0;

            List<SoLuongKhachThue> dsSLKTTungNgay = new List<SoLuongKhachThue>();
            List<SoLuongKhachThue> dsSLKTFinal = new List<SoLuongKhachThue>();
            if (_curTGNhanPhong.Date < _curTGTraPhong.Date)
            {
                for (DateTime date = _curTGNhanPhong; date.Date.CompareTo(_curTGTraPhong.Date) < 0; date = date.AddDays(1.0))
                {
                    var resultCount = from kt in dsKhachThue.Where(kt => kt.ThoiGianCheckin.Date <= date.Date && kt.ThoiGianCheckout.Date > date.Date)
                                                                .GroupBy(kt => kt.MaLoaiKhachHang)
                                      select new
                                      {
                                          count = kt.Count(),
                                          kt.First().MaLoaiKhachHang
                                      };
                    byte _countTongSoKhach = Convert.ToByte(dsKhachThue.Count(kt => kt.ThoiGianCheckin.Date <= date.Date && kt.ThoiGianCheckout.Date > date.Date));
                    foreach (var _reCount in resultCount)
                    {
                        foreach (var phuThuLKH in dsPhuThuLKH)
                        {
                            if (phuThuLKH.SoLuongApDung <= _reCount.count && _reCount.MaLoaiKhachHang == phuThuLKH.MaLoaiKhachHang && _heSoPhuThuMax < phuThuLKH.HeSoPhuThu)
                            {
                                _heSoPhuThuMax = phuThuLKH.HeSoPhuThu;
                            }
                        }
                    }
                    foreach (PhuThu phuThu in dsPhuThuSoKhach)
                    {
                        if (phuThu.SoLuongApDung <= _countTongSoKhach && phuThu.TiLePhuThu > _phuThuSoKhach)
                        {
                            _phuThuSoKhach = phuThu.TiLePhuThu;
                        }
                    }
                    dsSLKTTungNgay.Add(new SoLuongKhachThue
                    {
                        SoKhachThue = _countTongSoKhach,
                        PhuThu = _phuThuSoKhach,
                        HeSoKhach = _heSoPhuThuMax,
                    });
                    _countTongSoKhach = 0;
                    _heSoPhuThuMax = 1;
                    _phuThuSoKhach = 0;
                }
            }
            else if (_curTGNhanPhong.Date == _curTGTraPhong.Date)
            {
                var resultCount = from kt in dsKhachThue.GroupBy(kt => kt.MaLoaiKhachHang)
                                  select new
                                  {
                                      count = kt.Count(),
                                      kt.First().MaLoaiKhachHang
                                  };
                byte _countTongSoKhach = Convert.ToByte(dsKhachThue.Count());
                foreach (var _reCount in resultCount)
                {
                    foreach (var phuThuLKH in dsPhuThuLKH)
                    {
                        if (phuThuLKH.SoLuongApDung <= _reCount.count && _reCount.MaLoaiKhachHang == phuThuLKH.MaLoaiKhachHang && _heSoPhuThuMax < phuThuLKH.HeSoPhuThu)
                        {
                            _heSoPhuThuMax = phuThuLKH.HeSoPhuThu;
                        }
                    }
                }
                foreach (PhuThu phuThu in dsPhuThuSoKhach)
                {
                    if (phuThu.SoLuongApDung <= _countTongSoKhach && phuThu.TiLePhuThu > _phuThuSoKhach)
                    {
                        _phuThuSoKhach = phuThu.TiLePhuThu;
                    }
                }
                dsSLKTTungNgay.Add(new SoLuongKhachThue
                {
                    SoKhachThue = _countTongSoKhach,
                    PhuThu = _phuThuSoKhach,
                    HeSoKhach = _heSoPhuThuMax,
                });
                _countTongSoKhach = 0;
                _heSoPhuThuMax = 1;
                _phuThuSoKhach = 0;
                tiLePhuThuChechOut = 0;
            }

            if (dsSLKTTungNgay.Count > 0)
            {
                SoLuongKhachThue slktDangXet = new SoLuongKhachThue();
                int tongTienSLKT = 0;
                while (dsSLKTTungNgay.Count > 0)
                {
                    slktDangXet.SoKhachThue = dsSLKTTungNgay[0].SoKhachThue;
                    slktDangXet.PhuThu = dsSLKTTungNgay[0].PhuThu;
                    slktDangXet.HeSoKhach = dsSLKTTungNgay[0].HeSoKhach;
                    slktDangXet.SoNgayThue = 0;
                    slktDangXet.DonGia = _curGiaPhong;
                    slktDangXet.MaCTHD = _curMaCTHD;


                    for (int index = 0; index < dsSLKTTungNgay.Count; index++)
                    {
                        if (dsSLKTTungNgay[index].SoKhachThue == slktDangXet.SoKhachThue && dsSLKTTungNgay[index].PhuThu == slktDangXet.PhuThu && dsSLKTTungNgay[index].HeSoKhach == slktDangXet.HeSoKhach)
                        {
                            slktDangXet.SoNgayThue++;
                            dsSLKTTungNgay.RemoveAt(index);
                            index--;
                        }
                    }
                    slktDangXet.ThanhTien = (int)((double)(slktDangXet.HeSoKhach) * (slktDangXet.DonGia + (slktDangXet.DonGia * (slktDangXet.PhuThu / (double)100))) * slktDangXet.SoNgayThue);
                    tongTienSLKT = tongTienSLKT + slktDangXet.ThanhTien;
                    dsSLKTFinal.Add(new SoLuongKhachThue {
                        SoKhachThue = slktDangXet.SoKhachThue,
                        PhuThu = (int)((slktDangXet.PhuThu / (double)100) * slktDangXet.DonGia),
                        HeSoKhach = slktDangXet.HeSoKhach,
                        SoNgayThue = slktDangXet.SoNgayThue,
                        DonGia = slktDangXet.DonGia,
                        ThanhTien = slktDangXet.ThanhTien,
                        MaCTHD = slktDangXet.MaCTHD,
                        GhiChu = "Phụ thu: " + slktDangXet.PhuThu.ToString() + "%\nHệ số khách: "+ slktDangXet.HeSoKhach.ToString()
                    });
                }


                CTHDPhong.DsSoLuongKhachThue = dsSLKTFinal;
                CTHDPhong.PhuThuCICO = (int)(((double)(tiLePhuThuChechIn + tiLePhuThuChechOut) / 100) * CTHDPhong.GiaPhong);
                CTHDPhong.TongTienPhong = (int)(tongTienSLKT + CTHDPhong.PhuThuCICO);
                CTHDPhong.GhiChu = "Checkin sớm: " + tiLePhuThuChechIn.ToString() + "%\nCheckout trễ: " + tiLePhuThuChechOut.ToString() + "%";

                return CTHDPhong;
            }
            else return null;
        }

        public object CreateNewReceipt(string strPhongs, string doiTuongThanhToan, string maNhanVien)
        {
            List<CTHD1Phong> hoaDons = new List<CTHD1Phong>();
            object response;
            strPhongs = strPhongs.Remove(strPhongs.Length - 1);
            DateTime _curThoiGianTraPhong = DateTime.Now;
            string[] dsPhong = strPhongs.Split("@");
            int tongTienHoaDon = 0;
            foreach (string phong in dsPhong)
            {
                CTHD1Phong result = ThongKeSLKTCho1Phong(phong, _curThoiGianTraPhong);
                hoaDons.Add(result);
                tongTienHoaDon = tongTienHoaDon + result.TongTienPhong;
            }
            if (hoaDons.Count > 0)
            {
                int maHoaDon = 0;
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string queryInsertHoaDon = "INSERT INTO HOADON(ThoiGianXuat, MaNhanVien, TongSoTien, DoiTuongThanhToan) " +
                        "VALUES (@thoiGianXuat, @maNhanVien, @tongSoTien, @doiTuongThanhTOan); " +
                        "SELECT LAST_INSERT_ID() AS 'MaHoaDon';";
                    MySqlCommand cmdInsertHoaDon = new MySqlCommand(queryInsertHoaDon, conn);
                    cmdInsertHoaDon.Parameters.AddWithValue("thoiGianXuat", _curThoiGianTraPhong);
                    cmdInsertHoaDon.Parameters.AddWithValue("maNhanVien", maNhanVien);
                    cmdInsertHoaDon.Parameters.AddWithValue("tongSoTien", tongTienHoaDon);
                    cmdInsertHoaDon.Parameters.AddWithValue("doiTuongThanhTOan", doiTuongThanhToan);
                    using (MySqlDataReader readerInsertHoaDon = cmdInsertHoaDon.ExecuteReader())
                    {
                        if (readerInsertHoaDon.HasRows)
                        {
                            readerInsertHoaDon.Read();
                            maHoaDon = Convert.ToInt32(readerInsertHoaDon["MaHoaDon"]);
                        }
                    }
                    foreach (CTHD1Phong hoaDon in hoaDons)
                    {
                        if (hoaDon.DsSoLuongKhachThue.Count > 0)
                        {
                            string queryInsertSLKT = "INSERT INTO SOLUONGKHACHTHUE(SoKhachThue, PhuThu, HeSoKhach, SoNgayThue, GhiChu, DonGia, ThanhTien, MaCTHD) VALUES ";
                            foreach (SoLuongKhachThue soLuongKhachThue in hoaDon.DsSoLuongKhachThue)
                            {
                                queryInsertSLKT = queryInsertSLKT + "(" + soLuongKhachThue.SoKhachThue + "," + soLuongKhachThue.PhuThu + ","
                                     + soLuongKhachThue.HeSoKhach + "," + soLuongKhachThue.SoNgayThue + ", N'" + soLuongKhachThue.GhiChu + "',"
                                     + soLuongKhachThue.DonGia + "," + soLuongKhachThue.ThanhTien + "," + soLuongKhachThue.MaCTHD + "),";
                            }
                            queryInsertSLKT = queryInsertSLKT.Remove(queryInsertSLKT.Length - 1);
                            MySqlCommand cmdInsertSLKT = new MySqlCommand(queryInsertSLKT, conn);
                            cmdInsertSLKT.ExecuteNonQuery();

                            string queryUpdateCTHD = "UPDATE CHITIETHOADON " +
                                "SET MaHoaDon = @maHoaDon, ThoiGianTraPhong = @thoiGianTraPhong, PhuThuCICO = @phuThuCICO, GhiChu = @ghiChu, TongTienPhong = @tongTienPhong, TrangThai = 1 " +
                                "WHERE MaCTHD = @maCTHD";
                            MySqlCommand cmdUpdateCTHD = new MySqlCommand(queryUpdateCTHD, conn);
                            cmdUpdateCTHD.Parameters.AddWithValue("maHoaDon", maHoaDon);
                            cmdUpdateCTHD.Parameters.AddWithValue("thoiGianTraPhong", _curThoiGianTraPhong);
                            cmdUpdateCTHD.Parameters.AddWithValue("phuThuCICO", hoaDon.PhuThuCICO);
                            cmdUpdateCTHD.Parameters.AddWithValue("ghiChu", hoaDon.GhiChu);
                            cmdUpdateCTHD.Parameters.AddWithValue("tongTienPhong", hoaDon.TongTienPhong);
                            cmdUpdateCTHD.Parameters.AddWithValue("maCTHD", hoaDon.MaCTHD);
                            cmdUpdateCTHD.ExecuteNonQuery();

                            string queryUpdateStatusRoom = "UPDATE PHONG " +
                               "SET TrangThai = 4 " +
                               "WHERE MaPhong = @maPhong";
                            MySqlCommand cmdUpdateStatusRoom = new MySqlCommand(queryUpdateStatusRoom, conn);
                            cmdUpdateStatusRoom.Parameters.AddWithValue("maPhong", hoaDon.MaPhong);
                            cmdUpdateStatusRoom.ExecuteNonQuery();
                        }
                    }
                }
                response = new
                {
                    maHoaDon = maHoaDon,
                    thoiGianTraPhong = _curThoiGianTraPhong,
                    doiTuongThanhToan = doiTuongThanhToan,
                    dsChiTiet = hoaDons
                };
                return response;
            }
            return null;
        }

        public object GetCTHDPayRoom(string maPhong)
        {
            object infoCTHD;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT CTHD.ThoiGianNhanPhong, CTHD.MaPhong, LP.TenLoaiPhong " +
                    "FROM CHITIETHOADON CTHD, PHONG P, LOAIPHONG LP " +
                    "WHERE CTHD.MaPhong = @maPhong AND CTHD.MaHoaDon IS NULL AND CTHD.TrangThai = 0 AND CTHD.MaPhong = P.MaPhong AND P.MaLoaiPhong = LP.MaLoaiPhong";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("maphong", maPhong);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        infoCTHD = new
                        {
                            maPhong = reader["MaPhong"].ToString(),
                            tenLoaiPhong = reader["TenLoaiPhong"].ToString(),
                            thoiGianNhanPhong = Convert.ToDateTime(reader["ThoiGianNhanPhong"])
                        };
                    }
                    else infoCTHD = null;
                }
            }
            return infoCTHD;
        }

        public object GetDetailOldReceipt(string maHoaDon)
        {
            object response;
            DateTime thoiGianTraPhong = DateTime.Now;
            string doiTuongThanhToan = "";
            string nhanVienThanhToan ="";
            int tongSoTienHD = 0;
            List<CTHD1Phong> cacCTHDs = new List<CTHD1Phong>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string queryThongTinDauHoaDon = "SELECT HD.ThoiGianXuat, NV.HoTen, HD.TongSoTien, HD.DoiTuongThanhToan " +
                    "FROM HOADON HD, NHANVIEN NV " +
                    "WHERE HD.MaNhanVien = NV.MaNhanVien AND HD.MaHoaDon = @maHoaDon ";
                MySqlCommand cmdThongTinDauHoaDon = new MySqlCommand(queryThongTinDauHoaDon, conn);
                cmdThongTinDauHoaDon.Parameters.AddWithValue("maHoaDon", maHoaDon);
                using(MySqlDataReader readerThongTinDauHoaDon = cmdThongTinDauHoaDon.ExecuteReader())
                {
                    if (readerThongTinDauHoaDon.HasRows)
                    {
                        readerThongTinDauHoaDon.Read();
                        thoiGianTraPhong = Convert.ToDateTime(readerThongTinDauHoaDon["ThoiGianXuat"]);
                        doiTuongThanhToan = readerThongTinDauHoaDon["DoiTuongThanhToan"].ToString();
                        nhanVienThanhToan = readerThongTinDauHoaDon["HoTen"].ToString();
                        tongSoTienHD = Convert.ToInt32(readerThongTinDauHoaDon["TongSoTien"]);
                    }    
                }

                string queryGetCTHDs = "SELECT CTHD.MaCTHD, CTHD.MaPhong, CTHD.MaHoaDon, CTHD.PhuThuCICO, CTHD.GiaPhong, CTHD.GhiChu, CTHD.TongTienPhong " +
                    "FROM CHITIETHOADON CTHD " +
                    "WHERE CTHD.MaHoaDon = @maHoaDon ";
                MySqlCommand cmdGetCTHDs = new MySqlCommand(queryGetCTHDs, conn);
                cmdGetCTHDs.Parameters.AddWithValue("maHoaDon", maHoaDon);
                using (MySqlDataReader readerGetCTHDs = cmdGetCTHDs.ExecuteReader())
                {
                    if (readerGetCTHDs.HasRows)
                    {
                        while(readerGetCTHDs.Read())
                        {
                            cacCTHDs.Add(new CTHD1Phong
                            {
                                MaCTHD = Convert.ToInt32(readerGetCTHDs["MaCTHD"]),
                                MaPhong = readerGetCTHDs["MaPhong"].ToString(),
                                MaHoaDon = Convert.ToInt32(readerGetCTHDs["MaHoaDon"]),
                                PhuThuCICO = Convert.ToInt32(readerGetCTHDs["PhuThuCICO"]),
                                GiaPhong = Convert.ToInt32(readerGetCTHDs["GiaPhong"]),
                                TongTienPhong = Convert.ToInt32(readerGetCTHDs["TongTienPhong"]),
                                GhiChu = readerGetCTHDs["GhiChu"].ToString(),
                                DsSoLuongKhachThue = new List<SoLuongKhachThue>()
                            });
                        }    
                    }    
                }
                if (cacCTHDs.Count > 0)
                {
                    foreach (CTHD1Phong motCTHD in cacCTHDs)
                    {
                        string queryGetSLKT = "SELECT SLKT.SoKhachThue, SLKT.PhuThu, SLKT.HeSoKhach, SLKT.SoNgayThue, SLKT.DonGia, SLKT.ThanhTien, SLKT.MaCTHD, SLKT.GhiChu " +
                            "FROM SOLUONGKHACHTHUE SLKT " +
                            "WHERE SLKT.MaCTHD = @maCTHD";
                        MySqlCommand cmdGetSLKT = new MySqlCommand(queryGetSLKT, conn);
                        cmdGetSLKT.Parameters.AddWithValue("maCTHD", motCTHD.MaCTHD);
                        using (MySqlDataReader readerGetSLKT = cmdGetSLKT.ExecuteReader())
                        {
                            if (readerGetSLKT.HasRows)
                            {
                                while (readerGetSLKT.Read())
                                {
                                    motCTHD.DsSoLuongKhachThue.Add(new SoLuongKhachThue
                                    {
                                        SoKhachThue = Convert.ToByte(readerGetSLKT["SoKhachThue"]),
                                        PhuThu = Convert.ToInt32(readerGetSLKT["PhuThu"]),
                                        HeSoKhach = Convert.ToDouble(readerGetSLKT["HeSoKhach"]),
                                        SoNgayThue = Convert.ToByte(readerGetSLKT["SoNgayThue"]),
                                        DonGia = Convert.ToInt32(readerGetSLKT["DonGia"]),
                                        ThanhTien = Convert.ToInt32(readerGetSLKT["ThanhTien"]),
                                        MaCTHD = Convert.ToInt32(readerGetSLKT["MaCTHD"]),
                                        GhiChu = readerGetSLKT["GhiChu"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                    response = new
                    {
                        maHoaDon = maHoaDon,
                        thoiGianTraPhong = thoiGianTraPhong,
                        doiTuongThanhToan = doiTuongThanhToan,
                        dsChiTiet = cacCTHDs,
                        nhanVienThanhToan = nhanVienThanhToan,
                        tongSoTienHD = tongSoTienHD
                    };
                    return response;
                }
                else return null;
            }
        }

        public List<object> LoadDataForDanhSachHoaDonByFilter(DateTime ngayHoaDon, int? maHoaDon)
        {
            List<object> dsHoaDon = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT HD.MaHoaDon, HD.ThoiGianXuat, NV.HoTen, HD.TongSoTien, HD.DoiTuongThanhToan " +
                    "FROM HOADON HD, NHANVIEN NV " +
                    "WHERE YEAR(@ngayHoaDon) = YEAR(HD.ThoiGianXuat) AND MONTH(@ngayHoaDon) = MONTH(HD.ThoiGianXuat) AND DAY(@ngayHoaDon) = DAY(HD.ThoiGianXuat) AND HD.MaHoaDon = @maHoaDon AND NV.MaNhanVien = HD.MaNhanVien ";
                if (ngayHoaDon == null)
                {
                    query = query.Replace("YEAR(@ngayHoaDon) = YEAR(HD.ThoiGianXuat) AND MONTH(@ngayHoaDon) = MONTH(HD.ThoiGianXuat) AND DAY(@ngayHoaDon) = DAY(HD.ThoiGianXuat) AND ", "");
                }
                if (maHoaDon == null)
                {
                    query = query.Replace("HD.MaHoaDon = @maHoaDon AND ", "");
                }
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("ngayHoaDon", ngayHoaDon);
                cmd.Parameters.AddWithValue("maHoaDon", maHoaDon);
                using (MySqlDataReader readerResult = cmd.ExecuteReader())
                {
                    while (readerResult.Read())
                    {
                        dsHoaDon.Add(new
                        {
                            maHoaDon = Convert.ToInt32(readerResult["MaHoaDon"]),
                            thoiGianXuat = Convert.ToDateTime(readerResult["ThoiGianXuat"]),
                            hoTenNV = readerResult["HoTen"].ToString(),
                            tongSoTien = Convert.ToInt32(readerResult["TongSoTien"]),
                            doiTuongThanhToan = readerResult["DoiTuongThanhToan"]
                        });
                    }
                }
                return dsHoaDon;
            }
        }

        //TRA CỨU THUÊ PHÒNG
        public List<object> LoadDataForDSTP()
        {
            List<object> dsThuePhong = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT CTHD.MaCTHD, CTHD.MaPhong, LP.TenLoaiPhong, CTHD.ThoiGianNhanPhong, CTHD.ThoiGianTraPhong, CTHD.TrangThai " +
                    "FROM CHITIETHOADON CTHD, PHONG P, LOAIPHONG LP " +
                    "WHERE CTHD.MaPhong = P.MaPhong AND P.MaLoaiPhong = LP.MaLoaiPhong";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader readerResult = cmd.ExecuteReader())
                {
                    while (readerResult.Read())
                    {
                        dsThuePhong.Add(new
                        {
                            maCTHD = Convert.ToInt32(readerResult["MaCTHD"]),
                            maPhong = readerResult["MaPhong"].ToString(),
                            tenLoaiPhong = readerResult["TenLoaiPhong"].ToString(),
                            thoiGianNhanPhong = Convert.ToDateTime(readerResult["ThoiGianNhanPhong"]),
                            thoiGianTraPhong = (readerResult["ThoiGianTraPhong"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(readerResult["ThoiGianTraPhong"]),
                            trangThai = Convert.ToInt32(readerResult["TrangThai"])
                        });
                    }
                }
                return dsThuePhong;
            }
        }

        public List<object> LoadDataForDSTPByFilter(string ngayDenO, string CCCD, string maPhong)
        {
            List<object> dsThuePhong = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT CTHD.MaCTHD, CTHD.MaPhong, LP.TenLoaiPhong, CTHD.ThoiGianNhanPhong, CTHD.ThoiGianTraPhong, CTHD.TrangThai " +
                    "FROM CHITIETHOADON CTHD, PHONG P, LOAIPHONG LP " +
                    "WHERE CTHD.MaPhong = P.MaPhong AND P.MaLoaiPhong = LP.MaLoaiPhong ";
                if (ngayDenO != null || ngayDenO != "")
                {
                    query = query + " AND ((DATE(CTHD.ThoiGianNhanPhong) <= @ngayDenO AND DATE(CTHD.ThoiGianTraPhong) >= @ngayDenO AND CTHD.ThoiGianTraPhong IS NOT NULL) OR (CTHD.ThoiGianTraPhong IS NULL AND DATE(CTHD.ThoiGianNhanPhong) <= @ngayDenO))";
                }
                if (CCCD != null && CCCD != "")
                {
                    query = query + " AND EXISTS(SELECT * FROM KHACHTHUE KT WHERE KT.MaCTHD = CTHD.MaCTHD AND KT.CCCD = @CCCD)";
                }
                if (maPhong != "" && maPhong != null)
                {
                    query = query + " AND CTHD.MaPhong = @maPhong";
                }
                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (ngayDenO != null && ngayDenO != "")
                {
                    cmd.Parameters.AddWithValue("ngayDenO", ngayDenO);
                }
                if (CCCD != null && CCCD != "")
                {
                    cmd.Parameters.AddWithValue("CCCD", CCCD);
                }
                if (maPhong != "" && maPhong != null)
                {
                    cmd.Parameters.AddWithValue("maPhong", maPhong);
                }
                using (MySqlDataReader readerResult = cmd.ExecuteReader())
                {
                    while (readerResult.Read())
                    {
                        dsThuePhong.Add(new
                        {
                            maCTHD = Convert.ToInt32(readerResult["MaCTHD"]),
                            maPhong = readerResult["MaPhong"].ToString(),
                            tenLoaiPhong = readerResult["TenLoaiPhong"].ToString(),
                            thoiGianNhanPhong = Convert.ToDateTime(readerResult["ThoiGianNhanPhong"]),
                            thoiGianTraPhong = (readerResult["ThoiGianTraPhong"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(readerResult["ThoiGianTraPhong"]),
                            trangThai = Convert.ToInt32(readerResult["TrangThai"])
                        });
                    }
                }
                return dsThuePhong;
            }
        }

        public CTHD1Phong GetPhieuThuePhongDaThanhToan(int maCTHD)
        {
            CTHD1Phong phieuThuePhong = new CTHD1Phong();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string queryGetCTHD = "SELECT CTHD.MaCTHD, CTHD.MaPhong, CTHD.MaHoaDon, CTHD.PhuThuCICO, CTHD.GiaPhong, CTHD.GhiChu, CTHD.TongTienPhong, CTHD.ThoiGianNhanPhong, CTHD.ThoiGianTraPhong " +
                    "FROM CHITIETHOADON CTHD " +
                    "WHERE CTHD.MaCTHD = @maCTHD ";
                MySqlCommand cmdGetCTHD = new MySqlCommand(queryGetCTHD, conn);
                cmdGetCTHD.Parameters.AddWithValue("maCTHD", maCTHD);
                using (MySqlDataReader readerGetCTHD = cmdGetCTHD.ExecuteReader())
                {
                    if (readerGetCTHD.HasRows)
                    {
                        readerGetCTHD.Read();
                        phieuThuePhong.MaCTHD = Convert.ToInt32(readerGetCTHD["MaCTHD"]);
                        phieuThuePhong.MaPhong = readerGetCTHD["MaPhong"].ToString();
                        phieuThuePhong.MaHoaDon = Convert.ToInt32(readerGetCTHD["MaHoaDon"]);
                        phieuThuePhong.PhuThuCICO = Convert.ToInt32(readerGetCTHD["PhuThuCICO"]);
                        phieuThuePhong.GiaPhong = Convert.ToInt32(readerGetCTHD["GiaPhong"]);
                        phieuThuePhong.TongTienPhong = Convert.ToInt32(readerGetCTHD["TongTienPhong"]);
                        phieuThuePhong.GhiChu = readerGetCTHD["GhiChu"].ToString();
                        phieuThuePhong.DsSoLuongKhachThue = new List<SoLuongKhachThue>();
                        phieuThuePhong.ThoiGianNhanPhong = Convert.ToDateTime(readerGetCTHD["ThoiGianNhanPhong"]);
                        phieuThuePhong.ThoiGianTraPhong = Convert.ToDateTime(readerGetCTHD["ThoiGianTraPhong"]);
                    }    
                }
                string queryGetSLKT = "SELECT SLKT.SoKhachThue, SLKT.PhuThu, SLKT.HeSoKhach, SLKT.SoNgayThue, SLKT.DonGia, SLKT.ThanhTien, SLKT.MaCTHD, SLKT.GhiChu " +
                    "FROM SOLUONGKHACHTHUE SLKT " +
                    "WHERE SLKT.MaCTHD = @maCTHD";
                MySqlCommand cmdGetSLKT = new MySqlCommand(queryGetSLKT, conn);
                cmdGetSLKT.Parameters.AddWithValue("maCTHD", maCTHD);
                using (MySqlDataReader readerGetSLKT = cmdGetSLKT.ExecuteReader())
                {
                    if (readerGetSLKT.HasRows)
                    {
                        while (readerGetSLKT.Read())
                        {
                            phieuThuePhong.DsSoLuongKhachThue.Add(new SoLuongKhachThue
                            {
                                SoKhachThue = Convert.ToByte(readerGetSLKT["SoKhachThue"]),
                                PhuThu = Convert.ToInt32(readerGetSLKT["PhuThu"]),
                                HeSoKhach = Convert.ToDouble(readerGetSLKT["HeSoKhach"]),
                                SoNgayThue = Convert.ToByte(readerGetSLKT["SoNgayThue"]),
                                DonGia = Convert.ToInt32(readerGetSLKT["DonGia"]),
                                ThanhTien = Convert.ToInt32(readerGetSLKT["ThanhTien"]),
                                MaCTHD = Convert.ToInt32(readerGetSLKT["MaCTHD"]),
                                GhiChu = readerGetSLKT["GhiChu"].ToString(),
                            });
                        }
                    }
                }
            }
            return phieuThuePhong;
        }

        public CTHD1Phong GetPhieuThuePhongDangThue (int maCTHD)
        {
            List<KhachThue> dsKhachThue = new List<KhachThue>();
            int _curMaCTHD = 0;
            int _curGiaPhong = 0;
            string _curMaPhong = "";
            DateTime _curTGNhanPhong = DateTime.Now;
            DateTime _curTGTraPhong = DateTime.Now;
            List<PhuThuLKH> dsPhuThuLKH = new List<PhuThuLKH>();
            List<PhuThu> dsPhuThuSoKhach = new List<PhuThu>();
            List<PhuThu> dsPhuThuCheckin = new List<PhuThu>();
            List<PhuThu> dsPhuThuCheckout = new List<PhuThu>();
            List<object> dsSoLuongTheoLoaiKhach = new List<object>();
            string[] checkInTime = { "00", "00" };
            string[] checkOutTime = { "00", "00" };
            int overCheckIn = 0;
            int overCheckOut = 0;
            int tiLePhuThuChechIn = 0;
            int tiLePhuThuChechOut = 0;

            CTHD1Phong CTHDPhong = new CTHD1Phong();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //Lấy chi tiết hóa đơn của phòng
                string queryCTHD = "SELECT CTHD.MaCTHD, CTHD.MaPhong, CTHD.ThoiGianNhanPhong, CTHD.GiaPhong " +
                    "FROM CHITIETHOADON CTHD " +
                    "WHERE CTHD.MaCTHD = @maCTHD ";
                MySqlCommand cmdCTHD = new MySqlCommand(queryCTHD, conn);
                cmdCTHD.Parameters.AddWithValue("maCTHD", maCTHD);
                using (MySqlDataReader readerCTHD = cmdCTHD.ExecuteReader())
                {
                    if (readerCTHD.HasRows)
                    {
                        readerCTHD.Read();
                        _curMaCTHD = Convert.ToInt32(readerCTHD["MaCTHD"]);
                        _curMaPhong = readerCTHD["MaPhong"].ToString();
                        _curTGNhanPhong = Convert.ToDateTime(readerCTHD["ThoiGianNhanPhong"]);
                        _curGiaPhong = Convert.ToInt32(readerCTHD["GiaPhong"]);
                    }
                }
                CTHDPhong.MaCTHD = _curMaCTHD;
                CTHDPhong.MaPhong = _curMaPhong;
                CTHDPhong.ThoiGianNhanPhong = _curTGNhanPhong;
                CTHDPhong.ThoiGianTraPhong = _curTGTraPhong;
                CTHDPhong.GiaPhong = _curGiaPhong;

                //Lấy quy định về phụ thu checkin
                string queryPhuThuCheckin = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Check In' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuCheckin = new MySqlCommand(queryPhuThuCheckin, conn);
                cmdPhuThuCheckin.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuCheckin = cmdPhuThuCheckin.ExecuteReader())
                {
                    if (readerPhuThuCheckin.HasRows)
                    {
                        while (readerPhuThuCheckin.Read())
                        {
                            dsPhuThuCheckin.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuCheckin["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuCheckin["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuCheckin["SoLuongApDung"]),
                                TenLoaiPhuThu = "Checkin",
                                ThoiGianApDung = readerPhuThuCheckin["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuCheckin["TiLePhuThu"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu checkout
                string queryPhuThuCheckout = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Check Out' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuCheckout = new MySqlCommand(queryPhuThuCheckout, conn);
                cmdPhuThuCheckout.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuCheckout = cmdPhuThuCheckout.ExecuteReader())
                {
                    if (readerPhuThuCheckout.HasRows)
                    {
                        while (readerPhuThuCheckout.Read())
                        {
                            dsPhuThuCheckout.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuCheckout["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuCheckout["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuCheckout["SoLuongApDung"]),
                                TenLoaiPhuThu = "Checkout",
                                ThoiGianApDung = readerPhuThuCheckout["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuCheckout["TiLePhuThu"])
                            });
                        }
                    }
                }

                //Lấy thông tin giờ checkin checkout
                string queryGioCheckInOut = "SELECT * " +
                    "FROM THAMSO TS " +
                    "WHERE TS.TenThuocTinh = 'GioCheckIn' OR TS.TenThuocTinh = 'GioCheckOut'";
                MySqlCommand cmdGioCheckInOut = new MySqlCommand(queryGioCheckInOut, conn);
                using (MySqlDataReader readerGioCheckInOut = cmdGioCheckInOut.ExecuteReader())
                {
                    if (readerGioCheckInOut.HasRows)
                    {
                        while (readerGioCheckInOut.Read())
                        {
                            if (readerGioCheckInOut["TenThuocTinh"].ToString() == "GioCheckIn")
                            {
                                checkInTime = readerGioCheckInOut["GiaTri"].ToString().Split(":");
                            }
                            else if (readerGioCheckInOut["TenThuocTinh"].ToString() == "GioCheckOut")
                            {
                                checkOutTime = readerGioCheckInOut["GiaTri"].ToString().Split(":");
                            }
                        }
                    }
                }
                //Tính checkin sớm, checkout trễ
                if (_curTGNhanPhong.Hour < Convert.ToInt32(checkInTime[0]))
                {
                    overCheckIn = (int)Math.Round((double)((Convert.ToInt32(checkInTime[0]) - _curTGNhanPhong.Hour) * 60 + (Convert.ToInt32(checkInTime[1]) - _curTGNhanPhong.Minute)) / 60);
                }
                if (_curTGTraPhong.Hour > Convert.ToInt32(checkOutTime[0]))
                {
                    overCheckOut = (int)Math.Round((double)((_curTGTraPhong.Hour - Convert.ToInt32(checkOutTime[0])) * 60 + (_curTGTraPhong.Minute) - Convert.ToInt32(checkOutTime[1])) / 60);
                }
                foreach (PhuThu phuThuCheckIn in dsPhuThuCheckin)
                {
                    if (phuThuCheckIn.SoLuongApDung <= overCheckIn && phuThuCheckIn.TiLePhuThu > tiLePhuThuChechIn)
                    {
                        tiLePhuThuChechIn = phuThuCheckIn.TiLePhuThu;
                    }
                }
                foreach (PhuThu phuThuCheckOut in dsPhuThuCheckout)
                {
                    if (phuThuCheckOut.SoLuongApDung <= overCheckOut && phuThuCheckOut.TiLePhuThu > tiLePhuThuChechOut)
                    {
                        tiLePhuThuChechOut = phuThuCheckOut.TiLePhuThu;
                    }
                }

                //Lấy danh sách khách thuê của phòng
                string queryKhachThue = "SELECT KT.MaKhachThue, KT.CCCD, KT.ThoiGianCheckIn, KT.ThoiGianCheckOut, KT.HoTen, KT.MaLoaiKhachHang, KT.DiaChi, KT.MaCTHD " +
                    "FROM KHACHTHUE KT, CHITIETHOADON CTHD " +
                    "WHERE KT.MaCTHD = CTHD.MaCTHD AND CTHD.MaPhong = @maPhong AND CTHD.MaHoaDon IS NULL AND CTHD.TrangThai = 0 ";
                MySqlCommand cmdKhachThue = new MySqlCommand(queryKhachThue, conn);
                cmdKhachThue.Parameters.AddWithValue("maPhong", _curMaPhong);
                using (MySqlDataReader readerKhachThue = cmdKhachThue.ExecuteReader())
                {
                    if (readerKhachThue.HasRows)
                    {
                        while (readerKhachThue.Read())
                        {
                            dsKhachThue.Add(new KhachThue
                            {
                                MaKhachThue = Convert.ToInt32(readerKhachThue["MaKhachThue"]),
                                CCCD = readerKhachThue["CCCD"].ToString(),
                                ThoiGianCheckin = Convert.ToDateTime(readerKhachThue["ThoiGianCheckIn"]),
                                ThoiGianCheckout = (readerKhachThue["ThoiGianCheckOut"] == DBNull.Value) ? (_curTGTraPhong) : Convert.ToDateTime(readerKhachThue["ThoiGianCheckOut"]),
                                HoTen = readerKhachThue["HoTen"].ToString(),
                                MaLoaiKhachHang = Convert.ToInt32(readerKhachThue["MaLoaiKhachHang"]),
                                DiaChi = readerKhachThue["DiaChi"].ToString(),
                                MaCTHD = Convert.ToInt32(readerKhachThue["MaCTHD"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu loại khách hàng
                string queryPhuThuLKH = "SELECT * " +
                    "FROM PHUTHULKH PTLKH " +
                    "WHERE PTLKH.ThoiGianApDung <= @thoiGianNhanPhong AND PTLKH.ThoiGianApDung = (SELECT MAX(PT.ThoiGianApDung) " +
                    "                                                                                FROM PHUTHULKH PT " +
                    "                                                                                WHERE PT.MaLoaiKhachHang = PTLKH.MaLoaiKhachHang) ";
                MySqlCommand cmdPhuThuLKH = new MySqlCommand(queryPhuThuLKH, conn);
                cmdPhuThuLKH.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuLKH = cmdPhuThuLKH.ExecuteReader())
                {
                    if (readerPhuThuLKH.HasRows)
                    {
                        while (readerPhuThuLKH.Read())
                        {
                            dsPhuThuLKH.Add(new PhuThuLKH
                            {
                                MaPhuThuLKH = Convert.ToInt32(readerPhuThuLKH["MaPhuThuLKH"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuLKH["SoLuongApDung"]),
                                HeSoPhuThu = Math.Round(Convert.ToDouble(readerPhuThuLKH["HeSoPhuThu"]), 1),
                                ThoiGianApDung = "",
                                MaLoaiKhachHang = Convert.ToInt32(readerPhuThuLKH["MaLoaiKhachHang"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu theo số khách 
                string queryPhuThuSoKhach = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Theo số khách' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuSoKhach = new MySqlCommand(queryPhuThuSoKhach, conn);
                cmdPhuThuSoKhach.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuSoKhach = cmdPhuThuSoKhach.ExecuteReader())
                {
                    if (readerPhuThuSoKhach.HasRows)
                    {
                        while (readerPhuThuSoKhach.Read())
                        {
                            dsPhuThuSoKhach.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuSoKhach["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuSoKhach["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuSoKhach["SoLuongApDung"]),
                                TenLoaiPhuThu = "Theo số khách",
                                ThoiGianApDung = readerPhuThuSoKhach["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuSoKhach["TiLePhuThu"])
                            });
                        }
                    }
                }




            }
            double _heSoPhuThuMax = 1;
            int _phuThuSoKhach = 0;

            List<SoLuongKhachThue> dsSLKTTungNgay = new List<SoLuongKhachThue>();
            List<SoLuongKhachThue> dsSLKTFinal = new List<SoLuongKhachThue>();
            if (_curTGNhanPhong.Date < _curTGTraPhong.Date)
            {
                for (DateTime date = _curTGNhanPhong; date.Date.CompareTo(_curTGTraPhong.Date) < 0; date = date.AddDays(1.0))
                {
                    var resultCount = from kt in dsKhachThue.Where(kt => kt.ThoiGianCheckin.Date <= date.Date && kt.ThoiGianCheckout.Date > date.Date)
                                                                .GroupBy(kt => kt.MaLoaiKhachHang)
                                      select new
                                      {
                                          count = kt.Count(),
                                          kt.First().MaLoaiKhachHang
                                      };
                    byte _countTongSoKhach = Convert.ToByte(dsKhachThue.Count(kt => kt.ThoiGianCheckin.Date <= date.Date && kt.ThoiGianCheckout.Date > date.Date));
                    foreach (var _reCount in resultCount)
                    {
                        foreach (var phuThuLKH in dsPhuThuLKH)
                        {
                            if (phuThuLKH.SoLuongApDung <= _reCount.count && _reCount.MaLoaiKhachHang == phuThuLKH.MaLoaiKhachHang && _heSoPhuThuMax < phuThuLKH.HeSoPhuThu)
                            {
                                _heSoPhuThuMax = phuThuLKH.HeSoPhuThu;
                            }
                        }
                    }
                    foreach (PhuThu phuThu in dsPhuThuSoKhach)
                    {
                        if (phuThu.SoLuongApDung <= _countTongSoKhach && phuThu.TiLePhuThu > _phuThuSoKhach)
                        {
                            _phuThuSoKhach = phuThu.TiLePhuThu;
                        }
                    }
                    dsSLKTTungNgay.Add(new SoLuongKhachThue
                    {
                        SoKhachThue = _countTongSoKhach,
                        PhuThu = _phuThuSoKhach,
                        HeSoKhach = _heSoPhuThuMax,
                    });
                    _countTongSoKhach = 0;
                    _heSoPhuThuMax = 1;
                    _phuThuSoKhach = 0;
                }
            }
            else if (_curTGNhanPhong.Date == _curTGTraPhong.Date)
            {
                var resultCount = from kt in dsKhachThue.GroupBy(kt => kt.MaLoaiKhachHang)
                                  select new
                                  {
                                      count = kt.Count(),
                                      kt.First().MaLoaiKhachHang
                                  };
                byte _countTongSoKhach = Convert.ToByte(dsKhachThue.Count());
                foreach (var _reCount in resultCount)
                {
                    foreach (var phuThuLKH in dsPhuThuLKH)
                    {
                        if (phuThuLKH.SoLuongApDung <= _reCount.count && _reCount.MaLoaiKhachHang == phuThuLKH.MaLoaiKhachHang && _heSoPhuThuMax < phuThuLKH.HeSoPhuThu)
                        {
                            _heSoPhuThuMax = phuThuLKH.HeSoPhuThu;
                        }
                    }
                }
                foreach (PhuThu phuThu in dsPhuThuSoKhach)
                {
                    if (phuThu.SoLuongApDung <= _countTongSoKhach && phuThu.TiLePhuThu > _phuThuSoKhach)
                    {
                        _phuThuSoKhach = phuThu.TiLePhuThu;
                    }
                }
                dsSLKTTungNgay.Add(new SoLuongKhachThue
                {
                    SoKhachThue = _countTongSoKhach,
                    PhuThu = _phuThuSoKhach,
                    HeSoKhach = _heSoPhuThuMax,
                });
                _countTongSoKhach = 0;
                _heSoPhuThuMax = 1;
                _phuThuSoKhach = 0;
            }
            tiLePhuThuChechOut = 0;

            if (dsSLKTTungNgay.Count > 0)
            {
                SoLuongKhachThue slktDangXet = new SoLuongKhachThue();
                int tongTienSLKT = 0;
                while (dsSLKTTungNgay.Count > 0)
                {
                    slktDangXet.SoKhachThue = dsSLKTTungNgay[0].SoKhachThue;
                    slktDangXet.PhuThu = dsSLKTTungNgay[0].PhuThu;
                    slktDangXet.HeSoKhach = dsSLKTTungNgay[0].HeSoKhach;
                    slktDangXet.SoNgayThue = 0;
                    slktDangXet.DonGia = _curGiaPhong;
                    slktDangXet.MaCTHD = _curMaCTHD;


                    for (int index = 0; index < dsSLKTTungNgay.Count; index++)
                    {
                        if (dsSLKTTungNgay[index].SoKhachThue == slktDangXet.SoKhachThue && dsSLKTTungNgay[index].PhuThu == slktDangXet.PhuThu && dsSLKTTungNgay[index].HeSoKhach == slktDangXet.HeSoKhach)
                        {
                            slktDangXet.SoNgayThue++;
                            dsSLKTTungNgay.RemoveAt(index);
                            index--;
                        }
                    }
                    slktDangXet.ThanhTien = (int)((double)(slktDangXet.HeSoKhach) * (slktDangXet.DonGia + (slktDangXet.DonGia * (slktDangXet.PhuThu / (double)100))) * slktDangXet.SoNgayThue);
                    tongTienSLKT = tongTienSLKT + slktDangXet.ThanhTien;
                    dsSLKTFinal.Add(new SoLuongKhachThue
                    {
                        SoKhachThue = slktDangXet.SoKhachThue,
                        PhuThu = (int)((slktDangXet.PhuThu / (double)100) * slktDangXet.DonGia),
                        HeSoKhach = slktDangXet.HeSoKhach,
                        SoNgayThue = slktDangXet.SoNgayThue,
                        DonGia = slktDangXet.DonGia,
                        ThanhTien = slktDangXet.ThanhTien,
                        MaCTHD = slktDangXet.MaCTHD,
                        GhiChu = "Phụ thu: " + slktDangXet.PhuThu.ToString() + "%\nHệ số khách: " + slktDangXet.HeSoKhach.ToString()
                    });
                }


                CTHDPhong.DsSoLuongKhachThue = dsSLKTFinal;
                CTHDPhong.PhuThuCICO = (int)(((double)(tiLePhuThuChechIn + tiLePhuThuChechOut) / 100) * CTHDPhong.GiaPhong);
                CTHDPhong.TongTienPhong = (int)(tongTienSLKT + CTHDPhong.PhuThuCICO);
                CTHDPhong.GhiChu = "Checkin sớm: " + tiLePhuThuChechIn.ToString() + "%\nCheckout trễ: Chưa tính";

                return CTHDPhong;
            }
            else return null;
        }
        public CTHD1Phong ThongKeSLKTBoPhong(int maCTHD)
        {
            List<KhachThue> dsKhachThue = new List<KhachThue>();
            int _curMaCTHD = 0;
            int _curGiaPhong = 0;
            string _curMaPhong = "";
            DateTime _curTGNhanPhong = DateTime.Now;
            DateTime _curTGTraPhong = DateTime.Now;
            List<PhuThuLKH> dsPhuThuLKH = new List<PhuThuLKH>();
            List<PhuThu> dsPhuThuSoKhach = new List<PhuThu>();
            List<PhuThu> dsPhuThuCheckin = new List<PhuThu>();
            List<PhuThu> dsPhuThuCheckout = new List<PhuThu>();
            List<object> dsSoLuongTheoLoaiKhach = new List<object>();
            string[] checkInTime = { "00", "00" };
            string[] checkOutTime = { "00", "00" };
            int overCheckIn = 0;
            int overCheckOut = 0;
            int tiLePhuThuChechIn = 0;
            int tiLePhuThuChechOut = 0;

            CTHD1Phong CTHDPhong = new CTHD1Phong();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //Lấy chi tiết hóa đơn của phòng
                string queryCTHD = "SELECT CTHD.MaCTHD, CTHD.MaPhong, CTHD.ThoiGianNhanPhong, CTHD.ThoiGianTraPhong, CTHD.GiaPhong " +
                    "FROM CHITIETHOADON CTHD " +
                    "WHERE CTHD.MaCTHD = @maCTHD ";
                MySqlCommand cmdCTHD = new MySqlCommand(queryCTHD, conn);
                cmdCTHD.Parameters.AddWithValue("maCTHD", maCTHD);
                using (MySqlDataReader readerCTHD = cmdCTHD.ExecuteReader())
                {
                    if (readerCTHD.HasRows)
                    {
                        readerCTHD.Read();
                        _curMaCTHD = Convert.ToInt32(readerCTHD["MaCTHD"]);
                        _curMaPhong = readerCTHD["MaPhong"].ToString();
                        _curTGNhanPhong = Convert.ToDateTime(readerCTHD["ThoiGianNhanPhong"]);
                        _curTGTraPhong = Convert.ToDateTime(readerCTHD["ThoiGianTraPhong"]);
                        _curGiaPhong = Convert.ToInt32(readerCTHD["GiaPhong"]);
                    }
                }
                CTHDPhong.MaCTHD = _curMaCTHD;
                CTHDPhong.MaPhong = _curMaPhong;
                CTHDPhong.ThoiGianNhanPhong = _curTGNhanPhong;
                CTHDPhong.ThoiGianTraPhong = _curTGTraPhong;
                CTHDPhong.GiaPhong = _curGiaPhong;

                //Lấy quy định về phụ thu checkin
                string queryPhuThuCheckin = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Check In' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuCheckin = new MySqlCommand(queryPhuThuCheckin, conn);
                cmdPhuThuCheckin.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuCheckin = cmdPhuThuCheckin.ExecuteReader())
                {
                    if (readerPhuThuCheckin.HasRows)
                    {
                        while (readerPhuThuCheckin.Read())
                        {
                            dsPhuThuCheckin.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuCheckin["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuCheckin["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuCheckin["SoLuongApDung"]),
                                TenLoaiPhuThu = "Checkin",
                                ThoiGianApDung = readerPhuThuCheckin["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuCheckin["TiLePhuThu"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu checkout
                string queryPhuThuCheckout = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Check Out' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuCheckout = new MySqlCommand(queryPhuThuCheckout, conn);
                cmdPhuThuCheckout.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuCheckout = cmdPhuThuCheckout.ExecuteReader())
                {
                    if (readerPhuThuCheckout.HasRows)
                    {
                        while (readerPhuThuCheckout.Read())
                        {
                            dsPhuThuCheckout.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuCheckout["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuCheckout["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuCheckout["SoLuongApDung"]),
                                TenLoaiPhuThu = "Checkout",
                                ThoiGianApDung = readerPhuThuCheckout["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuCheckout["TiLePhuThu"])
                            });
                        }
                    }
                }

                //Lấy thông tin giờ checkin checkout
                string queryGioCheckInOut = "SELECT * " +
                    "FROM THAMSO TS " +
                    "WHERE TS.TenThuocTinh = 'GioCheckIn' OR TS.TenThuocTinh = 'GioCheckOut'";
                MySqlCommand cmdGioCheckInOut = new MySqlCommand(queryGioCheckInOut, conn);
                using (MySqlDataReader readerGioCheckInOut = cmdGioCheckInOut.ExecuteReader())
                {
                    if (readerGioCheckInOut.HasRows)
                    {
                        while (readerGioCheckInOut.Read())
                        {
                            if (readerGioCheckInOut["TenThuocTinh"].ToString() == "GioCheckIn")
                            {
                                checkInTime = readerGioCheckInOut["GiaTri"].ToString().Split(":");
                            }
                            else if (readerGioCheckInOut["TenThuocTinh"].ToString() == "GioCheckOut")
                            {
                                checkOutTime = readerGioCheckInOut["GiaTri"].ToString().Split(":");
                            }
                        }
                    }
                }
                //Tính checkin sớm, checkout trễ
                if (_curTGNhanPhong.Hour < Convert.ToInt32(checkInTime[0]))
                {
                    overCheckIn = (int)Math.Round((double)((Convert.ToInt32(checkInTime[0]) - _curTGNhanPhong.Hour) * 60 + (Convert.ToInt32(checkInTime[1]) - _curTGNhanPhong.Minute)) / 60);
                }
                if (_curTGTraPhong.Hour > Convert.ToInt32(checkOutTime[0]))
                {
                    overCheckOut = (int)Math.Round((double)((_curTGTraPhong.Hour - Convert.ToInt32(checkOutTime[0])) * 60 + (_curTGTraPhong.Minute) - Convert.ToInt32(checkOutTime[1])) / 60);
                }
                foreach (PhuThu phuThuCheckIn in dsPhuThuCheckin)
                {
                    if (phuThuCheckIn.SoLuongApDung <= overCheckIn && phuThuCheckIn.TiLePhuThu > tiLePhuThuChechIn)
                    {
                        tiLePhuThuChechIn = phuThuCheckIn.TiLePhuThu;
                    }
                }
                foreach (PhuThu phuThuCheckOut in dsPhuThuCheckout)
                {
                    if (phuThuCheckOut.SoLuongApDung <= overCheckOut && phuThuCheckOut.TiLePhuThu > tiLePhuThuChechOut)
                    {
                        tiLePhuThuChechOut = phuThuCheckOut.TiLePhuThu;
                    }
                }

                //Lấy danh sách khách thuê của phòng
                string queryKhachThue = "SELECT KT.MaKhachThue, KT.CCCD, KT.ThoiGianCheckIn, KT.ThoiGianCheckOut, KT.HoTen, KT.MaLoaiKhachHang, KT.DiaChi, KT.MaCTHD " +
                    "FROM KHACHTHUE KT, CHITIETHOADON CTHD " +
                    "WHERE KT.MaCTHD = CTHD.MaCTHD AND CTHD.MaCTHD = @maCTHD AND CTHD.MaHoaDon IS NULL AND CTHD.TrangThai = 2 ";
                MySqlCommand cmdKhachThue = new MySqlCommand(queryKhachThue, conn);
                cmdKhachThue.Parameters.AddWithValue("maCTHD", maCTHD);
                using (MySqlDataReader readerKhachThue = cmdKhachThue.ExecuteReader())
                {
                    if (readerKhachThue.HasRows)
                    {
                        while (readerKhachThue.Read())
                        {
                            dsKhachThue.Add(new KhachThue
                            {
                                MaKhachThue = Convert.ToInt32(readerKhachThue["MaKhachThue"]),
                                CCCD = readerKhachThue["CCCD"].ToString(),
                                ThoiGianCheckin = Convert.ToDateTime(readerKhachThue["ThoiGianCheckIn"]),
                                ThoiGianCheckout = (readerKhachThue["ThoiGianCheckOut"] == DBNull.Value) ? (_curTGTraPhong) : Convert.ToDateTime(readerKhachThue["ThoiGianCheckOut"]),
                                HoTen = readerKhachThue["HoTen"].ToString(),
                                MaLoaiKhachHang = Convert.ToInt32(readerKhachThue["MaLoaiKhachHang"]),
                                DiaChi = readerKhachThue["DiaChi"].ToString(),
                                MaCTHD = Convert.ToInt32(readerKhachThue["MaCTHD"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu loại khách hàng
                string queryPhuThuLKH = "SELECT * " +
                    "FROM PHUTHULKH PTLKH " +
                    "WHERE PTLKH.ThoiGianApDung <= @thoiGianNhanPhong AND PTLKH.ThoiGianApDung = (SELECT MAX(PT.ThoiGianApDung) " +
                    "                                                                                FROM PHUTHULKH PT " +
                    "                                                                                WHERE PT.MaLoaiKhachHang = PTLKH.MaLoaiKhachHang) ";
                MySqlCommand cmdPhuThuLKH = new MySqlCommand(queryPhuThuLKH, conn);
                cmdPhuThuLKH.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuLKH = cmdPhuThuLKH.ExecuteReader())
                {
                    if (readerPhuThuLKH.HasRows)
                    {
                        while (readerPhuThuLKH.Read())
                        {
                            dsPhuThuLKH.Add(new PhuThuLKH
                            {
                                MaPhuThuLKH = Convert.ToInt32(readerPhuThuLKH["MaPhuThuLKH"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuLKH["SoLuongApDung"]),
                                HeSoPhuThu = Math.Round(Convert.ToDouble(readerPhuThuLKH["HeSoPhuThu"]), 1),
                                ThoiGianApDung = "",
                                MaLoaiKhachHang = Convert.ToInt32(readerPhuThuLKH["MaLoaiKhachHang"])
                            });
                        }
                    }
                }

                //Lấy quy định về phụ thu theo số khách 
                string queryPhuThuSoKhach = "SELECT PT.MaPhuThu, PT.SoLuongApDung, PT.TiLePhuThu, PT.MaLoaiPhuThu, PT.ThoiGianApDung " +
                    "FROM PHUTHU PT INNER JOIN LOAIPHUTHU LPT ON PT.MaLoaiPhuThu = LPT.MaLoaiPhuThu " +
                    "WHERE LPT.TenLoaiPhuThu = N'Theo số khách' AND PT.ThoiGianApDung <= @thoiGianNhanPhong " +
                    "						AND PT.ThoiGianApDung = (SELECT MAX(ThoiGianApDung)" +
                    "                                                 FROM PHUTHU PT1" +
                    "                                                 WHERE PT1.MaLoaiPhuThu = PT.MaLoaiPhuThu AND PT.SoLuongApDung = PT1.SoLuongApDung)";
                MySqlCommand cmdPhuThuSoKhach = new MySqlCommand(queryPhuThuSoKhach, conn);
                cmdPhuThuSoKhach.Parameters.AddWithValue("thoiGianNhanPhong", _curTGNhanPhong);
                using (MySqlDataReader readerPhuThuSoKhach = cmdPhuThuSoKhach.ExecuteReader())
                {
                    if (readerPhuThuSoKhach.HasRows)
                    {
                        while (readerPhuThuSoKhach.Read())
                        {
                            dsPhuThuSoKhach.Add(new PhuThu
                            {
                                MaLoaiPhuThu = Convert.ToInt32(readerPhuThuSoKhach["MaLoaiPhuThu"]),
                                MaPhuThu = Convert.ToInt32(readerPhuThuSoKhach["MaPhuThu"]),
                                SoLuongApDung = Convert.ToInt32(readerPhuThuSoKhach["SoLuongApDung"]),
                                TenLoaiPhuThu = "Theo số khách",
                                ThoiGianApDung = readerPhuThuSoKhach["ThoiGianApDung"].ToString(),
                                TiLePhuThu = Convert.ToInt32(readerPhuThuSoKhach["TiLePhuThu"])
                            });
                        }
                    }
                }




            }
            double _heSoPhuThuMax = 1;
            int _phuThuSoKhach = 0;

            List<SoLuongKhachThue> dsSLKTTungNgay = new List<SoLuongKhachThue>();
            List<SoLuongKhachThue> dsSLKTFinal = new List<SoLuongKhachThue>();
            if (_curTGNhanPhong.Date < _curTGTraPhong.Date)
            {
                for (DateTime date = _curTGNhanPhong; date.Date.CompareTo(_curTGTraPhong.Date) < 0; date = date.AddDays(1.0))
                {
                    var resultCount = from kt in dsKhachThue.Where(kt => kt.ThoiGianCheckin.Date <= date.Date && kt.ThoiGianCheckout.Date > date.Date)
                                                                .GroupBy(kt => kt.MaLoaiKhachHang)
                                      select new
                                      {
                                          count = kt.Count(),
                                          kt.First().MaLoaiKhachHang
                                      };
                    byte _countTongSoKhach = Convert.ToByte(dsKhachThue.Count(kt => kt.ThoiGianCheckin.Date <= date.Date && kt.ThoiGianCheckout.Date > date.Date));
                    foreach (var _reCount in resultCount)
                    {
                        foreach (var phuThuLKH in dsPhuThuLKH)
                        {
                            if (phuThuLKH.SoLuongApDung <= _reCount.count && _reCount.MaLoaiKhachHang == phuThuLKH.MaLoaiKhachHang && _heSoPhuThuMax < phuThuLKH.HeSoPhuThu)
                            {
                                _heSoPhuThuMax = phuThuLKH.HeSoPhuThu;
                            }
                        }
                    }
                    foreach (PhuThu phuThu in dsPhuThuSoKhach)
                    {
                        if (phuThu.SoLuongApDung <= _countTongSoKhach && phuThu.TiLePhuThu > _phuThuSoKhach)
                        {
                            _phuThuSoKhach = phuThu.TiLePhuThu;
                        }
                    }
                    dsSLKTTungNgay.Add(new SoLuongKhachThue
                    {
                        SoKhachThue = _countTongSoKhach,
                        PhuThu = _phuThuSoKhach,
                        HeSoKhach = _heSoPhuThuMax,
                    });
                    _countTongSoKhach = 0;
                    _heSoPhuThuMax = 1;
                    _phuThuSoKhach = 0;
                }
            }
            else if (_curTGNhanPhong.Date == _curTGTraPhong.Date)
            {
                var resultCount = from kt in dsKhachThue.GroupBy(kt => kt.MaLoaiKhachHang)
                                  select new
                                  {
                                      count = kt.Count(),
                                      kt.First().MaLoaiKhachHang
                                  };
                byte _countTongSoKhach = Convert.ToByte(dsKhachThue.Count());
                foreach (var _reCount in resultCount)
                {
                    foreach (var phuThuLKH in dsPhuThuLKH)
                    {
                        if (phuThuLKH.SoLuongApDung <= _reCount.count && _reCount.MaLoaiKhachHang == phuThuLKH.MaLoaiKhachHang && _heSoPhuThuMax < phuThuLKH.HeSoPhuThu)
                        {
                            _heSoPhuThuMax = phuThuLKH.HeSoPhuThu;
                        }
                    }
                }
                foreach (PhuThu phuThu in dsPhuThuSoKhach)
                {
                    if (phuThu.SoLuongApDung <= _countTongSoKhach && phuThu.TiLePhuThu > _phuThuSoKhach)
                    {
                        _phuThuSoKhach = phuThu.TiLePhuThu;
                    }
                }
                dsSLKTTungNgay.Add(new SoLuongKhachThue
                {
                    SoKhachThue = _countTongSoKhach,
                    PhuThu = _phuThuSoKhach,
                    HeSoKhach = _heSoPhuThuMax,
                });
                _countTongSoKhach = 0;
                _heSoPhuThuMax = 1;
                _phuThuSoKhach = 0;
                tiLePhuThuChechOut = 0;
            }

            if (dsSLKTTungNgay.Count > 0)
            {
                SoLuongKhachThue slktDangXet = new SoLuongKhachThue();
                int tongTienSLKT = 0;
                while (dsSLKTTungNgay.Count > 0)
                {
                    slktDangXet.SoKhachThue = dsSLKTTungNgay[0].SoKhachThue;
                    slktDangXet.PhuThu = dsSLKTTungNgay[0].PhuThu;
                    slktDangXet.HeSoKhach = dsSLKTTungNgay[0].HeSoKhach;
                    slktDangXet.SoNgayThue = 0;
                    slktDangXet.DonGia = _curGiaPhong;
                    slktDangXet.MaCTHD = _curMaCTHD;


                    for (int index = 0; index < dsSLKTTungNgay.Count; index++)
                    {
                        if (dsSLKTTungNgay[index].SoKhachThue == slktDangXet.SoKhachThue && dsSLKTTungNgay[index].PhuThu == slktDangXet.PhuThu && dsSLKTTungNgay[index].HeSoKhach == slktDangXet.HeSoKhach)
                        {
                            slktDangXet.SoNgayThue++;
                            dsSLKTTungNgay.RemoveAt(index);
                            index--;
                        }
                    }
                    slktDangXet.ThanhTien = (int)((double)(slktDangXet.HeSoKhach) * (slktDangXet.DonGia + (slktDangXet.DonGia * (slktDangXet.PhuThu / (double)100))) * slktDangXet.SoNgayThue);
                    tongTienSLKT = tongTienSLKT + slktDangXet.ThanhTien;
                    dsSLKTFinal.Add(new SoLuongKhachThue
                    {
                        SoKhachThue = slktDangXet.SoKhachThue,
                        PhuThu = (int)((slktDangXet.PhuThu / (double)100) * slktDangXet.DonGia),
                        HeSoKhach = slktDangXet.HeSoKhach,
                        SoNgayThue = slktDangXet.SoNgayThue,
                        DonGia = slktDangXet.DonGia,
                        ThanhTien = slktDangXet.ThanhTien,
                        MaCTHD = slktDangXet.MaCTHD,
                        GhiChu = "Phụ thu: " + slktDangXet.PhuThu.ToString() + "%\nHệ số khách: " + slktDangXet.HeSoKhach.ToString()
                    });
                }


                CTHDPhong.DsSoLuongKhachThue = dsSLKTFinal;
                CTHDPhong.PhuThuCICO = (int)(((double)(tiLePhuThuChechIn + tiLePhuThuChechOut) / 100) * CTHDPhong.GiaPhong);
                CTHDPhong.TongTienPhong = (int)(tongTienSLKT + CTHDPhong.PhuThuCICO);
                CTHDPhong.GhiChu = "Checkin sớm: " + tiLePhuThuChechIn.ToString() + "%\nCheckout trễ: " + tiLePhuThuChechOut.ToString()+"%";

                return CTHDPhong;
            }
            else return null;
        }

        public object HoaDonChoBoPhong(int maCTHD, string doiTuongThanhToan, string maNhanVien, string tenNhanVien)
        {
            CTHD1Phong hoaDon;
            object response;
            DateTime thoiGianXuatHD = DateTime.Now;
            hoaDon = ThongKeSLKTBoPhong(maCTHD);
            if (hoaDon != null)
            {
                int maHoaDon = 0;
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string queryInsertHoaDon = "INSERT INTO HOADON(ThoiGianXuat, MaNhanVien, TongSoTien, DoiTuongThanhToan) " +
                        "VALUES (@thoiGianXuat, @maNhanVien, @tongSoTien, @doiTuongThanhToan); " +
                        "SELECT LAST_INSERT_ID() AS 'MaHoaDon';";
                    MySqlCommand cmdInsertHoaDon = new MySqlCommand(queryInsertHoaDon, conn);
                    cmdInsertHoaDon.Parameters.AddWithValue("thoiGianXuat", thoiGianXuatHD);
                    cmdInsertHoaDon.Parameters.AddWithValue("maNhanVien", maNhanVien);
                    cmdInsertHoaDon.Parameters.AddWithValue("tongSoTien", hoaDon.TongTienPhong);
                    cmdInsertHoaDon.Parameters.AddWithValue("doiTuongThanhToan", doiTuongThanhToan);
                    using (MySqlDataReader readerInsertHoaDon = cmdInsertHoaDon.ExecuteReader())
                    {
                        if (readerInsertHoaDon.HasRows)
                        {
                            readerInsertHoaDon.Read();
                            maHoaDon = Convert.ToInt32(readerInsertHoaDon["MaHoaDon"]);
                        }
                    }

                    if (hoaDon.DsSoLuongKhachThue.Count > 0)
                    {
                        string queryInsertSLKT = "INSERT INTO SOLUONGKHACHTHUE(SoKhachThue, PhuThu, HeSoKhach, SoNgayThue, GhiChu, DonGia, ThanhTien, MaCTHD) VALUES ";
                        foreach (SoLuongKhachThue soLuongKhachThue in hoaDon.DsSoLuongKhachThue)
                        {
                            queryInsertSLKT = queryInsertSLKT + "(" + soLuongKhachThue.SoKhachThue + "," + soLuongKhachThue.PhuThu + ","
                                 + soLuongKhachThue.HeSoKhach + "," + soLuongKhachThue.SoNgayThue + ", N'" + soLuongKhachThue.GhiChu + "',"
                                 + soLuongKhachThue.DonGia + "," + soLuongKhachThue.ThanhTien + "," + soLuongKhachThue.MaCTHD + "),";
                        }
                        queryInsertSLKT = queryInsertSLKT.Remove(queryInsertSLKT.Length - 1);
                        MySqlCommand cmdInsertSLKT = new MySqlCommand(queryInsertSLKT, conn);
                        cmdInsertSLKT.ExecuteNonQuery();

                        string queryUpdateCTHD = "UPDATE CHITIETHOADON " +
                            "SET MaHoaDon = @maHoaDon, PhuThuCICO = @phuThuCICO, GhiChu = @ghiChu, TongTienPhong = @tongTienPhong, TrangThai = 1 " +
                            "WHERE MaCTHD = @maCTHD";
                        MySqlCommand cmdUpdateCTHD = new MySqlCommand(queryUpdateCTHD, conn);
                        cmdUpdateCTHD.Parameters.AddWithValue("maHoaDon", maHoaDon);
                        cmdUpdateCTHD.Parameters.AddWithValue("phuThuCICO", hoaDon.PhuThuCICO);
                        cmdUpdateCTHD.Parameters.AddWithValue("ghiChu", hoaDon.GhiChu);
                        cmdUpdateCTHD.Parameters.AddWithValue("tongTienPhong", hoaDon.TongTienPhong);
                        cmdUpdateCTHD.Parameters.AddWithValue("maCTHD", hoaDon.MaCTHD);
                        cmdUpdateCTHD.ExecuteNonQuery();
                    }
                }
                response = new
                {
                    maHoaDon = maHoaDon,
                    thoiGianXuat = thoiGianXuatHD,
                    doiTuongThanhToan = doiTuongThanhToan,
                    tenNhanVien = tenNhanVien,
                    dsChiTiet = hoaDon
                };
                return response;
            }
            return null;
        }

        public object GetCTHDBoPhong(int maCTHD)
        {
            object infoCTHD;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT CTHD.ThoiGianNhanPhong, CTHD.MaPhong, CTHD.ThoiGianTraPhong, CTHD.MaCTHD " +
                    "FROM CHITIETHOADON CTHD " +
                    "WHERE CTHD.MaCTHD = @maCTHD ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        infoCTHD = new
                        {
                            maPhong = reader["MaPhong"].ToString(),
                            maCTHD = Convert.ToInt32(reader["MaCTHD"]),
                            thoiGianNhanPhong = Convert.ToDateTime(reader["ThoiGianNhanPhong"]),
                            thoiGianTraPhong = Convert.ToDateTime(reader["ThoiGianTraPhong"])
                        };
                    }
                    else infoCTHD = null;
                }
            }
            return infoCTHD;
        }

        public string XoaPhieuThuePhongByID(int maCTHD)
        {
            using(MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string queryDelete = "HuyThuePhong";
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn);
                cmdDelete.CommandType = CommandType.StoredProcedure;

                cmdDelete.Parameters.AddWithValue("_maCTHD", maCTHD);
                cmdDelete.Parameters["_maCTHD"].Direction = ParameterDirection.Input;

                cmdDelete.Parameters.Add("_result", MySqlDbType.VarChar);
                cmdDelete.Parameters["_result"].Direction = ParameterDirection.Output;

                cmdDelete.ExecuteNonQuery();
                return cmdDelete.Parameters["_result"].Value.ToString();
            }
        }
    }
}
