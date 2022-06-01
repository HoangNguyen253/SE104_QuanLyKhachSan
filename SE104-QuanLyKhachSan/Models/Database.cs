using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
                    while(readerResult.Read())
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
                        infoOrderdRoom.Add( new
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
                        while(reader.Read())
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
    }
}
