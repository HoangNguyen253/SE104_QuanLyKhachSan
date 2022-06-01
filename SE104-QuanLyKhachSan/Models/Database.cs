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
                                                                  "AND GiaTienCoBan=@giaTienCoBan " +
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
                                                                  "AND PT1.ThoiGianApDung <= @now AND PT1.TiLePhuThu <> 0) " +
                                                              "OR (PT1.ThoiGianApDung > @now " +
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
                            PhuThu pt= new PhuThu();
                            pt.MaPhuThu= Convert.ToInt32(reader["MaPhuThu"]);
                            pt.TenLoaiPhuThu= reader["TenLoaiPhuThu"].ToString();
                            pt.TiLePhuThu = Convert.ToInt32(reader["TiLePhuThu"]);
                            pt.MaLoaiPhuThu= Convert.ToInt32(reader["MaLoaiPhuThu"]);
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
                    if(reader.HasRows)
                    {
                        string gioCheckIn = "";
                        string gioCheckOut = "";
                        while (reader.Read())
                        {
                            if (reader["TenThuocTinh"].ToString() == "GioCheckIn")
                            {
                                gioCheckIn = reader["GiaTri"].ToString();
                            } else
                            {
                                gioCheckOut = reader["GiaTri"].ToString();
                            }
                        }
                        return new {gioCheckIn = gioCheckIn, gioCheckOut = gioCheckOut};
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
                            cv.TenChucVu= reader["TenChucVu"].ToString();
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


        public bool UpdateLuongToiThieuVung(int luongToiThieuVung)
        {
            using (MySqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query= "UPDATE thamso " +
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
                            quyen.TenManHinhDuocLoad = reader["TenManHinhDuocLoad"].ToString();
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
    }
}
