using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using SE104_QuanLyKhachSan.Common;
using SE104_QuanLyKhachSan.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SE104_QuanLyKhachSan.Models
{
    public class Database
    {
        public string ConnectionString { get; set; }
        public object SessionKeyUser { get; private set; }

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

        struct ThongKeKhach
        {
            public string ngay;
            public byte KhachNoiDia;
            public byte KhachNN;
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
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getLPByMP, conn);
                cmd.Parameters.AddWithValue("MaPhong", maPhong);
                var result = cmd.ExecuteReader();
                if(result != null)
                {
                    result.Read();
                    LoaiPhong lp = new LoaiPhong();
                    lp.MaLoaiPhong = Convert.ToInt32(result["MaLoaiPhong"]);
                    lp.TenLoaiPhong = result["TenLoaiPhong"].ToString();
                    lp.GiaTienCoBan = Convert.ToInt32(result["GiaTienCoBan"]);
                    lp.DaXoa = Convert.ToByte(result["DaXoa"]);
                    conn.Close();
                    return lp;
                } else
                {
                    conn.Close ();
                    return null;
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
                            detail.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                            detail.TrangThai = Convert.ToByte(result["TrangThai"]);
                            if(result["MaHoaDon"] == DBNull.Value)
                            {
                                detail.MaHoaDon = null;
                            } else
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
                using(var result = cmd.ExecuteReader())
                {
                    while(result.Read())
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
                        detail.PhuThuCICO = Convert.ToInt32(result["PhuThuCICO"]);
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
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getHoaDon, conn);
                cmd.Parameters.AddWithValue("@maHoaDon", MaHoaDon);
                var result = cmd.ExecuteReader();
                result.Read();
                if(result != null)
                {
                    HoaDon hoaDon = new HoaDon();
                    hoaDon.NV = GetNV(result["MaNhanVien"].ToString());
                    hoaDon.ThoiGianXuat = Convert.ToDateTime(result["ThoiGianXuat"]);
                    hoaDon.TongSoTien = Convert.ToInt32(result["TongSoTien"]);
                    hoaDon.DoiTuongThanhToan = result["DoiTuongThanhToan"].ToString();
                    hoaDon.ChiTiet = GetDetailById(MaHoaDon);
                    return hoaDon;
                } else
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
                if(result.Read())
                {
                    ChiTietHoaDon ct = new ChiTietHoaDon();
                    ct.MaCTHD = Convert.ToInt16(result["MaCTHD"]);
                    ct.MaPhong = result["MaPhong"].ToString();
                    LoaiPhong lp = GetLPByMP(ct.MaPhong);
                    ct.LoaiPhong = lp;
                    ct.MaHoaDon = null;
                    ct.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                    ct.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
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
                if(detail == null)
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
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                bool checkRoom = CheckInsertDetailToBill(roomID);
                if(checkRoom == true)
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

        public double PhuThuCI(DateTime thoiGianNhanPhong)
        {
            if(thoiGianNhanPhong.Hour < 14)
            {
                if(Math.Abs(thoiGianNhanPhong.Hour - 14) <= 3)
                {
                    return 0.3;
                } else if (thoiGianNhanPhong.Hour - 14 <= 5)
                {
                    return 0.5;
                } else
                {
                    return 1;
                }
            } else
            {
                return 0;
            }
        }

        public double PhuThuCO(DateTime thoiGianTraPhong)
        {
            if(thoiGianTraPhong.Hour > 12)
            {
                if(Math.Abs(thoiGianTraPhong.Hour - 12) < 3)
                {
                    return 0.3;
                } else if(Math.Abs(thoiGianTraPhong.Hour - 12) <= 5)
                {
                    return 0.5;
                } else
                {
                    return 1;
                }
            } else { return 0; }
        }

        public int SaveListDetailBill(String maNV, String doiTuong)
        {
            if (pendingList != null)
            {
                using(MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open ();
                    HoaDon hoaDonMoi = new HoaDon();

                    MySqlCommand pendBill = new MySqlCommand (SQLQuery.insertPendingBill, conn);
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
                        List<SoLuongKhachThue> listSLKT = ConvertToSLKT(mact);
                        foreach(SoLuongKhachThue luongKhach in listSLKT)
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
                if(result.HasRows)
                {
                    while(result.Read())
                    {
                        KhachThue khachThue = new KhachThue();
                        khachThue.MaKhachThue = Convert.ToInt32(result["MaKhachThue"]);
                        khachThue.CCCD = result["CCCD"].ToString();
                        khachThue.ThoiGianCheckin = Convert.ToDateTime(result["ThoiGianCheckIn"]);
                        if(result["ThoiGianCheckOut"] != DBNull.Value)
                        {
                            khachThue.ThoiGianCheckout = Convert.ToDateTime(result["ThoiGianCheckOut"]);
                        } else
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
                } else
                {
                    conn.Close();
                    return null;
                }
            }
        }

        public List<SoLuongKhachThue> GetListSoLuong(int maCTHD)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQLQuery.getListSoLuong, conn);
                cmd.Parameters.AddWithValue("maCTHD", maCTHD);
                var result = cmd.ExecuteReader();
                List<SoLuongKhachThue> kq = new List<SoLuongKhachThue>();
                if(result != null)
                {
                    while(result.Read())
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
                } else
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
                if(result.Read())
                {
                    ChiTietHoaDon kq = new ChiTietHoaDon();
                    kq.MaCTHD = maCTHD;
                    kq.MaPhong = result["MaPhong"].ToString();
                    if(result["MaHoaDon"] == DBNull.Value)
                    {
                        kq.MaHoaDon = null;
                    } else
                    {
                        kq.MaHoaDon = Convert.ToInt32(result["MaHoaDon"]);
                    }
                    kq.ThoiGianNhanPhong = Convert.ToDateTime(result["ThoiGianNhanPhong"]);
                    kq.ThoiGianTraPhong = Convert.ToDateTime(result["ThoiGianTraPhong"]);
                    kq.GiaPhong = Convert.ToInt32(result["GiaPhong"]);
                    if(result["PhuThuCICO"] == DBNull.Value)
                    {
                        kq.PhuThuCICO = 0;
                    } else
                    {
                        kq.PhuThuCICO = Convert.ToInt32(result["PhuThuCICO"]);
                    }
                    kq.GhiChu = result["GhiChu"].ToString();
                    kq.TongTienPhong = Convert.ToInt32(result["TongTienPhong"]);
                    kq.TrangThai = Convert.ToByte(result["TrangThai"]);
                    kq.DsKhachThue = GetListKhachThue(maCTHD);
                    kq.DsSoLuong = GetListSoLuong(maCTHD);
                    return kq;
                } else
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
                cmd.Parameters.AddWithValue("thoiGianCO", DateTime.Now);
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
                if(diff.Hours > 10)
                {
                    for(int i = 0; i<=diff.Days + 1; i++)
                    {
                        days.Add(checkIn.Date.AddDays(i));
                    }
                } else
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
                foreach(ThongKeKhach record in listThongKe)
                {
                    // Struct key = KhachNoiDia + KhachNN
                    string key = record.KhachNoiDia.ToString() + record.KhachNN.ToString();
                    
                    if(dictCount.ContainsKey(key))
                    {
                        dictCount[key]++;
                    } else
                    {
                        dictCount.Add(key, 1);
                    }
                }
                // Da co duoc Dict so luong khach theo loai khach va theo so luong ngay
                List<SoLuongKhachThue> kq = new List<SoLuongKhachThue> ();
                foreach(KeyValuePair<string, byte> entry in dictCount)
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
                    if(history.SoKhachThue > 2)
                    {
                        cmd = new MySqlCommand(SQLQuery.getPhuThuTheoSoLuong, conn);
                        cmd.Parameters.AddWithValue("soLuong", history.SoKhachThue);
                        cmd.Parameters.AddWithValue("thoiGianCI", checkInString);
                        var phuthu = cmd.ExecuteReader();
                        phuthu.Read();
                        history.PhuThu = Convert.ToInt32(phuthu["TiLePhuThu"]);
                        phuthu.Close();
                    }

                    //Tinh he so lkh
                    cmd = new MySqlCommand(SQLQuery.getHeSoLKH, conn);
                    cmd.Parameters.AddWithValue("thoiGianCI", checkIn);
                    cmd.Parameters.AddWithValue("soKhachNN", history.SoKhachNN);
                    cmd.Parameters.AddWithValue("soKhachND", Convert.ToByte(key[0].ToString()));
                    var heso = cmd.ExecuteReader();
                    heso.Read();
                    if(heso != null)
                    { 
                        history.HeSoKhach = (float)Convert.ToDouble(heso["HeSo"]);
                    } else
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
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand del = new MySqlCommand(SQLQuery.deleteDetailByID, conn);
                del.Parameters.AddWithValue("maCTHD", maCTHD);
                del.ExecuteNonQuery();
            }
        }

        public void UpdateCancelStatusDetail(int maCTHD)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand update = new MySqlCommand(SQLQuery.updateCancelStatusDetail, conn);
                update.Parameters.AddWithValue("maCTHD", maCTHD);
                update.ExecuteNonQuery();
            }
        }
    }
}
